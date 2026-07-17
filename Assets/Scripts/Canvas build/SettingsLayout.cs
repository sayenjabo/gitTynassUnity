using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Settings Layout
/// Pure UI script. No logic. No API. No navigation.
/// SliderRows now include − / + buttons.
/// </summary>
public class SettingsLayout : MonoBehaviour
{
 [Header("Canvas Root")]
 [SerializeField] RectTransform canvasRoot;
 [Header("Font")]
 [SerializeField] TMP_FontAsset font;

 [Header("Built refs")]
 public TMP_Text EmpInitials;
 public TMP_Text EmpName;
 public Button BackBtn;

 // Sidebar buttons
 public Button SidebarAudio;
 public Button SidebarLanguage;
 public Button SidebarController;
 public Button SidebarProfile;
 public Button SidebarSignOut;
 public Image SidebarAudioBg;
 public Image SidebarLanguageBg;
 public Image SidebarControllerBg;
 public Image SidebarProfileBg;
 public Image SidebarSignOutBg;

 // Panels
 public GameObject PanelAudio;
 public GameObject PanelLanguage;
 public GameObject PanelController;

 // Audio sliders + buttons
 public Image MasterFill; public TMP_Text MasterVal;
 public Button MasterMinus; public Button MasterPlus;
 public Image SfxFill; public TMP_Text SfxVal;
 public Button SfxMinus; public Button SfxPlus;
 public Image NarrFill; public TMP_Text NarrVal;
 public Button NarrMinus; public Button NarrPlus;

 // Audio toggles
 public Image MuteKnob; public Image MuteBg;
 public Image SpatialKnob; public Image SpatialBg;

 // Language
 public Button LangFR; public Button LangEN; public Button LangAR;
 public Button NarrFR; public Button NarrEN; public Button NarrAR;
 public Image SubtitleKnob; public Image SubtitleBg;

 // Controller
 public Button HandRight; public Button HandLeft;
 public Image GlowKnob; public Image GlowBg;
 public Image HapticKnob; public Image HapticBg;
 public Button SelectTrigger; public Button SelectDwell;
 public Image DwellFill; public TMP_Text DwellVal;
 public Button DwellMinus; public Button DwellPlus;
 public Image SoundKnob; public Image SoundBg;

 // Bottom nav
 public Button SignOutBtn;
 public Button DiscardBtn;
 public Button SaveBtn;

 // Colours 
 static readonly Color BG = C("0A1628");
 static readonly Color BLUE = C("0048FF");
 static readonly Color BLUE_DIM = new Color(0f,0.282f,1f,0.15f);
 static readonly Color BLUE_BORDER = new Color(0f,0.282f,1f,0.35f);
 static readonly Color CARD = new Color(1f,1f,1f,0.06f);
 static readonly Color CARD_BORDER = new Color(1f,1f,1f,0.10f);
 static readonly Color INPUT_BG = new Color(1f,1f,1f,0.07f);
 static readonly Color INPUT_BORDER = new Color(1f,1f,1f,0.11f);
 static readonly Color ROW_LINE = new Color(1f,1f,1f,0.06f);
 static readonly Color ICON_BG = new Color(1f,1f,1f,0.06f);
 static readonly Color ICON_BORDER = new Color(1f,1f,1f,0.10f);
 static readonly Color SLIDER_BG = new Color(1f,1f,1f,0.10f);
 static readonly Color TOGGLE_OFF = new Color(1f,1f,1f,0.12f);
 static readonly Color WHITE = Color.white;
 static readonly Color WHITE_75 = new Color(1f,1f,1f,0.75f);
 static readonly Color WHITE_65 = new Color(1f,1f,1f,0.65f);
 static readonly Color WHITE_55 = new Color(1f,1f,1f,0.55f);
 static readonly Color WHITE_50 = new Color(1f,1f,1f,0.50f);
 static readonly Color WHITE_38 = new Color(1f,1f,1f,0.38f);
 static readonly Color WHITE_35 = new Color(1f,1f,1f,0.35f);
 static readonly Color WHITE_30 = new Color(1f,1f,1f,0.30f);
 static readonly Color WHITE_20 = new Color(1f,1f,1f,0.20f);

