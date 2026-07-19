using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// TynassIt - Settings Panel Logic
/// Assign all refs in Inspector. No auto-build. No FindExistingRefs.
/// </summary>
public class SettingsPanel : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private GameObject modulesPanel;

    [Header("Back Button")]
    [SerializeField] private Button backBtn;

    [Header("Sidebar Buttons")]
    [SerializeField] private Button sidebarAudio;
    [SerializeField] private Button sidebarLanguage;
    [SerializeField] private Button sidebarController;
    [SerializeField] private Button sidebarSignOut;
    [SerializeField] private Image  sidebarAudioBg;
    [SerializeField] private Image  sidebarLanguageBg;
    [SerializeField] private Image  sidebarControllerBg;

    [Header("Panels")]
    [SerializeField] private GameObject panelAudio;
    [SerializeField] private GameObject panelLanguage;
    [SerializeField] private GameObject panelController;

    [Header("Audio - Master Volume")]
    [SerializeField] private Button  masterMinus;
    [SerializeField] private Button  masterPlus;
    [SerializeField] private RectTransform masterFill;
    [SerializeField] private TMP_Text masterVal;

    [Header("Audio - SFX Volume")]
    [SerializeField] private Button  sfxMinus;
    [SerializeField] private Button  sfxPlus;
    [SerializeField] private RectTransform sfxFill;
    [SerializeField] private TMP_Text sfxVal;

    [Header("Audio - Narration Volume")]
    [SerializeField] private Button  narrMinus;
    [SerializeField] private Button  narrPlus;
    [SerializeField] private RectTransform narrFill;
    [SerializeField] private TMP_Text narrVal;

    [Header("Audio - Toggles")]
    [SerializeField] private Button muteToggle;
    [SerializeField] private Image  muteBg;
    [SerializeField] private RectTransform muteKnob;
    [SerializeField] private Button spatialToggle;
    [SerializeField] private Image  spatialBg;
    [SerializeField] private RectTransform spatialKnob;

    [Header("Language - UI")]
    [SerializeField] private Button langFR;
    [SerializeField] private Button langEN;
    [SerializeField] private Button langAR;

    [Header("Language - Narration")]
    [SerializeField] private Button narrFR;
    [SerializeField] private Button narrEN;
    [SerializeField] private Button narrAR;

    [Header("Language - Subtitles")]
    [SerializeField] private Button subtitleToggle;
    [SerializeField] private Image  subtitleBg;
    [SerializeField] private RectTransform subtitleKnob;

    [Header("Controller - Hand")]
    [SerializeField] private Button handRight;
    [SerializeField] private Button handLeft;

    [Header("Controller - Glow")]
    [SerializeField] private Button glowToggle;
    [SerializeField] private Image  glowBg;
    [SerializeField] private RectTransform glowKnob;

    [Header("Controller - Haptic")]
    [SerializeField] private Button hapticToggle;
    [SerializeField] private Image  hapticBg;
    [SerializeField] private RectTransform hapticKnob;

    [Header("Controller - Selection Mode")]
    [SerializeField] private Button selectTrigger;
    [SerializeField] private Button selectDwell;

    [Header("Controller - Dwell Time")]
    [SerializeField] private Button dwellMinus;
    [SerializeField] private Button dwellPlus;
    [SerializeField] private RectTransform dwellFill;
    [SerializeField] private TMP_Text dwellVal;

    [Header("Controller - Sound")]
    [SerializeField] private Button soundToggle;
    [SerializeField] private Image  soundBg;
    [SerializeField] private RectTransform soundKnob;

    [Header("Bottom Nav")]
    [SerializeField] private Button signOutBtn;
    [SerializeField] private Button discardBtn;
    [SerializeField] private Button saveBtn;

    [Header("Topbar")]
    [SerializeField] private TMP_Text empName;
    [SerializeField] private TMP_Text empInitials;

    [Header("Slider Track Width")]
    [SerializeField] private float trackWidth = 160f;

    // ── State ─────────────────────────────────────────────────────────────────
    private float  _masterVol    = 0.75f;
    private float  _sfxVol       = 0.60f;
    private float  _narrVol      = 0.90f;
    private bool   _muteAll      = false;
    private bool   _spatialAudio = true;
    private string _uiLang       = "FR";
    private string _narrLang     = "FR";
    private bool   _subtitles    = false;
    private string _hand         = "Right";
    private bool   _glowOn       = true;
    private bool   _hapticOn     = true;
    private string _selectMode   = "Trigger";
    private float  _dwellTime    = 0.5f;
    private bool   _soundOn      = true;

    private const float VOL_STEP   = 0.05f;
    private const float DWELL_STEP = 0.1f;
    private const float DWELL_MIN  = 0.1f;
    private const float DWELL_MAX  = 2.0f;

    void OnEnable()
    {
        LoadFromPrefs();
        WireButtons();
        ShowPanel("Audio");
        RefreshAll();
    }

    void OnDisable()
    {
        RemoveAllListeners();
    }

    // ─────────────────────────────────────────
    // WIRE BUTTONS
    // ─────────────────────────────────────────
    void WireButtons()
    {
        // Sidebar
        sidebarAudio?.onClick.AddListener(() => ShowPanel("Audio"));
        sidebarLanguage?.onClick.AddListener(() => ShowPanel("Language"));
        sidebarController?.onClick.AddListener(() => ShowPanel("Controller"));
        sidebarSignOut?.onClick.AddListener(OnSignOutClicked);

        // Back / Bottom nav
        backBtn?.onClick.AddListener(OnBackClicked);
        saveBtn?.onClick.AddListener(OnSaveClicked);
        discardBtn?.onClick.AddListener(OnDiscardClicked);
        signOutBtn?.onClick.AddListener(OnSignOutClicked);

        // Master Volume
        masterMinus?.onClick.AddListener(() => { _masterVol = Mathf.Clamp(_masterVol - VOL_STEP, 0f, 1f); UpdateSlider(masterFill, masterVal, _masterVol); });
        masterPlus?.onClick.AddListener(()  => { _masterVol = Mathf.Clamp(_masterVol + VOL_STEP, 0f, 1f); UpdateSlider(masterFill, masterVal, _masterVol); });

        // SFX Volume
        sfxMinus?.onClick.AddListener(() => { _sfxVol = Mathf.Clamp(_sfxVol - VOL_STEP, 0f, 1f); UpdateSlider(sfxFill, sfxVal, _sfxVol); });
        sfxPlus?.onClick.AddListener(()  => { _sfxVol = Mathf.Clamp(_sfxVol + VOL_STEP, 0f, 1f); UpdateSlider(sfxFill, sfxVal, _sfxVol); });

        // Narration Volume
        narrMinus?.onClick.AddListener(() => { _narrVol = Mathf.Clamp(_narrVol - VOL_STEP, 0f, 1f); UpdateSlider(narrFill, narrVal, _narrVol); });
        narrPlus?.onClick.AddListener(()  => { _narrVol = Mathf.Clamp(_narrVol + VOL_STEP, 0f, 1f); UpdateSlider(narrFill, narrVal, _narrVol); });

        // Mute Toggle
        muteToggle?.onClick.AddListener(() => { _muteAll = !_muteAll; UpdateToggle(muteBg, muteKnob, _muteAll); });

        // Spatial Audio Toggle
        spatialToggle?.onClick.AddListener(() => { _spatialAudio = !_spatialAudio; UpdateToggle(spatialBg, spatialKnob, _spatialAudio); });

        // Language UI
        langFR?.onClick.AddListener(() => { _uiLang = "FR"; RefreshLanguage(); });
        langEN?.onClick.AddListener(() => { _uiLang = "EN"; RefreshLanguage(); });
        langAR?.onClick.AddListener(() => { _uiLang = "AR"; RefreshLanguage(); });

        // Language Narration
        narrFR?.onClick.AddListener(() => { _narrLang = "FR"; RefreshLanguage(); });
        narrEN?.onClick.AddListener(() => { _narrLang = "EN"; RefreshLanguage(); });
        narrAR?.onClick.AddListener(() => { _narrLang = "AR"; RefreshLanguage(); });

        // Subtitles Toggle
        subtitleToggle?.onClick.AddListener(() => { _subtitles = !_subtitles; UpdateToggle(subtitleBg, subtitleKnob, _subtitles); });

        // Hand
        handRight?.onClick.AddListener(() => { _hand = "Right"; RefreshController(); });
        handLeft?.onClick.AddListener(()  => { _hand = "Left";  RefreshController(); });

        // Glow Toggle
        glowToggle?.onClick.AddListener(() => { _glowOn = !_glowOn; UpdateToggle(glowBg, glowKnob, _glowOn); });

        // Haptic Toggle
        hapticToggle?.onClick.AddListener(() => { _hapticOn = !_hapticOn; UpdateToggle(hapticBg, hapticKnob, _hapticOn); });

        // Selection Mode
        selectTrigger?.onClick.AddListener(() => { _selectMode = "Trigger"; RefreshController(); });
        selectDwell?.onClick.AddListener(()   => { _selectMode = "Dwell";   RefreshController(); });

        // Dwell Time
        dwellMinus?.onClick.AddListener(() => { _dwellTime = Mathf.Clamp(_dwellTime - DWELL_STEP, DWELL_MIN, DWELL_MAX); UpdateSlider(dwellFill, dwellVal, _dwellTime / DWELL_MAX, true); });
        dwellPlus?.onClick.AddListener(()  => { _dwellTime = Mathf.Clamp(_dwellTime + DWELL_STEP, DWELL_MIN, DWELL_MAX); UpdateSlider(dwellFill, dwellVal, _dwellTime / DWELL_MAX, true); });

        // Sound Toggle
        soundToggle?.onClick.AddListener(() => { _soundOn = !_soundOn; UpdateToggle(soundBg, soundKnob, _soundOn); });
    }

    void RemoveAllListeners()
    {
        sidebarAudio?.onClick.RemoveAllListeners();
        sidebarLanguage?.onClick.RemoveAllListeners();
        sidebarController?.onClick.RemoveAllListeners();
        sidebarSignOut?.onClick.RemoveAllListeners();
        backBtn?.onClick.RemoveAllListeners();
        saveBtn?.onClick.RemoveAllListeners();
        discardBtn?.onClick.RemoveAllListeners();
        signOutBtn?.onClick.RemoveAllListeners();
        masterMinus?.onClick.RemoveAllListeners();
        masterPlus?.onClick.RemoveAllListeners();
        sfxMinus?.onClick.RemoveAllListeners();
        sfxPlus?.onClick.RemoveAllListeners();
        narrMinus?.onClick.RemoveAllListeners();
        narrPlus?.onClick.RemoveAllListeners();
        muteToggle?.onClick.RemoveAllListeners();
        spatialToggle?.onClick.RemoveAllListeners();
        langFR?.onClick.RemoveAllListeners();
        langEN?.onClick.RemoveAllListeners();
        langAR?.onClick.RemoveAllListeners();
        narrFR?.onClick.RemoveAllListeners();
        narrEN?.onClick.RemoveAllListeners();
        narrAR?.onClick.RemoveAllListeners();
        subtitleToggle?.onClick.RemoveAllListeners();
        handRight?.onClick.RemoveAllListeners();
        handLeft?.onClick.RemoveAllListeners();
        glowToggle?.onClick.RemoveAllListeners();
        hapticToggle?.onClick.RemoveAllListeners();
        selectTrigger?.onClick.RemoveAllListeners();
        selectDwell?.onClick.RemoveAllListeners();
        dwellMinus?.onClick.RemoveAllListeners();
        dwellPlus?.onClick.RemoveAllListeners();
        soundToggle?.onClick.RemoveAllListeners();
    }

    // ─────────────────────────────────────────
    // SHOW PANEL
    // ─────────────────────────────────────────
    void ShowPanel(string panel)
    {
        panelAudio?.SetActive(panel == "Audio");
        panelLanguage?.SetActive(panel == "Language");
        panelController?.SetActive(panel == "Controller");

        SetSidebarActive(sidebarAudioBg,      panel == "Audio");
        SetSidebarActive(sidebarLanguageBg,   panel == "Language");
        SetSidebarActive(sidebarControllerBg, panel == "Controller");
    }

    void SetSidebarActive(Image bg, bool active)
    {
        if (bg == null) return;
        bg.color = active
            ? new Color(0f, 0.282f, 1f, 0.15f)
            : Color.clear;
    }

    // ─────────────────────────────────────────
    // REFRESH UI
    // ─────────────────────────────────────────
    void RefreshAll()
    {
        if (empName != null)     empName.text     = AppSession.EmployeeName     ?? "";
        if (empInitials != null) empInitials.text = AppSession.EmployeeInitials ?? "";

        UpdateSlider(masterFill, masterVal, _masterVol);
        UpdateSlider(sfxFill,    sfxVal,    _sfxVol);
        UpdateSlider(narrFill,   narrVal,   _narrVol);
        UpdateToggle(muteBg,     muteKnob,     _muteAll);
        UpdateToggle(spatialBg,  spatialKnob,  _spatialAudio);

        RefreshLanguage();
        RefreshController();
    }

    void RefreshLanguage()
    {
        SetOptionBtn(langFR, _uiLang == "FR");
        SetOptionBtn(langEN, _uiLang == "EN");
        SetOptionBtn(langAR, _uiLang == "AR");
        SetOptionBtn(narrFR, _narrLang == "FR");
        SetOptionBtn(narrEN, _narrLang == "EN");
        SetOptionBtn(narrAR, _narrLang == "AR");
        UpdateToggle(subtitleBg, subtitleKnob, _subtitles);
    }

    void RefreshController()
    {
        SetOptionBtn(handRight,     _hand == "Right");
        SetOptionBtn(handLeft,      _hand == "Left");
        SetOptionBtn(selectTrigger, _selectMode == "Trigger");
        SetOptionBtn(selectDwell,   _selectMode == "Dwell");
        UpdateToggle(glowBg,    glowKnob,    _glowOn);
        UpdateToggle(hapticBg,  hapticKnob,  _hapticOn);
        UpdateToggle(soundBg,   soundKnob,   _soundOn);
        UpdateSlider(dwellFill, dwellVal, _dwellTime / DWELL_MAX, true);
    }

    // ─────────────────────────────────────────
    // UI HELPERS
    // ─────────────────────────────────────────
    void UpdateSlider(RectTransform fill, TMP_Text label, float value, bool isDwell = false)
    {
        if (fill != null)
            fill.sizeDelta = new Vector2(trackWidth * Mathf.Clamp01(value), fill.sizeDelta.y);

        if (label != null)
        {
            if (isDwell)
                label.text = _dwellTime.ToString("F1") + "s";
            else
                label.text = Mathf.RoundToInt(value * 100f) + "%";
        }
    }

    void UpdateToggle(Image bg, RectTransform knob, bool on)
    {
        if (bg != null)
            bg.color = on
                ? new Color(0f, 0.282f, 1f, 0.9f)
                : new Color(1f, 1f, 1f, 0.12f);

        if (knob != null)
            knob.anchoredPosition = new Vector2(on ? 13f : -13f, 0f);
    }

    void SetOptionBtn(Button btn, bool selected)
    {
        if (btn == null) return;
        var img = btn.GetComponent<Image>();
        if (img != null)
            img.color = selected
                ? new Color(0f, 0.30f, 1f, 0.5f)  // sélectionné — bleu
                : new Color(1f, 1f, 1f, 0f);     // normal — blanc transparent

        var label = btn.transform.Find("L")?.GetComponent<TMP_Text>();
        if (label != null)
            label.color = selected
                ? Color.white
                : new Color(1f, 1f, 1f, 0.40f);
    }

    // ─────────────────────────────────────────
    // BOTTOM NAV ACTIONS
    // ─────────────────────────────────────────
    void OnBackClicked()
    {
        if (modulesPanel != null)
        {
            gameObject.SetActive(false);
            modulesPanel.SetActive(true);
        }
    }

    void OnSaveClicked()
    {
        PlayerPrefs.SetFloat("vol_master",   _masterVol);
        PlayerPrefs.SetFloat("vol_sfx",      _sfxVol);
        PlayerPrefs.SetFloat("vol_narr",     _narrVol);
        PlayerPrefs.SetInt("mute_all",       _muteAll ? 1 : 0);
        PlayerPrefs.SetInt("spatial",        _spatialAudio ? 1 : 0);
        PlayerPrefs.SetString("ui_lang",     _uiLang);
        PlayerPrefs.SetString("narr_lang",   _narrLang);
        PlayerPrefs.SetInt("subtitles",      _subtitles ? 1 : 0);
        PlayerPrefs.SetString("hand",        _hand);
        PlayerPrefs.SetString("select_mode", _selectMode);
        PlayerPrefs.SetFloat("dwell_time",   _dwellTime);
        PlayerPrefs.SetInt("glow",           _glowOn ? 1 : 0);
        PlayerPrefs.SetInt("haptic",         _hapticOn ? 1 : 0);
        PlayerPrefs.SetInt("sound",          _soundOn ? 1 : 0);
        PlayerPrefs.Save();
        OnBackClicked();
    }

    void OnDiscardClicked()
    {
        LoadFromPrefs();
        RefreshAll();
    }

    void LoadFromPrefs()
    {
        _masterVol    = PlayerPrefs.GetFloat("vol_master",   0.75f);
        _sfxVol       = PlayerPrefs.GetFloat("vol_sfx",      0.60f);
        _narrVol      = PlayerPrefs.GetFloat("vol_narr",     0.90f);
        _muteAll      = PlayerPrefs.GetInt("mute_all",       0) == 1;
        _spatialAudio = PlayerPrefs.GetInt("spatial",        1) == 1;
        _uiLang       = PlayerPrefs.GetString("ui_lang",     "FR");
        _narrLang     = PlayerPrefs.GetString("narr_lang",   "FR");
        _subtitles    = PlayerPrefs.GetInt("subtitles",      0) == 1;
        _hand         = PlayerPrefs.GetString("hand",        "Right");
        _selectMode   = PlayerPrefs.GetString("select_mode", "Trigger");
        _dwellTime    = PlayerPrefs.GetFloat("dwell_time",   0.5f);
        _glowOn       = PlayerPrefs.GetInt("glow",           1) == 1;
        _hapticOn     = PlayerPrefs.GetInt("haptic",         1) == 1;
        _soundOn      = PlayerPrefs.GetInt("sound",          1) == 1;
    }

    void OnSignOutClicked()
    {
        AppSession.Clear();
        PlayerPrefs.DeleteKey("device_token");
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
