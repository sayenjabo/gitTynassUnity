using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Settings Layout
/// Pure UI script. No logic. No API. No navigation.
///
/// HIERARCHY:
/// Canvas
/// ├── Background
/// ├── Glow1 / Glow2
/// ├── Topbar                      (1280x66, y=327)
/// │   ├── LogoBadge / BrandName / BrandSub
/// │   ├── EmpPill                 (employee name)
/// │   └── BackBtn
/// ├── Separator
/// ├── Sidebar                     (220x510, x=-482, y=-30)
/// │   ├── Label_Settings
/// │   ├── SidebarBtn_Audio        (active by default)
/// │   ├── SidebarBtn_Language
/// │   ├── SidebarBtn_Controller
/// │   ├── SidebarDivider
/// │   ├── Label_Account
/// │   ├── SidebarBtn_Profile
/// │   └── SidebarBtn_SignOut
/// ├── Panel_Audio                 (900x510, x=90, y=-30)
/// │   ├── PanelTitle / PanelSub
/// │   ├── VolumeCard              (slider rows: Master, SFX, Narration)
/// │   └── OptionsCard             (toggle rows: Mute, Spatial Audio)
/// ├── Panel_Language              (hidden)
/// │   ├── PanelTitle / PanelSub
/// │   ├── UILangCard              (option group: FR/EN/AR)
/// │   └── NarrationCard           (option group + subtitle toggle)
/// ├── Panel_Controller            (hidden)
/// │   ├── PanelTitle / PanelSub
/// │   ├── LaserCard               (dominant hand, pointer glow, haptic)
/// │   └── InteractionCard         (selection mode, dwell time, sound)
/// └── BottomNav                   (1280x66, y=-304)
///     ├── SignOutBtn
///     ├── DiscardBtn
///     └── SaveBtn
/// </summary>
public class SettingsLayout : MonoBehaviour
{
    [Header("Canvas Root")]
    [SerializeField] RectTransform canvasRoot;
    [Header("Font")]
    [SerializeField] TMP_FontAsset font;

    [Header("Built refs")]
    public TMP_Text   EmpInitials;
    public TMP_Text   EmpName;
    public Button     BackBtn;

    // Sidebar buttons
    public Button SidebarAudio;
    public Button SidebarLanguage;
    public Button SidebarController;
    public Button SidebarProfile;
    public Button SidebarSignOut;
    public Image  SidebarAudioBg;
    public Image  SidebarLanguageBg;
    public Image  SidebarControllerBg;
    public Image  SidebarProfileBg;
    public Image  SidebarSignOutBg;

    // Panels (toggled by SettingsPanel.cs)
    public GameObject PanelAudio;
    public GameObject PanelLanguage;
    public GameObject PanelController;

    // Audio sliders
    public Image  MasterFill;    public TMP_Text MasterVal;
    public Image  SfxFill;       public TMP_Text SfxVal;
    public Image  NarrFill;      public TMP_Text NarrVal;
    // Audio toggles
    public Image  MuteKnob;      public Image MuteBg;
    public Image  SpatialKnob;   public Image SpatialBg;

    // Language option buttons
    public Button LangFR;  public Button LangEN;  public Button LangAR;
    public Button NarrFR;  public Button NarrEN;  public Button NarrAR;
    public Image  SubtitleKnob; public Image SubtitleBg;

    // Controller options
    public Button HandRight;    public Button HandLeft;
    public Image  GlowKnob;     public Image GlowBg;
    public Image  HapticKnob;   public Image HapticBg;
    public Button SelectTrigger; public Button SelectDwell;
    public Image  DwellFill;    public TMP_Text DwellVal;
    public Image  SoundKnob;    public Image SoundBg;

    // Bottom nav
    public Button SignOutBtn;
    public Button DiscardBtn;
    public Button SaveBtn;