 void Awake() => Build();

 void OnEnable()
 {
 if (PanelAudio == null)
 FindExistingRefs();
 }

 void Build()
 {
 if (canvasRoot == null) { Debug.LogError("[SettingsLayout] Canvas Root not assigned."); return; }
 if (canvasRoot.childCount > 0) { FindExistingRefs(); return; }

 // Background 
 var bg = Make("Background", canvasRoot); Stretch(bg); Img(bg, BG);

 // Glows 
 var g1 = Make("Glow1", canvasRoot); Place(g1, 400f, 200f, 600f, 600f);
 Img(g1, new Color(0f,0.282f,1f,0.10f));
 var g2 = Make("Glow2", canvasRoot); Place(g2, -450f, -200f, 400f, 400f);
 Img(g2, new Color(0f,0.282f,1f,0.07f));

 // Topbar 
 var topbar = Make("Topbar", canvasRoot); Place(topbar, 0f, 327f, 1280f, 66f);
 Img(topbar, new Color(1f,1f,1f,0.03f));

 var logoBadge = Make("LogoBadge", topbar); Place(logoBadge, -596f, 0f, 46f, 46f);
 Img(logoBadge, BLUE);
 var logoT = MakeTMP("LogoT", logoBadge, "T", 20f, FontStyles.Bold, WHITE);
 Place(logoT, 0f, 0f, 46f, 46f); Align(logoT, TextAlignmentOptions.Center);

 var brandName = MakeTMP("BrandName", topbar, "TynassIt", 18f, FontStyles.Bold, WHITE);
 Place(brandName, -548f, 4f, 100f, 28f); Align(brandName, TextAlignmentOptions.Left);
 var brandSub = MakeTMP("BrandSub", topbar, "TRAINING PLATFORM", 9f, FontStyles.Normal, new Color(1f,1f,1f,0.40f));
 Place(brandSub, -548f, -10f, 140f, 16f); Align(brandSub, TextAlignmentOptions.Left);

 // Employee pill
 var empPill = Make("EmpPill", topbar); Place(empPill, 530f, 0f, 200f, 44f);
 Img(empPill, BLUE_DIM); Border(empPill, BLUE_BORDER);
 var empAv = Make("EmpAv", empPill); Place(empAv, -74f, 0f, 32f, 32f); Img(empAv, BLUE);
 EmpInitials = MakeTMP("EmpInit", empAv, "AM", 12f, FontStyles.Bold, WHITE);
 Place(EmpInitials, 0f, 0f, 32f, 32f); Align(EmpInitials, TextAlignmentOptions.Center);
 EmpName = MakeTMP("EmpName", empPill, "Ahmed Maalej", 14f, FontStyles.Bold, WHITE);
 Place(EmpName, 16f, 0f, 136f, 28f); Align(EmpName, TextAlignmentOptions.Left);

 // Back button
 var backRect = Make("BackBtn", topbar); Place(backRect, -440f, 0f, 100f, 40f);
 Img(backRect, INPUT_BG); Border(backRect, INPUT_BORDER);
 BackBtn = backRect.gameObject.AddComponent<Button>();
 var backLbl = MakeTMP("L", backRect, "← Back", 14f, FontStyles.Bold, WHITE_55);
 Place(backLbl, 0f, 0f, 100f, 40f); Align(backLbl, TextAlignmentOptions.Center);

 // Separator 
 var sep = Make("Separator", canvasRoot); Place(sep, 0f, 294f, 1280f, 1f);
 Img(sep, new Color(1f,1f,1f,0.06f));

 // Sidebar 
 var sidebar = Make("Sidebar", canvasRoot); Place(sidebar, -482f, -30f, 220f, 510f);
 Img(sidebar, CARD); Border(sidebar, CARD_BORDER);

 var lblSettings = MakeTMP("LblSettings", sidebar, "SETTINGS", 10f, FontStyles.Bold, new Color(1f,1f,1f,0.35f));
 Place(lblSettings, 0f, 230f, 180f, 18f); Align(lblSettings, TextAlignmentOptions.Left);
 lblSettings.characterSpacing = 2f;

 (SidebarAudio, SidebarAudioBg) = SidebarBtn(sidebar, " Audio", 160f, true);
 (SidebarLanguage, SidebarLanguageBg) = SidebarBtn(sidebar, " Language", 108f, false);
 (SidebarController, SidebarControllerBg) = SidebarBtn(sidebar, " Controller", 56f, false);

 var div = Make("Divider", sidebar); Place(div, 0f, 20f, 190f, 1f); Img(div, ROW_LINE);

 var lblAccount = MakeTMP("LblAccount", sidebar, "ACCOUNT", 10f, FontStyles.Bold, new Color(1f,1f,1f,0.35f));
 Place(lblAccount, 0f, 0f, 180f, 18f); Align(lblAccount, TextAlignmentOptions.Left);
 lblAccount.characterSpacing = 2f;

 (SidebarProfile, SidebarProfileBg) = SidebarBtn(sidebar, " Profile", -30f, false);
 (SidebarSignOut, SidebarSignOutBg) = SidebarBtn(sidebar, " Sign Out", -82f, false);

 // PANEL AUDIO 
 var panelAudio = Make("Panel_Audio", canvasRoot); Place(panelAudio, 98f, -22f, 900f, 510f);
 Img(panelAudio, Color.clear);
 PanelAudio = panelAudio.gameObject;

 var audTitle = MakeTMP("Title", panelAudio, "Audio Settings", 20f, FontStyles.Bold, WHITE);
 Place(audTitle, -320f, 236f, 300f, 30f); Align(audTitle, TextAlignmentOptions.Left);

 var volCard = Make("VolumeCard", panelAudio); Place(volCard, 0f, 100f, 900f, 190f);
 Img(volCard, CARD); Border(volCard, CARD_BORDER);
 SectionTitle("Volume", volCard, 0f, 78f);

 (MasterFill, MasterVal, MasterMinus, MasterPlus) = SliderRow(volCard, "", "Master Volume", "Overall audio output level", 40f, 0.75f, "75%");
 (SfxFill, SfxVal, SfxMinus, SfxPlus) = SliderRow(volCard, "", "Sound Effects", "In-simulation sound effects", -4f, 0.60f, "60%");
 (NarrFill, NarrVal, NarrMinus, NarrPlus) = SliderRow(volCard, "", "Voice Narration", "Scenario instructions read aloud", -48f, 0.90f, "90%");

 var optCard = Make("OptionsCard", panelAudio); Place(optCard, 0f, -80f, 900f, 114f);
 Img(optCard, CARD); Border(optCard, CARD_BORDER);
 SectionTitle("AudioOptions", optCard, 0f, 42f);

 (MuteBg, MuteKnob) = ToggleRow(optCard, "", "Mute All Audio", "Silence everything instantly", 10f, false);
 (SpatialBg, SpatialKnob) = ToggleRow(optCard, "", "Spatial Audio", "3D positional sound in VR", -32f, true);

 // PANEL LANGUAGE 
 var panelLang = Make("Panel_Language", canvasRoot); Place(panelLang, 98f, -22f, 900f, 510f);
 Img(panelLang, Color.clear);
 PanelLanguage = panelLang.gameObject;
 PanelLanguage.SetActive(false);

 var langTitle = MakeTMP("Title", panelLang, "Language", 20f, FontStyles.Bold, WHITE);
 Place(langTitle, -380f, 236f, 200f, 30f); Align(langTitle, TextAlignmentOptions.Left);

 var uiLangCard = Make("UILangCard", panelLang); Place(uiLangCard, 0f, 124f, 900f, 90f);
 Img(uiLangCard, CARD); Border(uiLangCard, CARD_BORDER);
 SectionTitle("UILang", uiLangCard, 0f, 34f);
 IconInfo(uiLangCard, "", "UI Language", "Menus, buttons, and labels", -100f, -2f);
 (LangFR, LangEN, LangAR) = OptionGroup3(uiLangCard, "Français","English","عربية", 260f, -2f, true);

 var narrCard = Make("NarrationCard", panelLang); Place(narrCard, 0f, -6f, 900f, 130f);
 Img(narrCard, CARD); Border(narrCard, CARD_BORDER);
 SectionTitle("Narration", narrCard, 0f, 50f);
 IconInfo(narrCard, "", "Voice Language", "Spoken instructions in simulation", -100f, 16f);
 (NarrFR, NarrEN, NarrAR) = OptionGroup3(narrCard, "Français","English","عربية", 260f, 16f, true);
 (SubtitleBg, SubtitleKnob) = ToggleRow(narrCard, "", "Show Subtitles", "Display text alongside narration", -30f, true);

 // PANEL CONTROLLER 
 var panelCtrl = Make("Panel_Controller", canvasRoot); Place(panelCtrl, 98f, -22f, 900f, 510f);
 Img(panelCtrl, Color.clear);
 PanelController = panelCtrl.gameObject;
 PanelController.SetActive(false);

 var ctrlTitle = MakeTMP("Title", panelCtrl, "Controller & Interaction", 20f, FontStyles.Bold, WHITE);
 Place(ctrlTitle, -230f, 236f, 440f, 30f); Align(ctrlTitle, TextAlignmentOptions.Left);

 var laserCard = Make("LaserCard", panelCtrl); Place(laserCard, -232f, 60f, 436f, 240f);
 Img(laserCard, CARD); Border(laserCard, CARD_BORDER);
 SectionTitle("Laser", laserCard, 0f, 106f);
 IconInfo(laserCard, "", "Dominant Hand", "Ray cast origin", -60f, 60f);
 (HandRight, HandLeft) = OptionGroup2(laserCard, "Right Hand", "Left Hand", 0f, 14f, true);
 (GlowBg, GlowKnob) = ToggleRow(laserCard, "", "Pointer Glow", "Visual ray effect", -40f, true);
 (HapticBg, HapticKnob) = ToggleRow(laserCard, "", "Haptic Feedback", "Vibrate on selection", -84f, true);

 var interCard = Make("InteractionCard", panelCtrl); Place(interCard, 232f, 60f, 436f, 240f);
 Img(interCard, CARD); Border(interCard, CARD_BORDER);
 SectionTitle("Interact", interCard, 0f, 106f);
 IconInfo(interCard, "", "Selection Mode", "How to confirm a choice", -60f, 60f);
 (SelectTrigger, SelectDwell) = OptionGroup2(interCard, "Trigger Press", "Dwell (Gaze)", 0f, 14f, true);
 (DwellFill, DwellVal, DwellMinus, DwellPlus) = SliderRow(interCard, "⏱", "Dwell Time", "Seconds to auto-select", -42f, 0.25f, "0.5s");
 (SoundBg, SoundKnob) = ToggleRow(interCard, "", "Selection Sound", "Click sound on confirm", -86f, true);

 // BOTTOM NAV 
 var nav = Make("BottomNav", canvasRoot); Place(nav, 0f, -304f, 1280f, 66f);
 Img(nav, Color.clear);

 var soRect = Make("SignOutBtn", nav); Place(soRect, -260f, 0f, 200f, 56f);
 Img(soRect, INPUT_BG); Border(soRect, INPUT_BORDER);
 SignOutBtn = soRect.gameObject.AddComponent<Button>();
 var soLbl = MakeTMP("L", soRect, " Sign Out", 15f, FontStyles.Bold, WHITE_55);
 Place(soLbl, 0f, 0f, 200f, 56f); Align(soLbl, TextAlignmentOptions.Center);

 var discRect = Make("DiscardBtn", nav); Place(discRect, 0f, 0f, 200f, 56f);
 Img(discRect, INPUT_BG); Border(discRect, INPUT_BORDER);
 DiscardBtn = discRect.gameObject.AddComponent<Button>();
 var discLbl = MakeTMP("L", discRect, "Discard", 15f, FontStyles.Bold, WHITE_55);
 Place(discLbl, 0f, 0f, 200f, 56f); Align(discLbl, TextAlignmentOptions.Center);

 var saveRect = Make("SaveBtn", nav); Place(saveRect, 224f, 0f, 220f, 56f);
 Img(saveRect, BLUE);
 SaveBtn = saveRect.gameObject.AddComponent<Button>();
 var saveLbl = MakeTMP("L", saveRect, " Save Settings", 16f, FontStyles.Bold, WHITE);
 Place(saveLbl, 0f, 0f, 220f, 56f); Align(saveLbl, TextAlignmentOptions.Center);
 }

