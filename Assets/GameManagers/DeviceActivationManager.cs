// DeviceActivationManager.cs
// Attacher sur GameManager dans la scène Companys

using System.Collections;
using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

public class DeviceActivationManager : MonoBehaviour
{
    public static DeviceActivationManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject activationCodePanel;
    [SerializeField] private GameObject employeePickerPanel;

    [Header("Activation Code UI")]
    [SerializeField] private TMPro.TMP_Text activationCodeText;
    [SerializeField] private TMPro.TMP_Text activationStatusText;
    [SerializeField] private TMPro.TMP_Text activationTimerText;

    private string _metaUserId;
    private float  _activationTimer = 900f; // 15 minutes

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        loadingPanel?.SetActive(true);
        activationCodePanel?.SetActive(false);
        employeePickerPanel?.SetActive(false);

        // Charger le deviceToken stocké si disponible
        string storedToken = PlayerPrefs.GetString("device_token", null);
        if (!string.IsNullOrEmpty(storedToken))
            AppSession.DeviceToken = storedToken;

        StartCoroutine(InitializeDevice());
    }

    // ─────────────────────────────────────────
    // ÉTAPE 1 — Récupérer le Meta User ID
    // ─────────────────────────────────────────
    private IEnumerator InitializeDevice()
    {
        Debug.Log("[DeviceActivation] Initialisation Meta Platform...");

        Core.Initialize();
        yield return new WaitUntil(() => Core.IsInitialized());

        bool userFetched = false;

        Users.GetLoggedInUser().OnComplete((Message<User> msg) =>
        {
            if (msg.IsError)
            {
                Debug.LogWarning($"[DeviceActivation] Erreur Meta User ID : {msg.GetError().Message}");
                _metaUserId = SystemInfo.deviceUniqueIdentifier; // fallback Editor
            }
            else
            {
                _metaUserId = msg.Data.ID.ToString();
                Debug.Log($"[DeviceActivation] Meta User ID : {_metaUserId}");
            }
            userFetched = true;
        });

        yield return new WaitUntil(() => userFetched);

        yield return StartCoroutine(CheckDevice());
    }

    // ─────────────────────────────────────────
    // ÉTAPE 2 — Vérifier si le casque est activé
    // ─────────────────────────────────────────
    private IEnumerator CheckDevice()
    {
        Debug.Log("[DeviceActivation] Vérification du casque...");

        var checkTask = TynassApiClient.Instance.CheckDevice(_metaUserId);
        yield return new WaitUntil(() => checkTask.IsCompleted);

        var result = checkTask.Result;

        if (result == null)
        {
            Debug.LogWarning("[DeviceActivation] Erreur réseau — demande d'activation");
            yield return StartCoroutine(RequestActivation());
            yield break;
        }

        if (result.status == "activated" || result.status == "already_activated")
        {
            string companyId = result.company?.id ?? result.companyId;
            Debug.Log($"[DeviceActivation] Casque activé ✅ — Company : {companyId}");
            AppSession.CompanyId = companyId;

            // Stocker le deviceToken si présent (retourné une seule fois)
            if (!string.IsNullOrEmpty(result.deviceToken))
            {
                PlayerPrefs.SetString("device_token", result.deviceToken);
                PlayerPrefs.Save();
                Debug.Log("[DeviceActivation] DeviceToken stocké ✅");
            }

            loadingPanel?.SetActive(false);
            employeePickerPanel?.SetActive(true);
        }
        else if (result.status == "suspended")
        {
            Debug.LogWarning("[DeviceActivation] Entreprise suspendue.");
            if (activationStatusText != null)
                activationStatusText.text = "Accès suspendu. Contactez TynassIt.";
            activationCodePanel?.SetActive(true);
            loadingPanel?.SetActive(false);
        }
        else // "not_activated" ou autre
        {
            yield return StartCoroutine(RequestActivation());
        }
    }

    // ─────────────────────────────────────────
    // ÉTAPE 3 — Demander un code d'activation
    // ─────────────────────────────────────────
    private IEnumerator RequestActivation()
    {
        Debug.Log("[DeviceActivation] Demande de code d'activation...");

        var requestTask = TynassApiClient.Instance.RequestActivation(_metaUserId);
        yield return new WaitUntil(() => requestTask.IsCompleted);

        var result = requestTask.Result;

        loadingPanel?.SetActive(false);

        if (result == null)
        {
            Debug.LogError("[DeviceActivation] Impossible de contacter le serveur.");
            if (activationStatusText != null)
                activationStatusText.text = "Impossible de contacter le serveur.\nVérifiez votre connexion.";
            activationCodePanel?.SetActive(true);
            yield break;
        }

        activationCodePanel?.SetActive(true);

        if (activationCodeText != null)
            activationCodeText.text = result.activationCode;

        if (activationStatusText != null)
            activationStatusText.text = "Montrez ce code à votre administrateur.\nValide 15 minutes.";

        Debug.Log($"[DeviceActivation] Code d'activation : {result.activationCode}");

        // Démarrer le timer et le polling en parallèle
        StartCoroutine(CountdownTimer());
        StartCoroutine(PollForActivation());
    }

    // ─────────────────────────────────────────
    // TIMER — Compte à rebours 15 minutes
    // ─────────────────────────────────────────
    private IEnumerator CountdownTimer()
    {
        _activationTimer = 900f;

        while (_activationTimer > 0f)
        {
            _activationTimer -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(_activationTimer / 60f);
            int seconds = Mathf.FloorToInt(_activationTimer % 60f);

            if (activationTimerText != null)
                activationTimerText.text = $"Expire dans {minutes:00}:{seconds:00}";

            yield return null;
        }

        // Timer expiré — redemander un nouveau code
        if (activationTimerText != null)
            activationTimerText.text = "Code expiré — renouvellement...";

        yield return StartCoroutine(RequestActivation());
    }

    // ─────────────────────────────────────────
    // POLLING — Vérifier toutes les 10s
    // ─────────────────────────────────────────
    private IEnumerator PollForActivation()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            Debug.Log("[DeviceActivation] Polling — vérification activation...");

            var checkTask = TynassApiClient.Instance.CheckDevice(_metaUserId);
            yield return new WaitUntil(() => checkTask.IsCompleted);

            var result = checkTask.Result;

            if (result?.status == "activated" || result?.status == "already_activated")
            {
                string companyId = result.company?.id ?? result.companyId;
                Debug.Log($"[DeviceActivation] Casque activé ! ✅ Company: {companyId}");
                AppSession.CompanyId = companyId;

                // Stocker le deviceToken si présent (retourné une seule fois)
                if (!string.IsNullOrEmpty(result.deviceToken))
                {
                    PlayerPrefs.SetString("device_token", result.deviceToken);
                    PlayerPrefs.Save();
                    Debug.Log("[DeviceActivation] DeviceToken stocké ✅");
                }

                StopAllCoroutines();
                activationCodePanel?.SetActive(false);
                employeePickerPanel?.SetActive(true);
                yield break;
            }
        }
    }
}