    // ── Colours ───────────────────────────────────────────────────────────────
    static readonly Color BG           = C("0A1628");
    static readonly Color BLUE         = C("0048FF");
    static readonly Color BLUE_DIM     = new Color(0f,0.282f,1f,0.15f);
    static readonly Color BLUE_BORDER  = new Color(0f,0.282f,1f,0.35f);
    static readonly Color CARD         = new Color(1f,1f,1f,0.06f);
    static readonly Color CARD_BORDER  = new Color(1f,1f,1f,0.10f);
    static readonly Color INPUT_BG     = new Color(1f,1f,1f,0.07f);
    static readonly Color INPUT_BORDER = new Color(1f,1f,1f,0.11f);
    static readonly Color ROW_LINE     = new Color(1f,1f,1f,0.06f);
    static readonly Color ICON_BG      = new Color(1f,1f,1f,0.06f);
    static readonly Color ICON_BORDER  = new Color(1f,1f,1f,0.10f);
    static readonly Color SLIDER_BG    = new Color(1f,1f,1f,0.10f);
    static readonly Color TOGGLE_OFF   = new Color(1f,1f,1f,0.12f);
    static readonly Color WHITE        = Color.white;
    static readonly Color WHITE_75     = new Color(1f,1f,1f,0.75f);
    static readonly Color WHITE_65     = new Color(1f,1f,1f,0.65f);
    static readonly Color WHITE_55     = new Color(1f,1f,1f,0.55f);
    static readonly Color WHITE_50     = new Color(1f,1f,1f,0.50f);
    static readonly Color WHITE_38     = new Color(1f,1f,1f,0.38f);
    static readonly Color WHITE_35     = new Color(1f,1f,1f,0.35f);
    static readonly Color WHITE_30     = new Color(1f,1f,1f,0.30f);
    static readonly Color WHITE_20     = new Color(1f,1f,1f,0.20f);

    void Awake() => Build();

