using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// TynassIt — Modules Panel Logic
/// Loads trainings from backend using deviceToken
/// and populates ModulesLayout with real data.
/// Attach on the same GameObject as ModulesLayout.
/// </summary>
public class ModulesPanel : MonoBehaviour
{
    [Header("Scene to load on Start Simulation")]
    [SerializeField] private string simulationSceneName = "intigo";

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject aboutPanel;

    private ModulesLayout _layout;
    private List<TrainingData> _trainings = new List<TrainingData>();
    private int _selectedIndex = -1;

    async void OnEnable()
    {
        _layout = GetComponent<ModulesLayout>();

        if (_layout == null)
        {
            Debug.LogError("[ModulesPanel] ModulesLayout not found on same GameObject.");
            return;
        }

        SetLoadingState(true);
        await LoadTrainings();

        // Wire bottom nav buttons
        if (_layout.StartSimBtn != null)
            _layout.StartSimBtn.onClick.AddListener(OnStartSimulation);

        if (_layout.SettingsBtn != null)
            _layout.SettingsBtn.onClick.AddListener(OnSettingsClicked);

        if (_layout.OverviewBtn != null)
            _layout.OverviewBtn.onClick.AddListener(OnOverviewClicked);
    }

    void OnDisable()
    {
        // Clean up listeners
        if (_layout == null) return;
        _layout.StartSimBtn?.onClick.RemoveAllListeners();
        _layout.SettingsBtn?.onClick.RemoveAllListeners();
        _layout.OverviewBtn?.onClick.RemoveAllListeners();
    }

    // ─────────────────────────────────────────
    // LOAD TRAININGS FROM BACKEND
    // ─────────────────────────────────────────
    private async Task LoadTrainings()
    {
        string deviceToken = PlayerPrefs.GetString("device_token", null);

        if (string.IsNullOrEmpty(deviceToken))
        {
            Debug.LogError("[ModulesPanel] No device token found.");
            SetLoadingState(false);
            return;
        }

        var response = await TynassApiClient.Instance.GetDeviceTrainings(deviceToken);

        SetLoadingState(false);

        if (response == null || response.Count == 0)
        {
            Debug.LogWarning("[ModulesPanel] No trainings found.");
            if (_layout.SectionCountLabel != null)
                _layout.SectionCountLabel.text = "No modules assigned";
            return;
        }

        _trainings = response;
        AppSession.AssignedTrainings = _trainings;
        Debug.Log($"[ModulesPanel] Loaded {_trainings.Count} trainings.");

        PopulateModules();

        // Auto-select first
        if (_trainings.Count > 0)
            SelectTraining(0);
    }

    // ─────────────────────────────────────────
    // POPULATE CARDS
    // ─────────────────────────────────────────
    private void PopulateModules()
    {
        // Section count
        if (_layout.SectionCountLabel != null)
            _layout.SectionCountLabel.text = $"{_trainings.Count} module{(_trainings.Count > 1 ? "s" : "")} assigned";

        // Company info
        if (_layout.CompanyName != null)
            _layout.CompanyName.text = AppSession.CompanyName ?? "";

        if (_layout.CompanyInitials != null)
            _layout.CompanyInitials.text = GetInitials(AppSession.CompanyName ?? "CO");

        // Cards — ModulesLayout uses Cards[] array
        if (_layout.Cards == null) return;

        for (int i = 0; i < _layout.Cards.Length; i++)
        {
            var card = _layout.Cards[i];
            if (card == null) continue;

            if (i < _trainings.Count)
            {
                var training = _trainings[i];

                // Make sure card is visible
                if (card.Card != null)
                    card.Card.gameObject.SetActive(true);

                // Update texts
                if (card.ModuleName != null)
                    card.ModuleName.text = training.title;

                if (card.ModuleDesc != null)
                    card.ModuleDesc.text = training.description;

                if (card.BadgeLabel != null)
                    card.BadgeLabel.text = training.category?.ToUpper();

                // Wire card click
                int index = i;
                card.CardButton?.onClick.RemoveAllListeners();
                card.CardButton?.onClick.AddListener(() => SelectTraining(index));
            }
            else
            {
                // Hide unused cards
                if (card.Card != null)
                    card.Card.gameObject.SetActive(false);
            }
        }
    }

    // ─────────────────────────────────────────
    // SELECT TRAINING
    // ─────────────────────────────────────────
    private void SelectTraining(int index)
    {
        if (index < 0 || index >= _trainings.Count) return;

        _selectedIndex = index;
        AppSession.ActiveTraining = _trainings[index];

        Debug.Log($"[ModulesPanel] Selected: {_trainings[index].title}");

        // Update Start button label
        if (_layout.StartSimLabel != null)
            _layout.StartSimLabel.text = $"▶  Start Simulation";

        // Highlight selected card
        if (_layout.Cards == null) return;
        for (int i = 0; i < _layout.Cards.Length; i++)
        {
            var card = _layout.Cards[i];
            if (card?.CardBorder != null)
                card.CardBorder.effectColor = i == index
                    ? new Color(0f, 0.282f, 1f, 0.9f)
                    : new Color(1f, 1f, 1f, 0.10f);
        }
    }

    // ─────────────────────────────────────────
    // BOTTOM NAV
    // ─────────────────────────────────────────
    private void OnStartSimulation()
    {
        if (_selectedIndex < 0)
        {
            Debug.LogWarning("[ModulesPanel] No training selected.");
            return;
        }

        Debug.Log($"[ModulesPanel] Starting: {AppSession.ActiveTraining?.title}");
        SceneManager.LoadScene(simulationSceneName);
    }

    private void OnSettingsClicked()
    {
        if (settingsPanel != null)
        {
            gameObject.SetActive(false);
            settingsPanel.SetActive(true);
        }
    }

    private void OnOverviewClicked()
    {
        if (aboutPanel != null)
        {
            gameObject.SetActive(false);
            aboutPanel.SetActive(true);
        }
    }

    // ─────────────────────────────────────────
    // HELPERS
    // ─────────────────────────────────────────
    private void SetLoadingState(bool loading)
    {
        if (_layout?.StartSimBtn != null)
            _layout.StartSimBtn.interactable = !loading;
    }

    private string GetInitials(string name)
    {
        var parts = name.Trim().Split(' ');
        if (parts.Length >= 2)
            return $"{parts[0][0]}{parts[1][0]}".ToUpper();
        return name.Substring(0, Mathf.Min(2, name.Length)).ToUpper();
    }
}