 // =========================================================================
 // COMPONENT BUILDERS
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

 // Modified SliderRow — now returns fill, label, minus button, plus button
 (Image fill, TMP_Text val, Button minus, Button plus) SliderRow(
 RectTransform parent, string icon, string name, string desc, float y, float pct, string valStr)
 {
 IconInfo(parent, icon, name, desc, -100f, y);

 // − button
 var minusRect = Make("Minus_"+name, parent); Place(minusRect, 106f, y, 32f, 32f);
 Img(minusRect, INPUT_BG); Border(minusRect, INPUT_BORDER);
 var minusBtn = minusRect.gameObject.AddComponent<Button>();
 var minusLbl = MakeTMP("L", minusRect, "−", 20f, FontStyles.Bold, WHITE);
 Place(minusLbl, 0f, 0f, 32f, 32f); Align(minusLbl, TextAlignmentOptions.Center);

 // Track
 var track = Make("Track_"+name, parent); Place(track, 220f, y, 160f, 6f);
 Img(track, SLIDER_BG);
 var fillRect = Make("Fill", track);
 fillRect.anchorMin = new Vector2(0f, 0f); fillRect.anchorMax = new Vector2(pct, 1f);
 fillRect.pivot = new Vector2(0f, 0.5f); fillRect.anchoredPosition = Vector2.zero;
 fillRect.sizeDelta = Vector2.zero;
 var fill = fillRect.gameObject.AddComponent<Image>(); fill.color = BLUE;

 // + button
 var plusRect = Make("Plus_"+name, parent); Place(plusRect, 334f, y, 32f, 32f);
 Img(plusRect, INPUT_BG); Border(plusRect, INPUT_BORDER);
 var plusBtn = plusRect.gameObject.AddComponent<Button>();
 var plusLbl = MakeTMP("L", plusRect, "+", 20f, FontStyles.Bold, WHITE);
 Place(plusLbl, 0f, 0f, 32f, 32f); Align(plusLbl, TextAlignmentOptions.Center);

 // Value label
 var vLbl = MakeTMP("Val_"+name, parent, valStr, 14f, FontStyles.Bold, WHITE);
 Place(vLbl, 400f, y, 60f, 22f); Align(vLbl, TextAlignmentOptions.Right);

 return (fill, vLbl, minusBtn, plusBtn);
 }