    void Build()
    {
        if (canvasRoot == null) { Debug.LogError("[SettingsLayout] Canvas Root not assigned."); return; }
        for (int i = canvasRoot.childCount - 1; i >= 0; i--)
            Destroy(canvasRoot.GetChild(i).gameObject);

        var bg = Make("Background", canvasRoot); Stretch(bg); Img(bg, BG);
        var g1 = Make("Glow1", canvasRoot); Place(g1, 300f, 180f, 700f, 700f);
        Img(g1, new Color(0f,0.282f,1f,0.13f));
        var g2 = Make("Glow2", canvasRoot); Place(g2, -380f, -220f, 450f, 450f);
        Img(g2, new Color(0f,0.282f,1f,0.09f));

        // ── Topbar ────────────────────────────────────────────────────────────
        var tb = Make("Topbar", canvasRoot); Place(tb, 0f, 327f, 1280f, 66f);
        Img(tb, Color.clear);

        var lbadge = Make("LogoBadge", tb); Place(lbadge, -584f, 0f, 46f, 46f); Img(lbadge, BLUE);
        var lt = MakeTMP("T", lbadge, "T", 20f, FontStyles.Bold, WHITE);
        Place(lt, 0f, 0f, 46f, 46f); Align(lt, TextAlignmentOptions.Center);

        var bname = MakeTMP("BrandName", tb, "TynassIt", 17f, FontStyles.Bold, WHITE);
        Place(bname, -516f, 8f, 120f, 24f); Align(bname, TextAlignmentOptions.Left);
        var bsub = MakeTMP("BrandSub", tb, "TRAINING PLATFORM", 9f, FontStyles.Normal, WHITE_35);
        Place(bsub, -510f, -8f, 160f, 14f); Align(bsub, TextAlignmentOptions.Left);
        bsub.characterSpacing = 2f;

        // Employee pill  200x44 at (380, 0)
        var empPill = Make("EmpPill", tb); Place(empPill, 380f, 0f, 200f, 44f);
        Img(empPill, INPUT_BG); Border(empPill, INPUT_BORDER);
        var empAvatar = Make("EmpAvatar", empPill); Place(empAvatar, -74f, 0f, 32f, 32f);
        Img(empAvatar, BLUE);
        EmpInitials = MakeTMP("Init", empAvatar, "AM", 11f, FontStyles.Bold, WHITE);
        Place(EmpInitials, 0f, 0f, 32f, 32f); Align(EmpInitials, TextAlignmentOptions.Center);
        EmpName = MakeTMP("Name", empPill, "Ahmed Maalej", 13f, FontStyles.Bold, WHITE_75);
        Place(EmpName, 18f, 0f, 130f, 44f); Align(EmpName, TextAlignmentOptions.Left);

        // Back button  160x48 at (556, 0)
        var backRect = Make("BackBtn", tb); Place(backRect, 556f, 0f, 160f, 48f);
        Img(backRect, INPUT_BG); Border(backRect, INPUT_BORDER);
        BackBtn = backRect.gameObject.AddComponent<Button>();
        var backLbl = MakeTMP("L", backRect, "← Back", 15f, FontStyles.Bold, WHITE_65);
        Place(backLbl, 0f, 0f, 160f, 48f); Align(backLbl, TextAlignmentOptions.Center);

        var sep = Make("Sep", canvasRoot); Place(sep, 0f, 294f, 1280f, 1f);
        Img(sep, new Color(1f,1f,1f,0.07f));

        // =====================================================================
        //  SIDEBAR  220x510 at x=-478, y=-22
        // =====================================================================
        var sidebar = Make("Sidebar", canvasRoot); Place(sidebar, -478f, -22f, 220f, 510f);
        Img(sidebar, Color.clear);

        var lbl1 = MakeTMP("Lbl_Settings", sidebar, "SETTINGS", 10f, FontStyles.Bold, WHITE_30);
        Place(lbl1, -10f, 236f, 200f, 16f); Align(lbl1, TextAlignmentOptions.Left);
        lbl1.characterSpacing = 1.8f;

        // Sidebar buttons — y positions from top
        (SidebarAudio,      SidebarAudioBg)      = SidebarBtn(sidebar, "🔊  Audio",      200f, true);
        (SidebarLanguage,   SidebarLanguageBg)   = SidebarBtn(sidebar, "🌐  Language",   148f, false);
        (SidebarController, SidebarControllerBg) = SidebarBtn(sidebar, "🎮  Controller", 96f,  false);

        // Divider
        var div = Make("Divider", sidebar); Place(div, 0f, 64f, 200f, 1f);
        Img(div, new Color(1f,1f,1f,0.07f));

        var lbl2 = MakeTMP("Lbl_Account", sidebar, "ACCOUNT", 10f, FontStyles.Bold, WHITE_30);
        Place(lbl2, -10f, 46f, 200f, 16f); Align(lbl2, TextAlignmentOptions.Left);
        lbl2.characterSpacing = 1.8f;

        (SidebarProfile,  SidebarProfileBg)  = SidebarBtn(sidebar, "👤  Profile",  14f,  false);
        (SidebarSignOut,  SidebarSignOutBg)  = SidebarBtn(sidebar, "🚪  Sign Out", -38f, false);

        // =====================================================================
        //  PANEL AUDIO  900x510 at x=98, y=-22
        // =====================================================================
        var panelAudio = Make("Panel_Audio", canvasRoot); Place(panelAudio, 98f, -22f, 900f, 510f);
        Img(panelAudio, Color.clear);
        PanelAudio = panelAudio.gameObject;

        var audTitle = MakeTMP("Title", panelAudio, "Audio Settings", 20f, FontStyles.Bold, WHITE);
        Place(audTitle, -320f, 236f, 300f, 30f); Align(audTitle, TextAlignmentOptions.Left);
        var audSub = MakeTMP("Sub", panelAudio, "Control all sound output for your VR session", 13f, FontStyles.Normal, WHITE_38);
        Place(audSub, -160f, 208f, 580f, 20f); Align(audSub, TextAlignmentOptions.Left);

        // Volume Card  900x178 at (0, 100)
        var volCard = Make("VolumeCard", panelAudio); Place(volCard, 0f, 100f, 900f, 178f);
        Img(volCard, CARD); Border(volCard, CARD_BORDER);
        SectionTitle("Volume", volCard, 0f, 68f);

        (MasterFill, MasterVal) = SliderRow(volCard, "🔊", "Master Volume",   "Overall audio output level",         40f, 0.75f, "75");
        (SfxFill,    SfxVal)    = SliderRow(volCard, "🎵", "Sound Effects",   "In-simulation sound effects",        -4f,  0.60f, "60");
        (NarrFill,   NarrVal)   = SliderRow(volCard, "🎙", "Voice Narration", "Scenario instructions read aloud",  -48f,  0.90f, "90");

        // Options Card  900x114 at (0, -42)
        var optCard = Make("OptionsCard", panelAudio); Place(optCard, 0f, -70f, 900f, 114f);
        Img(optCard, CARD); Border(optCard, CARD_BORDER);
        SectionTitle("AudioOptions", optCard, 0f, 42f);

        (MuteBg,    MuteKnob)    = ToggleRow(optCard, "🔇", "Mute All Audio",  "Silence everything instantly",    10f,  false);
        (SpatialBg, SpatialKnob) = ToggleRow(optCard, "🌍", "Spatial Audio",   "3D positional sound in VR",      -32f,  true);

        // =====================================================================
        //  PANEL LANGUAGE  900x510 at x=98, y=-22  (hidden)
        // =====================================================================
        var panelLang = Make("Panel_Language", canvasRoot); Place(panelLang, 98f, -22f, 900f, 510f);
        Img(panelLang, Color.clear);
        PanelLanguage = panelLang.gameObject;
        PanelLanguage.SetActive(false);

        var langTitle = MakeTMP("Title", panelLang, "Language", 20f, FontStyles.Bold, WHITE);
        Place(langTitle, -380f, 236f, 200f, 30f); Align(langTitle, TextAlignmentOptions.Left);
        var langSub = MakeTMP("Sub", panelLang, "Choose your preferred interface and narration language", 13f, FontStyles.Normal, WHITE_38);
        Place(langSub, -130f, 208f, 700f, 20f); Align(langSub, TextAlignmentOptions.Left);

        // UI Language Card  900x90 at (0, 124)
        var uiLangCard = Make("UILangCard", panelLang); Place(uiLangCard, 0f, 124f, 900f, 90f);
        Img(uiLangCard, CARD); Border(uiLangCard, CARD_BORDER);
        SectionTitle("UILang", uiLangCard, 0f, 34f);
        IconInfo(uiLangCard, "🖥", "UI Language", "Menus, buttons, and labels", -100f, -2f);
        (LangFR, LangEN, LangAR) = OptionGroup3(uiLangCard, "Français","English","عربية", 260f, -2f, true);

        // Narration Card  900x130 at (0, 0)
        var narrCard = Make("NarrationCard", panelLang); Place(narrCard, 0f, -6f, 900f, 130f);
        Img(narrCard, CARD); Border(narrCard, CARD_BORDER);
        SectionTitle("Narration", narrCard, 0f, 50f);
        IconInfo(narrCard, "🎙", "Voice Language", "Spoken instructions in simulation", -100f, 16f);
        (NarrFR, NarrEN, NarrAR) = OptionGroup3(narrCard, "Français","English","عربية", 260f, 16f, true);
        (SubtitleBg, SubtitleKnob) = ToggleRow(narrCard, "📄", "Show Subtitles", "Display text alongside narration", -30f, true);

        // =====================================================================
        //  PANEL CONTROLLER  900x510 at x=98, y=-22  (hidden)
        // =====================================================================
        var panelCtrl = Make("Panel_Controller", canvasRoot); Place(panelCtrl, 98f, -22f, 900f, 510f);
        Img(panelCtrl, Color.clear);
        PanelController = panelCtrl.gameObject;
        PanelController.SetActive(false);

        var ctrlTitle = MakeTMP("Title", panelCtrl, "Controller & Interaction", 20f, FontStyles.Bold, WHITE);
        Place(ctrlTitle, -230f, 236f, 440f, 30f); Align(ctrlTitle, TextAlignmentOptions.Left);
        var ctrlSub = MakeTMP("Sub", panelCtrl, "Customize how you interact with the VR environment", 13f, FontStyles.Normal, WHITE_38);
        Place(ctrlSub, -100f, 208f, 700f, 20f); Align(ctrlSub, TextAlignmentOptions.Left);

        // Laser Card  436x240 at (-232, 60)
        var laserCard = Make("LaserCard", panelCtrl); Place(laserCard, -232f, 60f, 436f, 240f);
        Img(laserCard, CARD); Border(laserCard, CARD_BORDER);
        SectionTitle("Laser", laserCard, 0f, 106f);
        IconInfo(laserCard, "🎯", "Dominant Hand", "Ray cast origin", -60f, 60f);
        (HandRight, HandLeft) = OptionGroup2(laserCard, "Right Hand", "Left Hand", 0f, 14f, true);
        (GlowBg,   GlowKnob)   = ToggleRow(laserCard, "✨", "Pointer Glow",    "Visual ray effect",    -40f, true);
        (HapticBg, HapticKnob) = ToggleRow(laserCard, "📳", "Haptic Feedback", "Vibrate on selection", -84f, true);

        // Interaction Card  436x240 at (232, 60)
        var interCard = Make("InteractionCard", panelCtrl); Place(interCard, 232f, 60f, 436f, 240f);
        Img(interCard, CARD); Border(interCard, CARD_BORDER);
        SectionTitle("Interact", interCard, 0f, 106f);
        IconInfo(interCard, "👆", "Selection Mode", "How to confirm a choice", -60f, 60f);
        (SelectTrigger, SelectDwell) = OptionGroup2(interCard, "Trigger Press", "Dwell (Gaze)", 0f, 14f, true);
        (DwellFill, DwellVal) = SliderRow(interCard, "⏱", "Dwell Time", "Seconds to auto-select", -42f, 0.5f, "1.5s");
        (SoundBg,  SoundKnob) = ToggleRow(interCard, "🔔", "Selection Sound", "Click sound on confirm", -86f, true);

        // =====================================================================
        //  BOTTOM NAV  1280x66 at y=-304
        // =====================================================================
        var nav = Make("BottomNav", canvasRoot); Place(nav, 0f, -304f, 1280f, 66f);
        Img(nav, Color.clear);

        // Sign Out  200x56 at (-260, 0) — neutral grey
        var soRect = Make("SignOutBtn", nav); Place(soRect, -260f, 0f, 200f, 56f);
        Img(soRect, INPUT_BG); Border(soRect, INPUT_BORDER);
        SignOutBtn = soRect.gameObject.AddComponent<Button>();
        var soLbl = MakeTMP("L", soRect, "🚪  Sign Out", 15f, FontStyles.Bold, WHITE_55);
        Place(soLbl, 0f, 0f, 200f, 56f); Align(soLbl, TextAlignmentOptions.Center);

        // Discard  200x56 at (0, 0)
        var discRect = Make("DiscardBtn", nav); Place(discRect, 0f, 0f, 200f, 56f);
        Img(discRect, INPUT_BG); Border(discRect, INPUT_BORDER);
        DiscardBtn = discRect.gameObject.AddComponent<Button>();
        var discLbl = MakeTMP("L", discRect, "Discard", 15f, FontStyles.Bold, WHITE_55);
        Place(discLbl, 0f, 0f, 200f, 56f); Align(discLbl, TextAlignmentOptions.Center);

        // Save Settings  220x56 at (224, 0) — #0048FF
        var saveRect = Make("SaveBtn", nav); Place(saveRect, 224f, 0f, 220f, 56f);
        Img(saveRect, BLUE);
        SaveBtn = saveRect.gameObject.AddComponent<Button>();
        var saveLbl = MakeTMP("L", saveRect, "✓  Save Settings", 16f, FontStyles.Bold, WHITE);
        Place(saveLbl, 0f, 0f, 220f, 56f); Align(saveLbl, TextAlignmentOptions.Center);
    }

