using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Settings Panel Logic
/// Sliders use +/- buttons. Toggles use trigger press.
/// </summary>
public class SettingsPanel : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject modulesPanel;

    private SettingsLayout _layout;

    // State
    private float _masterVol    = 0.75f;
    private float _sfxVol       = 0.60f;
    private float _narrVol      = 0.90f;
    private bool  _muteAll      = false;
    private bool  _spatialAudio = true;
    private string _uiLang      = "FR";
    private string _narrLang    = "FR";
    private bool  _subtitles    = false;
    private string _hand        = "Right";
    private bool  _glowOn       = true;
    private bool  _hapticOn     = true;
    private string _selectMode  = "Trigger";
    private float _dwellTime    = 0.5f;
    private bool  _soundOn      = true;

    private const float VOL_STEP   = 0.05f;
    private const float DWELL_STEP = 0.1f;
    private const float DWELL_MIN  = 0.1f;
    private const float DWELL_MAX  = 2.0f;

    void Awake() => _layout = GetComponent<SettingsLayout>();

    void OnEnable()
    {
        _layout = GetComponent<SettingsLayout>();
        if (_layout == null) { Debug.LogError("[SettingsPanel] SettingsLayout not found."); return; }

        // Force refresh refs before anything else
        if (_layout.PanelAudio == null)
            _layout.RefreshRefs();

        LoadFromPrefs();
        WireButtons();
        ShowPanel("Audio");
        RefreshAll();
    }

    void OnDisable()
    {
        if (_layout == null) return;
        _layout.SidebarAudio?.onClick.RemoveAllListeners();
        _layout.SidebarLanguage?.onClick.RemoveAllListeners();
        _layout.SidebarController?.onClick.RemoveAllListeners();
        _layout.SidebarSignOut?.onClick.RemoveAllListeners();
        _layout.BackBtn?.onClick.RemoveAllListeners();
        _layout.SaveBtn?.onClick.RemoveAllListeners();
        _layout.DiscardBtn?.onClick.RemoveAllListeners();
        _layout.SignOutBtn?.onClick.RemoveAllListeners();
    }

    // ─────────────────────────────────────────
    // WIRE BUTTONS
    // ─────────────────────────────────────────
    void WireButtons()
    {
        // Sidebar navigation
        _layout.SidebarAudio?.onClick.AddListener(() => ShowPanel("Audio"));
        _layout.SidebarLanguage?.onClick.AddListener(() => ShowPanel("Language"));
        _layout.SidebarController?.onClick.AddListener(() => ShowPanel("Controller"));

        // Back / Bottom nav
        _layout.BackBtn?.onClick.AddListener(OnBackClicked);
        _layout.SaveBtn?.onClick.AddListener(OnSaveClicked);
        _layout.DiscardBtn?.onClick.AddListener(OnDiscardClicked);
        _layout.SignOutBtn?.onClick.AddListener(OnSignOutClicked);
        _layout.SidebarSignOut?.onClick.AddListener(OnSignOutClicked);

        // ── Audio — +/- buttons ───────────────────────────────────────────────
        WireSliderButtons(_layout.MasterMinus, _layout.MasterPlus,
            () => _masterVol, v => { _masterVol = v; SetSlider(_layout.MasterFill, _layout.MasterVal, _masterVol); });

        WireSliderButtons(_layout.SfxMinus, _layout.SfxPlus,
            () => _sfxVol, v => { _sfxVol = v; SetSlider(_layout.SfxFill, _layout.SfxVal, _sfxVol); });

        WireSliderButtons(_layout.NarrMinus, _layout.NarrPlus,
            () => _narrVol, v => { _narrVol = v; SetSlider(_layout.NarrFill, _layout.NarrVal, _narrVol); });

        // ── Audio — Toggles ───────────────────────────────────────────────────
        WireToggle(_layout.MuteBg,    _layout.MuteKnob,    () => _muteAll,     v => _muteAll = v);
        WireToggle(_layout.SpatialBg, _layout.SpatialKnob, () => _spatialAudio, v => _spatialAudio = v);

        // ── Language ──────────────────────────────────────────────────────────
        _layout.LangFR?.onClick.AddListener(() => { _uiLang = "FR"; RefreshLanguage(); });
        _layout.LangEN?.onClick.AddListener(() => { _uiLang = "EN"; RefreshLanguage(); });
        _layout.LangAR?.onClick.AddListener(() => { _uiLang = "AR"; RefreshLanguage(); });
        _layout.NarrFR?.onClick.AddListener(() => { _narrLang = "FR"; RefreshLanguage(); });
        _layout.NarrEN?.onClick.AddListener(() => { _narrLang = "EN"; RefreshLanguage(); });
        _layout.NarrAR?.onClick.AddListener(() => { _narrLang = "AR"; RefreshLanguage(); });
        WireToggle(_layout.SubtitleBg, _layout.SubtitleKnob, () => _subtitles, v => _subtitles = v);

        // ── Controller ────────────────────────────────────────────────────────
        _layout.HandRight?.onClick.AddListener(() => { _hand = "Right"; RefreshController(); });
        _layout.HandLeft?.onClick.AddListener(() => { _hand = "Left";  RefreshController(); });
        _layout.SelectTrigger?.onClick.AddListener(() => { _selectMode = "Trigger"; RefreshController(); });
        _layout.SelectDwell?.onClick.AddListener(() => { _selectMode = "Dwell";   RefreshController(); });
        WireToggle(_layout.GlowBg,   _layout.GlowKnob,   () => _glowOn,   v => _glowOn = v);
        WireToggle(_layout.HapticBg, _layout.HapticKnob, () => _hapticOn, v => _hapticOn = v);
        WireToggle(_layout.SoundBg,  _layout.SoundKnob,  () => _soundOn,  v => _soundOn = v);

        WireSliderButtons(_layout.DwellMinus, _layout.DwellPlus,
            () => _dwellTime, v => { _dwellTime = v; SetSlider(_layout.DwellFill, _layout.DwellVal, _dwellTime / DWELL_MAX); },
            DWELL_STEP, DWELL_MIN, DWELL_MAX);
    }

    // ─────────────────────────────────────────
    // WIRE HELPERS
    // ─────────────────────────────────────────
    void WireSliderButtons(Button minus, Button plus,
        System.Func<float> getter, System.Action<float> setter,
        float step = -1f, float min = 0f, float max = 1f)
    {
        if (step < 0) step = VOL_STEP;
        minus?.onClick.AddListener(() => setter(Mathf.Clamp(getter() - step, min, max)));
        plus?.onClick.AddListener(()  => setter(Mathf.Clamp(getter() + step, min, max)));
    }

    void WireToggle(Image bg, Image knob, System.Func<bool> getter, System.Action<bool> setter)
    {
        var btn = bg?.GetComponent<Button>();
        if (btn == null && bg != null)
            btn = bg.gameObject.AddComponent<Button>();

        btn?.onClick.AddListener(() =>
        {
            setter(!getter());
            SetToggle(bg, knob, getter());
        });
    }



    // ─────────────────────────────────────────
    // SHOW PANEL
    // ─────────────────────────────────────────
    void ShowPanel(string panel)
    {
        _layout.PanelAudio?.SetActive(panel == "Audio");
        _layout.PanelLanguage?.SetActive(panel == "Language");
        _layout.PanelController?.SetActive(panel == "Controller");

        SetSidebarActive(_layout.SidebarAudioBg,      panel == "Audio");
        SetSidebarActive(_layout.SidebarLanguageBg,   panel == "Language");
        SetSidebarActive(_layout.SidebarControllerBg, panel == "Controller");
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
        if (_layout.EmpName != null)     _layout.EmpName.text     = AppSession.EmployeeName ?? "";
        if (_layout.EmpInitials != null) _layout.EmpInitials.text = AppSession.EmployeeInitials ?? "";

        SetSlider(_layout.MasterFill, _layout.MasterVal, _masterVol);
        SetSlider(_layout.SfxFill,    _layout.SfxVal,    _sfxVol);
        SetSlider(_layout.NarrFill,   _layout.NarrVal,   _narrVol);
        SetToggle(_layout.MuteBg,     _layout.MuteKnob,    _muteAll);
        SetToggle(_layout.SpatialBg,  _layout.SpatialKnob, _spatialAudio);

        RefreshLanguage();
        RefreshController();
    }

    void RefreshLanguage()
    {
        SetOptionBtn(_layout.LangFR, _uiLang == "FR");
        SetOptionBtn(_layout.LangEN, _uiLang == "EN");
        SetOptionBtn(_layout.LangAR, _uiLang == "AR");
        SetOptionBtn(_layout.NarrFR, _narrLang == "FR");
        SetOptionBtn(_layout.NarrEN, _narrLang == "EN");
        SetOptionBtn(_layout.NarrAR, _narrLang == "AR");
        SetToggle(_layout.SubtitleBg, _layout.SubtitleKnob, _subtitles);
    }

    void RefreshController()
    {
        SetOptionBtn(_layout.HandRight,     _hand == "Right");
        SetOptionBtn(_layout.HandLeft,      _hand == "Left");
        SetOptionBtn(_layout.SelectTrigger, _selectMode == "Trigger");
        SetOptionBtn(_layout.SelectDwell,   _selectMode == "Dwell");
        SetToggle(_layout.GlowBg,    _layout.GlowKnob,   _glowOn);
        SetToggle(_layout.HapticBg,  _layout.HapticKnob, _hapticOn);
        SetToggle(_layout.SoundBg,   _layout.SoundKnob,  _soundOn);
        SetSlider(_layout.DwellFill, _layout.DwellVal,   _dwellTime / DWELL_MAX);
    }

    // ─────────────────────────────────────────
    // UI HELPERS
    // ─────────────────────────────────────────
    void SetSlider(Image fill, TMPro.TMP_Text label, float value)
    {
        if (fill != null)
        {
            var rt = fill.GetComponent<RectTransform>();
            if (rt != null) rt.anchorMax = new Vector2(Mathf.Clamp01(value), 1f);
        }
        if (label != null)
            label.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    void SetToggle(Image bg, Image knob, bool on)
    {
        if (bg != null)
            bg.color = on
                ? new Color(0f, 0.282f, 1f, 0.9f)
                : new Color(1f, 1f, 1f, 0.12f);

        if (knob != null)
        {
            var rt = knob.GetComponent<RectTransform>();
            if (rt != null) rt.anchoredPosition = new Vector2(on ? 12f : -12f, 0f);
        }
    }

    void SetOptionBtn(Button btn, bool selected)
    {
        if (btn == null) return;
        var img = btn.GetComponent<Image>();
        if (img != null)
            img.color = selected
                ? new Color(0f, 0.282f, 1f, 0.9f)
                : new Color(1f, 1f, 1f, 0.07f);
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
        PlayerPrefs.SetFloat("vol_master",      _masterVol);
        PlayerPrefs.SetFloat("vol_sfx",         _sfxVol);
        PlayerPrefs.SetFloat("vol_narr",        _narrVol);
        PlayerPrefs.SetInt("mute_all",          _muteAll ? 1 : 0);
        PlayerPrefs.SetInt("spatial",           _spatialAudio ? 1 : 0);
        PlayerPrefs.SetString("ui_lang",        _uiLang);
        PlayerPrefs.SetString("narr_lang",      _narrLang);
        PlayerPrefs.SetInt("subtitles",         _subtitles ? 1 : 0);
        PlayerPrefs.SetString("hand",           _hand);
        PlayerPrefs.SetString("select_mode",    _selectMode);
        PlayerPrefs.SetFloat("dwell_time",      _dwellTime);
        PlayerPrefs.SetInt("glow",              _glowOn ? 1 : 0);
        PlayerPrefs.SetInt("haptic",            _hapticOn ? 1 : 0);
        PlayerPrefs.SetInt("sound",             _soundOn ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("[SettingsPanel] Settings saved ✅");
        OnBackClicked();
    }

    void OnDiscardClicked()
    {
        LoadFromPrefs();
        RefreshAll();
        Debug.Log("[SettingsPanel] Settings discarded");
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
        Debug.Log("[SettingsPanel] Signed out");
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
