using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Training Modules Layout
/// Pure UI script. No logic. No API. No navigation.
/// Builds the entire Modules panel AND its full hierarchy from scratch.
///
/// HIERARCHY CREATED:
/// Canvas
/// ├── Background              (Image #0A1628, stretch)
/// ├── Glow1                   (Image blue 13%, 600x600)
/// ├── Glow2                   (Image blue 9%,  400x400)
/// ├── Topbar                  (transparent, 1280x66, y=327)
/// │   ├── LogoBadge           (Image #0048FF, 46x46)
/// │   │   └── LogoT           (TMP "T")
/// │   ├── BrandName           (TMP "TynassIt")
/// │   ├── BrandSub            (TMP "TRAINING PLATFORM")
/// │   ├── SearchField         (TMP_InputField, 300x52)
/// │   │   └── Viewport/Text/Placeholder
/// │   └── CompanyPill         (Image white7%, 210x44)
/// │       ├── CompanyAvatar   (Image #0048FF, 32x32)
/// │       │   └── Initials    (TMP)
/// │       └── CompanyName     (TMP)
/// ├── SectionHeader           (transparent, 1176x30, y=248)
/// │   ├── SectionTitle        (TMP "Training Modules")
/// │   └── SectionCount        (TMP "4 modules assigned")
/// ├── ModulesGrid             (transparent, 1176x420, y=20)
/// │   ├── ModuleCard_0        (280x420 — Customs Inspection)
/// │   ├── ModuleCard_1        (280x420 — Electrical Safety)
/// │   ├── ModuleCard_2        (280x420 — Fire Emergency)
/// │   └── ModuleCard_3        (280x420 — Construction Safety)
/// └── BottomNav               (transparent, 1280x72, y=-304)
///     ├── SettingsBtn         (Image white7%, 200x56)
///     ├── StartSimBtn         (Image #0048FF, 260x56)
///     └── OverviewBtn         (Image blue15%, 230x56)
///
/// HOW TO USE:
///   1. Create empty GameObject in scene
///   2. Attach ModulesLayout.cs
///   3. Assign Canvas Root + Font in Inspector
///   4. Press Play
/// </summary>
public class ModulesLayout : MonoBehaviour
{
    [Header("Canvas Root — your 1280x720 World Space Canvas")]
    [SerializeField] RectTransform canvasRoot;

    [Header("Font — TMP Font Asset")]
    [SerializeField] TMP_FontAsset font;

    [Header("Sprites")]
    [SerializeField] private Sprite uiSprite;

    // ── Public refs — auto-filled, read by ModulesPanel.cs ───────────────────
    [Header("Built refs — auto-filled")]
    public TMP_Text       CompanyInitials;
    public TMP_Text       CompanyName;
    public TMP_InputField SearchInput;
    public TMP_Text       SectionCountLabel;
    public RectTransform  ModulesGrid;          // parent of all 4 cards
    public ModuleCardRefs[] Cards;              // array of 4 card ref bundles
    public Button         SettingsBtn;
    public Button         StartSimBtn;
    public TMP_Text       StartSimLabel;
    public Button         OverviewBtn;

    // ── Colours ───────────────────────────────────────────────────────────────
    static readonly Color BG           = C("0A1628");
    static readonly Color BLUE         = C("0048FF");
    static readonly Color BLUE_DIM     = new Color(0f,0.282f,1f,0.15f);
    static readonly Color BLUE_BORDER  = new Color(0f,0.282f,1f,0.40f);
    static readonly Color CARD_BG      = new Color(1f,1f,1f,0.06f);
    static readonly Color CARD_BORDER  = new Color(1f,1f,1f,0.10f);
    static readonly Color INPUT_BG     = new Color(1f,1f,1f,0.07f);
    static readonly Color INPUT_BORDER = new Color(1f,1f,1f,0.11f);
    static readonly Color WHITE        = Color.white;
    static readonly Color WHITE_75     = new Color(1f,1f,1f,0.75f);
    static readonly Color WHITE_65     = new Color(1f,1f,1f,0.65f);
    static readonly Color WHITE_45     = new Color(1f,1f,1f,0.45f);
    static readonly Color WHITE_40     = new Color(1f,1f,1f,0.40f);
    static readonly Color WHITE_35     = new Color(1f,1f,1f,0.35f);
    static readonly Color WHITE_20     = new Color(1f,1f,1f,0.20f);
    static readonly Color WHITE_10     = new Color(1f,1f,1f,0.10f);
    static readonly Color GREEN        = C("00AA44");
    static readonly Color ACCENT       = C("98B5FF");