    // =========================================================================
    //  COMPONENT BUILDERS
    // =========================================================================

    (Button btn, Image bg) SidebarBtn(RectTransform parent, string label, float y, bool active)
    {
        var r = Make($"Btn_{label.Replace(" ","")}", parent); Place(r, 0f, y, 200f, 48f);
        var bg = Img(r, active ? BLUE_DIM : Color.clear);
        if (active) { var o = r.gameObject.AddComponent<Outline>(); o.effectColor = BLUE_BORDER; o.effectDistance = new Vector2(2f,2f); }
        var btn = r.gameObject.AddComponent<Button>();
        var lbl = MakeTMP("L", r, label, 15f, FontStyles.Bold, active ? WHITE : WHITE_50);
        Place(lbl, 4f, 0f, 192f, 48f); Align(lbl, TextAlignmentOptions.Left);
        return (btn, bg);
    }

    void SectionTitle(string name, RectTransform parent, float x, float y)
    {
        var t = MakeTMP(name+"Title", parent, name.ToUpper(), 10f, FontStyles.Bold, WHITE_38);
        Place(t, x - 300f, y, 180f, 16f); Align(t, TextAlignmentOptions.Left);
        t.characterSpacing = 1.8f;
        var line = Make(name+"Line", parent); Place(line, x + 60f, y, 500f, 1f);
        Img(line, new Color(1f,1f,1f,0.08f));
    }