 (Image bg, Image knob) ToggleRow(RectTransform parent, string icon, string name, string desc, float y, bool isOn)
 {
 IconInfo(parent, icon, name, desc, -100f, y);
 var pill = Make("Toggle_"+name, parent); Place(pill, 380f, y, 56f, 30f);
 var bg = Img(pill, isOn ? BLUE : TOGGLE_OFF);
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
 Img(r1, firstSelected ? BLUE_DIM : Color.clear); Border(r1, firstSelected ? BLUE_BORDER : INPUT_BORDER);
 var b1 = r1.gameObject.AddComponent<Button>();
 var t1 = MakeTMP("L", r1, l1, 13f, FontStyles.Bold, firstSelected ? WHITE : WHITE_50);
 Place(t1, 0f, 0f, 120f, 38f); Align(t1, TextAlignmentOptions.Center);

 var r2 = Make("Opt2", parent); Place(r2, x + 70f, y, 120f, 38f);
 Img(r2, !firstSelected ? BLUE_DIM : Color.clear); Border(r2, !firstSelected ? BLUE_BORDER : INPUT_BORDER);
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
 Img(r, sel ? BLUE_DIM : Color.clear); Border(r, sel ? BLUE_BORDER : INPUT_BORDER);
 btns[i] = r.gameObject.AddComponent<Button>();
 var t = MakeTMP("L", r, labels[i], 13f, FontStyles.Bold, sel ? WHITE : WHITE_50);
 Place(t, 0f, 0f, 84f, 38f); Align(t, TextAlignmentOptions.Center);
 }
 return (btns[0], btns[1], btns[2]);
 }

 // =========================================================================
 // HELPERS
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
 rt.anchoredPosition = new Vector2(x, y);
 rt.sizeDelta = new Vector2(w, h);
 }

 void Place(TMP_Text t, float x, float y, float w, float h)
 => Place(t.GetComponent<RectTransform>(), x, y, w, h);

 void Stretch(RectTransform rt)
 {
 rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 }

 Image Img(RectTransform rt, Color color)
 {
 var img = rt.gameObject.AddComponent<Image>();
 img.color = color;
 return img;
 }

 void Border(RectTransform rt, Color color)
 {
 var o = rt.gameObject.AddComponent<Outline>();
 o.effectColor = color;
 o.effectDistance = new Vector2(1f, 1f);
 }

 TMP_Text MakeTMP(string name, RectTransform parent, string text, float size, FontStyles style, Color color)
 {
 var go = new GameObject(name, typeof(RectTransform));
 go.transform.SetParent(parent, false);
 var t = go.AddComponent<TextMeshProUGUI>();
 t.text = text; t.fontSize = size; t.fontStyle = style; t.color = color;
 if (font != null) t.font = font;
 return t;
 }

 void Align(TMP_Text t, TextAlignmentOptions a) => t.alignment = a;

 static Color C(string hex) { ColorUtility.TryParseHtmlString("#" + hex, out Color c); return c; }

 public void RefreshRefs() => FindExistingRefs();

 private void FindExistingRefs()
 {
 PanelAudio = canvasRoot.Find("Panel_Audio")?.gameObject;
 PanelLanguage = canvasRoot.Find("Panel_Language")?.gameObject;
 PanelController = canvasRoot.Find("Panel_Controller")?.gameObject;

 var topbar = canvasRoot.Find("Topbar");
 if (topbar != null)
 {
 BackBtn = topbar.Find("BackBtn")?.GetComponent<Button>();
 EmpInitials = topbar.Find("EmpPill/EmpAv/EmpInit")?.GetComponent<TMP_Text>();
 EmpName = topbar.Find("EmpPill/EmpName")?.GetComponent<TMP_Text>();
 }

 var sidebar = canvasRoot.Find("Sidebar");
 if (sidebar != null)
 {
 SidebarAudioBg = sidebar.Find("Btn_ Audio")?.GetComponent<Image>();
 SidebarLanguageBg = sidebar.Find("Btn_ Language")?.GetComponent<Image>();
 SidebarControllerBg = sidebar.Find("Btn_ Controller")?.GetComponent<Image>();
 SidebarAudio = SidebarAudioBg?.GetComponent<Button>();
 SidebarLanguage = SidebarLanguageBg?.GetComponent<Button>();
 SidebarController = SidebarControllerBg?.GetComponent<Button>();
 SidebarSignOut = sidebar.Find("Btn_ SignOut")?.GetComponent<Button>();
 SidebarSignOutBg = SidebarSignOut?.GetComponent<Image>();
 }

 var nav = canvasRoot.Find("BottomNav");
 if (nav != null)
 {
 SignOutBtn = nav.Find("SignOutBtn")?.GetComponent<Button>();
 DiscardBtn = nav.Find("DiscardBtn")?.GetComponent<Button>();
 SaveBtn = nav.Find("SaveBtn")?.GetComponent<Button>();
 }

 if (PanelAudio != null)
 {
 var volCard = PanelAudio.transform.Find("VolumeCard");
 if (volCard != null)
 {
 MasterFill = volCard.Find("Track_Master Volume/Fill")?.GetComponent<Image>();
 MasterVal = volCard.Find("Val_Master Volume")?.GetComponent<TMP_Text>();
 MasterMinus = volCard.Find("Minus_Master Volume")?.GetComponent<Button>();
 MasterPlus = volCard.Find("Plus_Master Volume")?.GetComponent<Button>();
 SfxFill = volCard.Find("Track_Sound Effects/Fill")?.GetComponent<Image>();
 SfxVal = volCard.Find("Val_Sound Effects")?.GetComponent<TMP_Text>();
 SfxMinus = volCard.Find("Minus_Sound Effects")?.GetComponent<Button>();
 SfxPlus = volCard.Find("Plus_Sound Effects")?.GetComponent<Button>();
 NarrFill = volCard.Find("Track_Voice Narration/Fill")?.GetComponent<Image>();
 NarrVal = volCard.Find("Val_Voice Narration")?.GetComponent<TMP_Text>();
 NarrMinus = volCard.Find("Minus_Voice Narration")?.GetComponent<Button>();
 NarrPlus = volCard.Find("Plus_Voice Narration")?.GetComponent<Button>();
 }
 var optCard = PanelAudio.transform.Find("OptionsCard");
 if (optCard != null)
 {
 MuteBg = optCard.Find("Toggle_Mute All Audio")?.GetComponent<Image>();
 MuteKnob = optCard.Find("Toggle_Mute All Audio/Knob")?.GetComponent<Image>();
 SpatialBg = optCard.Find("Toggle_Spatial Audio")?.GetComponent<Image>();
 SpatialKnob = optCard.Find("Toggle_Spatial Audio/Knob")?.GetComponent<Image>();
 }
 }

 if (PanelLanguage != null)
 {
 var uiCard = PanelLanguage.transform.Find("UILangCard");
 if (uiCard != null)
 {
 LangFR = uiCard.Find("Opt1")?.GetComponent<Button>();
 LangEN = uiCard.Find("Opt2")?.GetComponent<Button>();
 LangAR = uiCard.Find("Opt3")?.GetComponent<Button>();
 }
 var narrCard = PanelLanguage.transform.Find("NarrationCard");
 if (narrCard != null)
 {
 NarrFR = narrCard.Find("Opt1")?.GetComponent<Button>();
 NarrEN = narrCard.Find("Opt2")?.GetComponent<Button>();
 NarrAR = narrCard.Find("Opt3")?.GetComponent<Button>();
 SubtitleBg = narrCard.Find("Toggle_Show Subtitles")?.GetComponent<Image>();
 SubtitleKnob = narrCard.Find("Toggle_Show Subtitles/Knob")?.GetComponent<Image>();
 }
 }

 if (PanelController != null)
 {
 var laserCard = PanelController.transform.Find("LaserCard");
 if (laserCard != null)
 {
 HandRight = laserCard.Find("Opt1")?.GetComponent<Button>();
 HandLeft = laserCard.Find("Opt2")?.GetComponent<Button>();
 GlowBg = laserCard.Find("Toggle_Pointer Glow")?.GetComponent<Image>();
 GlowKnob = laserCard.Find("Toggle_Pointer Glow/Knob")?.GetComponent<Image>();
 HapticBg = laserCard.Find("Toggle_Haptic Feedback")?.GetComponent<Image>();
 HapticKnob = laserCard.Find("Toggle_Haptic Feedback/Knob")?.GetComponent<Image>();
 }
 var interCard = PanelController.transform.Find("InteractionCard");
 if (interCard != null)
 {
 SelectTrigger = interCard.Find("Opt1")?.GetComponent<Button>();
 SelectDwell = interCard.Find("Opt2")?.GetComponent<Button>();
 DwellFill = interCard.Find("Track_Dwell Time/Fill")?.GetComponent<Image>();
 DwellVal = interCard.Find("Val_Dwell Time")?.GetComponent<TMP_Text>();
 DwellMinus = interCard.Find("Minus_Dwell Time")?.GetComponent<Button>();
 DwellPlus = interCard.Find("Plus_Dwell Time")?.GetComponent<Button>();
 SoundBg = interCard.Find("Toggle_Selection Sound")?.GetComponent<Image>();
 SoundKnob = interCard.Find("Toggle_Selection Sound/Knob")?.GetComponent<Image>();
 }
 }

 Debug.Log("[SettingsLayout] Refs restored from existing hierarchy ");
 }

}