    // ── Card data (matches the HTML) ─────────────────────────────────────────
    struct CardData
    {
        public string Name, Desc, Duration, Badge, Pct, Icon;
        public Color  ThumbA, ThumbB, BadgeColor, BarColor;
        public bool   Locked;
    }

    readonly CardData[] _cards = new CardData[]
    {
        new CardData {
            Name="Customs Inspection", Desc="Identify illegal items and apply correct inspection procedures.",
            Duration="10–15 min", Badge="In Progress", Pct="65%", Icon="🛃",
            ThumbA=C2("0A1F5C"), ThumbB=new Color(0f,0.282f,1f,0.40f),
            BadgeColor=new Color(0f,0.282f,1f,0.90f), BarColor=C2("0048FF"), Locked=false },

        new CardData {
            Name="Electrical Safety", Desc="Safe handling of electrical equipment in industrial settings.",
            Duration="20–25 min", Badge="New", Pct="0%", Icon="⚡",
            ThumbA=C2("1A0A3C"), ThumbB=new Color(0.4f,0f,1f,0.27f),
            BadgeColor=new Color(0f,0.627f,0.353f,0.90f), BarColor=C2("0048FF"), Locked=false },

        new CardData {
            Name="Fire Emergency Response", Desc="React correctly to fire hazards and evacuation procedures.",
            Duration="15–20 min", Badge="Completed", Pct="100%", Icon="🔥",
            ThumbA=C2("0A2A1A"), ThumbB=new Color(0f,0.533f,0.2f,0.27f),
            BadgeColor=new Color(0f,0.627f,0.267f,0.90f), BarColor=C2("00AA44"), Locked=false },

        new CardData {
            Name="Construction Site Safety", Desc="PPE usage and hazard identification on construction sites.",
            Duration="25–30 min", Badge="Locked", Pct="—", Icon="🏗",
            ThumbA=C2("1A1A2E"), ThumbB=new Color(0.2f,0.2f,0.27f,0.27f),
            BadgeColor=new Color(1f,1f,1f,0.15f), BarColor=C2("0048FF"), Locked=true },
    };

    // ─────────────────────────────────────────────────────────────────────────
    void Awake() => Build();