    void IconInfo(RectTransform parent, string icon, string name, string desc, float x, float y)
    {
        var wrap = Make("IconWrap_"+name, parent); Place(wrap, x - 140f, y, 44f, 44f);
        Img(wrap, ICON_BG); Border(wrap, ICON_BORDER);
        var iLbl = MakeTMP("I", wrap, icon, 18f, FontStyles.Normal, WHITE);
        Place(iLbl, 0f, 0f, 44f, 44f); Align(iLbl, TextAlignmentOptions.Center);
        var nLbl = MakeTMP("N", parent, name, 15f, FontStyles.Bold, WHITE);
        Place(nLbl, x - 56f, y + 8f, 200f, 22f); Align(nLbl, TextAlignmentOptions.Left);
        var dLbl = MakeTMP("D", parent, desc, 12f, FontStyles.Normal, WHITE_35);
        Place(dLbl, x - 40f, y - 10f, 220f, 18f); Align(dLbl, TextAlignmentOptions.Left);
    }

    (Image fill, TMP_Text val) SliderRow(RectTransform parent, string icon, string name, string desc, float y, float pct, string valStr)
    {
        IconInfo(parent, icon, name, desc, -100f, y);

        // Track  220x6
        var track = Make("Track_"+name, parent); Place(track, 220f, y, 220f, 6f); Img(track, SLIDER_BG);
        // Fill
        var fillRect = Make("Fill", track);
        fillRect.anchorMin = new Vector2(0f, 0f); fillRect.anchorMax = new Vector2(0f, 1f);
        fillRect.pivot = new Vector2(0f, 0.5f); fillRect.anchoredPosition = Vector2.zero;
        fillRect.sizeDelta = new Vector2(220f * pct, 0f);
        var fill = fillRect.gameObject.AddComponent<Image>(); fill.color = BLUE;
        // Thumb  22x22
        var thumb = Make("Thumb", track);
        thumb.anchorMin = thumb.anchorMax = new Vector2(pct, 0.5f); thumb.pivot = new Vector2(0.5f, 0.5f);
        thumb.anchoredPosition = Vector2.zero; thumb.sizeDelta = new Vector2(22f, 22f);
        var tImg = thumb.gameObject.AddComponent<Image>(); tImg.color = WHITE;
        var tOut = thumb.gameObject.AddComponent<Outline>(); tOut.effectColor = BLUE; tOut.effectDistance = new Vector2(2f,2f);
        // Value label
        var vLbl = MakeTMP("Val_"+name, parent, valStr, 14f, FontStyles.Bold, WHITE);
        Place(vLbl, 356f, y, 50f, 22f); Align(vLbl, TextAlignmentOptions.Right);
        return (fill, vLbl);
    }

