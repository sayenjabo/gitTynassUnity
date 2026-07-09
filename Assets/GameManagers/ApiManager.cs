// ApiManager.cs — Singleton réseau
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance { get; private set; }

    // ─── Config ──────────────────────────────────────────────
    // En développement : ton backend local ou déployé
    private const string BASE_URL = "https://ton-backend.com";
    private string _jwtToken = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetToken(string token) => _jwtToken = token;
    public bool IsAuthenticated => !string.IsNullOrEmpty(_jwtToken);

    // ─── Login ───────────────────────────────────────────────
    public IEnumerator Login(string email, string password,
        Action<LoginResponse> onSuccess,
        Action<string> onError)
    {
        var body = JsonUtility.ToJson(new LoginRequest
        {
            email = email,
            password = password
        });

        using var req = new UnityWebRequest(
            BASE_URL + "/api/company/auth/login", "POST");
        req.uploadHandler = new UploadHandlerRaw(
            System.Text.Encoding.UTF8.GetBytes(body));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        // Timeout — critique pour le réseau d'entreprise
        req.timeout = 10;

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<LoginResponse>(
                req.downloadHandler.text);
            SetToken(response.token);
            onSuccess?.Invoke(response);
        }
        else
        {
            // Distingue timeout vs serveur vs réseau
            string errorMsg = req.result switch
            {
                UnityWebRequest.Result.ConnectionError =>
                    "Impossible de contacter le serveur. Vérifiez votre connexion.",
                UnityWebRequest.Result.ProtocolError =>
                    "Email ou mot de passe incorrect.",
                UnityWebRequest.Result.DataProcessingError =>
                    "Erreur de données. Contactez Tynass.",
                _ => "Erreur inconnue."
            };
            onError?.Invoke(errorMsg);
        }
    }

    // ─── Submit Session ──────────────────────────────────────
    public IEnumerator SubmitSession(SessionPayload payload,
        Action onSuccess,
        Action<string> onError)
    {
        var body = JsonUtility.ToJson(payload);

        using var req = new UnityWebRequest(
            BASE_URL + "/api/sessions/submit", "POST");
        req.uploadHandler = new UploadHandlerRaw(
            System.Text.Encoding.UTF8.GetBytes(body));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + _jwtToken);
        req.timeout = 15;

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke();
        else
            onError?.Invoke("Erreur envoi session : " + req.error);
    }
}

// ─── Data structures ─────────────────────────────────────────

[Serializable]
public class LoginRequest
{
    public string email;
    public string password;
}

[Serializable]
public class LoginResponse
{
    public string token;
    public CompanyData company;
    public TrainingData[] trainings;
}

[Serializable]
public class CompanyData
{
    public string id;
    public string companyName;
    public string email;
}

[Serializable]
public class TrainingData
{
    public string _id;
    public string title;
    public string category;
    public string description;
}

[Serializable]
public class SessionPayload
{
    public string trainingId;
    public string startedAt;
    public string completedAt;
    public float score;
    public bool passed;
    public string employeeName;
    public string employeeUniqueId;
    public EvaluationCriteria[] evaluationCriteria;
}