    void Build()
    {
        if (canvasRoot == null)
        {
            Debug.LogError("[ModulesLayout] Canvas Root not assigned.");
            return;
        }

        for (int i = canvasRoot.childCount - 1; i >= 0; i--)
            Destroy(canvasRoot.GetChild(i).gameObject);

        // ── Background ────────────────────────────────────────────────────────
        var bg = Make("Background", canvasRoot); Stretch(bg); Img(bg, BG);

        // ── Glow 1 — top right  600x600 at (250, 160) ────────────────────────
        var g1 = Make("Glow1", canvasRoot);
        Place(g1, 360f, 240f, 700f, 700f);
        var col = C("0048FF");
        col.a = 0.025f;
        var g1Img = Img(g1, col);
        g1Img.type = UnityEngine.UI.Image.Type.Sliced;
        g1Img.fillCenter = false;
        g1Img.pixelsPerUnitMultiplier = 0.01f;
        g1Img.sprite = uiSprite;
        g1.localScale = new Vector3(3f, 3f, 3f);
        // ── Glow 2 — bottom left  400x400 at (-380, -210) ────────────────────
        var g2 = Make("Glow2", canvasRoot);
        Place(g2, -434f, -218f, 450f, 450f);
        col.a = 0.035f;
        var g2Img = Img(g2, col);
        g2Img.type = UnityEngine.UI.Image.Type.Sliced;
        g2Img.fillCenter = false;
        g2Img.pixelsPerUnitMultiplier = 0.01f;
        g2Img.sprite = uiSprite;
        g2.localScale = new Vector3(3f, 3f, 3f);

        // =====================================================================
        //  TOPBAR  — 1280x66 at (0, 327)
        // =====================================================================
        var topbar = Make("Topbar", canvasRoot);
        Place(topbar, 0f, 327f, 1280f, 66f);
        Img(topbar, Color.clear);

        // Logo Badge — 46x46 at (-584, 0) inside Topbar
        var logoBadge = Make("LogoBadge", topbar);
        Place(logoBadge, -584f, 0f, 46f, 46f);
        Img(logoBadge, BLUE);
        var logoT = MakeTMP("LogoT", logoBadge, "T", 20f, FontStyles.Bold, WHITE);
        Place(logoT, 0f, 0f, 46f, 46f); Align(logoT, TextAlignmentOptions.Center);

        // Brand Name — at (-520, 8)
        var brandName = MakeTMP("BrandName", topbar, "TynassIt", 17f, FontStyles.Bold, WHITE);
        Place(brandName, -492f, 8f, 120f, 24f); Align(brandName, TextAlignmentOptions.Left);

        // Brand Sub — at (-516, -8)
        var brandSub = MakeTMP("BrandSub", topbar, "TRAINING PLATFORM", 9f, FontStyles.Normal, new Color(1f,1f,1f,0.35f));
        Place(brandSub, -474f, -8f, 126f, 14f); Align(brandSub, TextAlignmentOptions.Left);
        brandSub.characterSpacing = 2f;

        // Search Field — 300x52 at (0, 0) inside Topbar — centered
        SearchInput = MakeInputField("SearchField", topbar, 0f, 0f, 300f, 52f, "🔍  Search modules...", false);

        // Company Pill — 210x44 at (528, 0)
        var pill = Make("CompanyPill", topbar);
        Place(pill, 528f, 0f, 210f, 44f);
        Img(pill, INPUT_BG); Border(pill, INPUT_BORDER);

        // Avatar circle 32x32 at (-74, 0) inside pill
        var avatar = Make("CompanyAvatar", pill);
        Place(avatar, -74f, 0f, 32f, 32f); Img(avatar, BLUE);
        CompanyInitials = MakeTMP("Initials", avatar, "AC", 11f, FontStyles.Bold, WHITE);
        Place(CompanyInitials, 0f, 0f, 32f, 32f); Align(CompanyInitials, TextAlignmentOptions.Center);

        // Company name at (20, 0) inside pill
        CompanyName = MakeTMP("CompanyName", pill, "Acme Corp", 13f, FontStyles.Bold, WHITE_75);
        Place(CompanyName, 20f, 0f, 130f, 44f); Align(CompanyName, TextAlignmentOptions.Left);

        // Topbar bottom separator
        var sep = Make("Separator", canvasRoot);
        Place(sep, 0f, 294f, 1280f, 1f); Img(sep, new Color(1f,1f,1f,0.07f));

        // =====================================================================
        //  SECTION HEADER — 1176x30 at (0, 258)
        // =====================================================================
        var sectionHeader = Make("SectionHeader", canvasRoot);
        Place(sectionHeader, 0f, 258f, 1176f, 30f); Img(sectionHeader, Color.clear);

        var sectionTitle = MakeTMP("SectionTitle", sectionHeader, "Training Modules", 20f, FontStyles.Bold, WHITE);
        Place(sectionTitle, -440f, 0f, 300f, 30f); Align(sectionTitle, TextAlignmentOptions.Left);

        SectionCountLabel = MakeTMP("SectionCount", sectionHeader, "4 modules assigned", 14f, FontStyles.Normal, WHITE_35);
        Place(SectionCountLabel, 440f, 0f, 200f, 30f); Align(SectionCountLabel, TextAlignmentOptions.Right);

        // =====================================================================
        //  MODULE CARDS GRID — 1176x400 at (0, 30)
        //  4 cards: each 276x400, spaced 18px apart
        //  x positions: -441, -147, 147, 441
        // =====================================================================
        var grid = Make("ModulesGrid", canvasRoot);
        Place(grid, 0f, 30f, 1176f, 400f); Img(grid, Color.clear);
        ModulesGrid = grid;

        Cards = new ModuleCardRefs[4];
        float[] cardX = { -441f, -147f, 147f, 441f };

        for (int i = 0; i < 4; i++)
            Cards[i] = BuildModuleCard(grid, _cards[i], cardX[i]);

        // =====================================================================
        //  BOTTOM NAV — 1280x72 at (0, -304)
        // =====================================================================
        var bottomNav = Make("BottomNav", canvasRoot);
        Place(bottomNav, 0f, -304f, 1280f, 72f); Img(bottomNav, Color.clear);

        // Settings button — 200x56 at (-260, 0)
        var settingsRect = Make("SettingsBtn", bottomNav);
        Place(settingsRect, -260f, 0f, 200f, 56f);
        Img(settingsRect, INPUT_BG); Border(settingsRect, INPUT_BORDER);
        SettingsBtn = settingsRect.gameObject.AddComponent<Button>();
        var settingsLabel = MakeTMP("Label", settingsRect, "⚙  Settings", 15f, FontStyles.Bold, WHITE_65);
        Place(settingsLabel, 0f, 0f, 200f, 56f); Align(settingsLabel, TextAlignmentOptions.Center);

        // Start Simulation button — 260x56 at (0, 0)  #0048FF
        var startRect = Make("StartSimBtn", bottomNav);
        Place(startRect, 0f, 0f, 260f, 56f); Img(startRect, BLUE);
        StartSimBtn = startRect.gameObject.AddComponent<Button>();
        StartSimLabel = MakeTMP("Label", startRect, "▶  Start Simulation", 16f, FontStyles.Bold, WHITE);
        Place(StartSimLabel, 0f, 0f, 260f, 56f); Align(StartSimLabel, TextAlignmentOptions.Center);

        // Training Overview button — 230x56 at (258, 0)  blue 15%
        var overviewRect = Make("OverviewBtn", bottomNav);
        Place(overviewRect, 258f, 0f, 230f, 56f);
        Img(overviewRect, BLUE_DIM); Border(overviewRect, BLUE_BORDER);
        OverviewBtn = overviewRect.gameObject.AddComponent<Button>();
        var overviewLabel = MakeTMP("Label", overviewRect, "📋  Training Overview", 15f, FontStyles.Bold, ACCENT);
        Place(overviewLabel, 0f, 0f, 230f, 56f); Align(overviewLabel, TextAlignmentOptions.Center);
    }