    (Image bg, Image knob) ToggleRow(RectTransform parent, string icon, string name, string desc, float y, bool isOn)
    {
        IconInfo(parent, icon, name, desc, -100f, y);
        // Toggle pill  56x30
        var pill = Make("Toggle_"+name, parent); Place(pill, 380f, y, 56f, 30f);
        var bg = Img(pill, isOn ? BLUE : TOGGLE_OFF);
        // Knob  24x24
        var knob = Make("Knob", pill);
        knob.anchorMin = knob.anchorMax = new Vector2(0.5f, 0.5f); knob.pivot = new Vector2(0.5f, 0.5f);
        knob.anchoredPosition = new Vector2(isOn ? 13f : -13f, 0f); knob.sizeDelta = new Vector2(24f, 24f);
        var kImg = knob.gameObject.AddComponent<Image>(); kImg.color = WHITE;
        pill.gameObject.AddComponent<Button>();
        return (bg, kImg);
    }

    (Button b1, Button b2) OptionGroup2(RectTransform parent, string l1, string l2, float x, float y, bool firstSelected)
    {
        var r1 = Make("Opt1", parent); Place(r1, x - 70f, y, 120f, 38f);
        Img(r1, firstSelected ? BLUE_DIM : Color.clear);
        Border(r1, firstSelected ? BLUE_BORDER : INPUT_BORDER);
        var b1 = r1.gameObject.AddComponent<Button>();
        var t1 = MakeTMP("L", r1, l1, 13f, FontStyles.Bold, firstSelected ? WHITE : WHITE_50);
        Place(t1, 0f, 0f, 120f, 38f); Align(t1, TextAlignmentOptions.Center);

        var r2 = Make("Opt2", parent); Place(r2, x + 70f, y, 120f, 38f);
        Img(r2, !firstSelected ? BLUE_DIM : Color.clear);
        Border(r2, !firstSelected ? BLUE_BORDER : INPUT_BORDER);
        var b2 = r2.gameObject.AddComponent<Button>();
        var t2 = MakeTMP("L", r2, l2, 13f, FontStyles.Bold, !firstSelected ? WHITE : WHITE_50);
        Place(t2, 0f, 0f, 120f, 38f); Align(t2, TextAlignmentOptions.Center);

        return (b1, b2);
    }