    // =========================================================================
    //  MODULE CARD BUILDER
    //  Card: 276x400, children anchored inside
    //
    //  ModuleCard_N
    //  ├── Thumb           (Image gradient bg, 276x116, y=142)
    //  │   ├── ThumbOverlay (Image gradient dark, stretch)
    //  │   └── Badge       (Image + TMP, 90x26, top-left)
    //  ├── ModuleName      (TMP, 240x44, y=56)
    //  ├── ModuleDesc      (TMP, 240x60, y=10)
    //  ├── DurationDot     (Image 5x5, y=-46)
    //  ├── DurationText    (TMP, y=-46)
    //  ├── ProgressPct     (TMP, y=-46, right)
    //  ├── ProgressBg      (Image white10%, 240x4, y=-68)
    //  └── ProgressFill    (Image colored, y=-68, left-anchored)
    // =========================================================================
    ModuleCardRefs BuildModuleCard(RectTransform parent, CardData d, float x)
    {
        var refs = new ModuleCardRefs();

        // Card root
        var card = Make($"ModuleCard_{d.Name.Replace(" ","")}", parent);
        Place(card, x, 0f, 276f, 400f);
        refs.CardBg = Img(card, CARD_BG);
        refs.CardBorder = card.gameObject.AddComponent<Outline>();
        refs.CardBorder.effectColor    = d.Locked ? new Color(1f,1f,1f,0.06f) : CARD_BORDER;
        refs.CardBorder.effectDistance = new Vector2(2f, 2f);
        refs.Card = card;
        refs.CardButton = card.gameObject.AddComponent<Button>();

        if (d.Locked)
        {
            var cg = card.gameObject.AddComponent<CanvasGroup>();
            cg.alpha          = 0.45f;
            cg.interactable   = false;
            cg.blocksRaycasts = false;
        }

        // ── Thumbnail area — 276x116 at (0, 142) ─────────────────────────────
        var thumb = Make("Thumb", card);
        Place(thumb, 0f, 142f, 276f, 116f);
        refs.ThumbBg = Img(thumb, d.ThumbA); // solid base color (gradient needs shader)

        // Thumb overlay (dark fade at bottom)
        var overlay = Make("ThumbOverlay", thumb);
        Stretch(overlay);
        Img(overlay, new Color(0.039f, 0.086f, 0.157f, 0.55f));

        // Emoji icon — centered in thumb
        var iconLabel = MakeTMP("Icon", thumb, d.Icon, 40f, FontStyles.Normal, WHITE);
        Place(iconLabel, 0f, 10f, 276f, 80f); Align(iconLabel, TextAlignmentOptions.Center);
        refs.ThumbIcon = iconLabel;

        // Badge — 90x26 at (-83, 38) inside thumb (top-left)
        var badge = Make("Badge", thumb);
        Place(badge, -83f, 38f, 90f, 26f);
        refs.BadgeBg = Img(badge, d.BadgeColor);
        refs.BadgeLabel = MakeTMP("Label", badge, d.Badge, 10f, FontStyles.Bold, WHITE);
        Place(refs.BadgeLabel, 0f, 0f, 90f, 26f); Align(refs.BadgeLabel, TextAlignmentOptions.Center);
        refs.BadgeLabel.characterSpacing = 0.5f;

        // ── Module Name — 240x44 at (0, 60) ──────────────────────────────────
        refs.ModuleName = MakeTMP("ModuleName", card, d.Name, 15f, FontStyles.Bold, WHITE);
        Place(refs.ModuleName, 0f, 60f, 240f, 44f); Align(refs.ModuleName, TextAlignmentOptions.Left);
        refs.ModuleName.enableWordWrapping = true;
        refs.ModuleName.GetComponent<RectTransform>().anchoredPosition = new Vector2(-8f, 60f);

        // ── Module Desc — 240x70 at (-8, 14) ─────────────────────────────────
        refs.ModuleDesc = MakeTMP("ModuleDesc", card, d.Desc, 12f, FontStyles.Normal, WHITE_45);
        Place(refs.ModuleDesc, -8f, 14f, 240f, 70f); Align(refs.ModuleDesc, TextAlignmentOptions.Left);
        refs.ModuleDesc.enableWordWrapping = true;
        refs.ModuleDesc.lineSpacing = 4f;

        // ── Meta row — Duration dot + text + Pct — y = -52 ──────────────────
        // Blue dot — 5x5 at (-112, -52)
        var metaDot = Make("MetaDot", card);
        Place(metaDot, -112f, -52f, 5f, 5f); Img(metaDot, BLUE);

        // Duration text at (-88, -52)
        refs.DurationText = MakeTMP("Duration", card, d.Duration, 12f, FontStyles.Normal, WHITE_40);
        Place(refs.DurationText, -72f, -52f, 120f, 18f); Align(refs.DurationText, TextAlignmentOptions.Left);

        // Pct text at (100, -52)
        refs.ProgressPct = MakeTMP("ProgressPct", card, d.Pct, 12f, FontStyles.Bold, WHITE_35);
        Place(refs.ProgressPct, 100f, -52f, 50f, 18f); Align(refs.ProgressPct, TextAlignmentOptions.Right);

        // ── Progress bar — 240x4 at (-8, -72) ────────────────────────────────
        var barBg = Make("ProgressBg", card);
        Place(barBg, -8f, -72f, 240f, 4f); Img(barBg, WHITE_10);

        // Fill — left-anchored, width = pct * 240
        var barFill = Make("ProgressFill", barBg);
        float pctVal = d.Pct == "—" ? 0f : float.Parse(d.Pct.Replace("%","")) / 100f;
        barFill.anchorMin = new Vector2(0f, 0f);
        barFill.anchorMax = new Vector2(0f, 1f);
        barFill.pivot     = new Vector2(0f, 0.5f);
        barFill.anchoredPosition = Vector2.zero;
        barFill.sizeDelta = new Vector2(240f * pctVal, 0f);
        refs.ProgressFill = Img(barFill, d.BarColor);

        return refs;
    }

    // =========================================================================
    //  INPUT FIELD BUILDER
    // =========================================================================
    TMP_InputField MakeInputField(string name, RectTransform parent,
        float x, float y, float w, float h, string placeholder, bool isPassword)
    {
        var wrap = Make(name, parent);
        Place(wrap, x, y, w, h);
        Img(wrap, INPUT_BG); Border(wrap, INPUT_BORDER);

        var vp = Make("Viewport", wrap);
        vp.anchorMin = Vector2.zero; vp.anchorMax = Vector2.one;
        vp.offsetMin = new Vector2(16f, 0f); vp.offsetMax = new Vector2(-16f, 0f);
        vp.gameObject.AddComponent<RectMask2D>();

        var textGo = Make("Text", vp); Stretch(textGo);
        var textComp = textGo.gameObject.AddComponent<TextMeshProUGUI>();
        textComp.color = WHITE; textComp.fontSize = 15f;
        textComp.alignment = TextAlignmentOptions.MidlineLeft;
        if (font != null) textComp.font = font;

        var phGo = Make("Placeholder", vp); Stretch(phGo);
        var phComp = phGo.gameObject.AddComponent<TextMeshProUGUI>();
        phComp.text = placeholder; phComp.color = WHITE_20;
        phComp.fontSize = 15f; phComp.fontStyle = FontStyles.Italic;
        phComp.alignment = TextAlignmentOptions.MidlineLeft;
        if (font != null) phComp.font = font;

        var field = wrap.gameObject.AddComponent<TMP_InputField>();
        field.textViewport = vp; field.textComponent = textComp;
        field.placeholder = phComp; field.caretColor = BLUE; field.caretWidth = 2;
        if (isPassword) { field.contentType = TMP_InputField.ContentType.Password; field.ForceLabelUpdate(); }
        return field;
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
        img.color = color; return img;
    }