    (Button b1, Button b2, Button b3) OptionGroup3(RectTransform parent, string l1, string l2, string l3, float x, float y, bool firstSelected)
    {
        float[] ox = { x - 80f, x + 10f, x + 96f };
        string[] labels = { l1, l2, l3 };
        Button[] btns = new Button[3];
        for (int i = 0; i < 3; i++)
        {
            bool sel = firstSelected && i == 0;
            var r = Make($"Opt{i+1}", parent); Place(r, ox[i], y, 84f, 38f);
            Img(r, sel ? BLUE_DIM : Color.clear);
            Border(r, sel ? BLUE_BORDER : INPUT_BORDER);
            btns[i] = r.gameObject.AddComponent<Button>();
            var t = MakeTMP("L", r, labels[i], 13f, FontStyles.Bold, sel ? WHITE : WHITE_50);
            Place(t, 0f, 0f, 84f, 38f); Align(t, TextAlignmentOptions.Center);
        }
        return (btns[0], btns[1], btns[2]);
    }

    // =========================================================================
    //  PRIMITIVE HELPERS
    // =========================================================================
    RectTransform Make(string name, RectTransform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go.GetComponent<RectTransform>();
    }
    void Place(RectTransform rt, float x, float y, float w, float h)
    {
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(x, y); rt.sizeDelta = new Vector2(w, h);
    }
    void Place(TMP_Text t, float x, float y, float w, float h)
        => Place(t.GetComponent<RectTransform>(), x, y, w, h);
    void Stretch(RectTransform rt)
    { rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = rt.offsetMax = Vector2.zero; }
    Image Img(RectTransform rt, Color color)
    { var img = rt.gameObject.AddComponent<Image>(); img.color = color; return img; }
    void Border(RectTransform rt, Color color)
    { var o = rt.gameObject.AddComponent<Outline>(); o.effectColor = color; o.effectDistance = new Vector2(1.5f,1.5f); }
    TMP_Text MakeTMP(string name, RectTransform parent, string text, float size, FontStyles style, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform)); go.transform.SetParent(parent, false);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.text = text; t.fontSize = size; t.fontStyle = style; t.color = color;
        if (font != null) t.font = font; return t;
    }
    void Align(TMP_Text t, TextAlignmentOptions a) => t.alignment = a;
    static Color C(string hex) { ColorUtility.TryParseHtmlString("#" + hex, out Color c); return c; }
}