    void Border(RectTransform rt, Color color)
    {
        var o = rt.gameObject.AddComponent<Outline>();
        o.effectColor = color; o.effectDistance = new Vector2(1.5f, 1.5f);
    }

    TMP_Text MakeTMP(string name, RectTransform parent, string text,
        float size, FontStyles style, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.text = text; t.fontSize = size; t.fontStyle = style; t.color = color;
        if (font != null) t.font = font;
        return t;
    }

    void Align(TMP_Text t, TextAlignmentOptions a) => t.alignment = a;

    static Color C(string hex)
    { ColorUtility.TryParseHtmlString("#" + hex, out Color c); return c; }

    // Same as C() but static for use inside CardData initializer
    static Color C2(string hex) => C(hex);
}

// ── Card refs bundle — one per card, read by ModulesPanel.cs ─────────────────
[System.Serializable]
public class ModuleCardRefs
{
    public RectTransform Card;
    public Image         CardBg;
    public Outline       CardBorder;
    public Button        CardButton;
    public Image         ThumbBg;
    public TMP_Text      ThumbIcon;
    public Image         BadgeBg;
    public TMP_Text      BadgeLabel;
    public TMP_Text      ModuleName;
    public TMP_Text      ModuleDesc;
    public TMP_Text      DurationText;
    public TMP_Text      ProgressPct;
    public Image         ProgressFill;
}
