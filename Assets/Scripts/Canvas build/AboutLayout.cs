using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — About Us Layout
/// Pure UI script. No logic. No API. No navigation.
///
/// HIERARCHY:
/// Canvas
/// ├── Background              (stretch, #0A1628)
/// ├── Glow1                   (700x700, blue 13%, top-right)
/// ├── Glow2                   (500x500, blue 9%,  bottom-left)
/// ├── Topbar                  (1280x66, y=327)
/// │   ├── LogoBadge           (48x48, #0048FF)
/// │   │   └── LogoT
/// │   ├── BrandName
/// │   ├── BrandSub
/// │   └── BackBtn             (160x48)
/// ├── Separator               (1280x1, y=294)
/// ├── LeftColumn              (320x560, x=-420, y=-20)
/// │   ├── BrandCard           (320x240)
/// │   │   ├── LogoBadgeLg     (68x68)
/// │   │   ├── BrandNameLg
/// │   │   ├── BrandSubLg
/// │   │   └── Tagline
/// │   └── StatsRow            (320x90)
/// │       ├── StatCard_0      (Modules)
/// │       ├── StatCard_1      (Companies)
/// │       └── StatCard_2      (Sessions)
/// └── RightColumn             (780x560, x=180, y=-20)
///     ├── AboutCard           (780x130)
///     ├── ContactCard         (780x190)
///     └── BottomRow           (780x120)
///         ├── MissionCard     (380x120)
///         └── SpecialtiesCard (380x120)
/// </summary>
public class AboutLayout : MonoBehaviour
{
    [Header("Canvas Root")]
    [SerializeField] RectTransform canvasRoot;
    [Header("Font")]
    [SerializeField] TMP_FontAsset font;

    [Header("Built refs")]
    public Button   BackBtn;
    public TMP_Text AboutText;
    public TMP_Text ContactEmail;
    public TMP_Text ContactAddress;
    public TMP_Text ContactPhone;

    // ── Colours ───────────────────────────────────────────────────────────────
    static readonly Color BG          = C("0A1628");
    static readonly Color BLUE        = C("0048FF");
    static readonly Color BLUE_DIM    = new Color(0f,0.282f,1f,0.12f);
    static readonly Color BLUE_BORDER = new Color(0f,0.282f,1f,0.28f);
    static readonly Color CARD        = new Color(1f,1f,1f,0.06f);
    static readonly Color CARD_BORDER = new Color(1f,1f,1f,0.12f);
    static readonly Color INPUT_BG    = new Color(1f,1f,1f,0.07f);
    static readonly Color INPUT_BORDER= new Color(1f,1f,1f,0.11f);
    static readonly Color WHITE       = Color.white;
    static readonly Color WHITE_68    = new Color(1f,1f,1f,0.68f);
    static readonly Color WHITE_60    = new Color(1f,1f,1f,0.60f);
    static readonly Color WHITE_50    = new Color(1f,1f,1f,0.50f);
    static readonly Color WHITE_40    = new Color(1f,1f,1f,0.40f);
    static readonly Color WHITE_38    = new Color(1f,1f,1f,0.38f);
    static readonly Color WHITE_35    = new Color(1f,1f,1f,0.35f);
    static readonly Color WHITE_08    = new Color(1f,1f,1f,0.08f);
    static readonly Color ACCENT      = C("98B5FF");

    void Awake() => Build();

    void Build()
    {
        if (canvasRoot == null) { Debug.LogError("[AboutLayout] Canvas Root not assigned."); return; }
        for (int i = canvasRoot.childCount - 1; i >= 0; i--)
            Destroy(canvasRoot.GetChild(i).gameObject);

        // ── Background + Glows ────────────────────────────────────────────────
        var bg = Make("Background", canvasRoot); Stretch(bg); Img(bg, BG);
        var g1 = Make("Glow1", canvasRoot); Place(g1, 350f, 200f, 700f, 700f);
        Img(g1, new Color(0f,0.282f,1f,0.13f));
        var g2 = Make("Glow2", canvasRoot); Place(g2, -400f, -230f, 500f, 500f);
        Img(g2, new Color(0f,0.282f,1f,0.09f));

        // ── Topbar  1280x66 at y=327 ──────────────────────────────────────────
        var topbar = Make("Topbar", canvasRoot); Place(topbar, 0f, 327f, 1280f, 66f);
        Img(topbar, Color.clear);

        // Logo Badge 48x48 at (-584, 0)
        var badge = Make("LogoBadge", topbar); Place(badge, -584f, 0f, 48f, 48f);
        Img(badge, BLUE);
        var lt = MakeTMP("LogoT", badge, "T", 20f, FontStyles.Bold, WHITE);
        Place(lt, 0f, 0f, 48f, 48f); Align(lt, TextAlignmentOptions.Center);

        var bn = MakeTMP("BrandName", topbar, "TynassIt", 18f, FontStyles.Bold, WHITE);
        Place(bn, -516f, 8f, 120f, 24f); Align(bn, TextAlignmentOptions.Left);
        var bs = MakeTMP("BrandSub", topbar, "TRAINING PLATFORM", 9f, FontStyles.Normal, new Color(1f,1f,1f,0.35f));
        Place(bs, -510f, -8f, 160f, 14f); Align(bs, TextAlignmentOptions.Left);
        bs.characterSpacing = 2f;

        // Back button 160x48 at (548, 0)
        var backRect = Make("BackBtn", topbar); Place(backRect, 548f, 0f, 160f, 48f);
        Img(backRect, INPUT_BG); Border(backRect, INPUT_BORDER);
        BackBtn = backRect.gameObject.AddComponent<Button>();
        var backLbl = MakeTMP("Label", backRect, "← Back", 15f, FontStyles.Bold, new Color(1f,1f,1f,0.65f));
        Place(backLbl, 0f, 0f, 160f, 48f); Align(backLbl, TextAlignmentOptions.Center);

        // Separator
        var sep = Make("Separator", canvasRoot); Place(sep, 0f, 294f, 1280f, 1f);
        Img(sep, new Color(1f,1f,1f,0.07f));

        // =====================================================================
        //  LEFT COLUMN  320x548 at x=-420, y=-20
        // =====================================================================
        var left = Make("LeftColumn", canvasRoot); Place(left, -420f, -20f, 320f, 548f);
        Img(left, Color.clear);

        // Brand Card  320x240 at (0, 154) inside left
        var brandCard = Make("BrandCard", left); Place(brandCard, 0f, 154f, 320f, 240f);
        Img(brandCard, CARD); Border(brandCard, CARD_BORDER);

        var badgeLg = Make("LogoBadgeLg", brandCard); Place(badgeLg, 0f, 82f, 68f, 68f);
        Img(badgeLg, BLUE);
        var ltLg = MakeTMP("LogoT", badgeLg, "T", 28f, FontStyles.Bold, WHITE);
        Place(ltLg, 0f, 0f, 68f, 68f); Align(ltLg, TextAlignmentOptions.Center);

        var bnLg = MakeTMP("BrandNameLg", brandCard, "TynassIt", 22f, FontStyles.Bold, WHITE);
        Place(bnLg, 0f, 30f, 260f, 32f); Align(bnLg, TextAlignmentOptions.Center);

        var bsLg = MakeTMP("BrandSubLg", brandCard, "TRAINING PLATFORM", 10f, FontStyles.Normal, WHITE_40);
        Place(bsLg, 0f, 8f, 260f, 16f); Align(bsLg, TextAlignmentOptions.Center);
        bsLg.characterSpacing = 2.5f;

        var tagline = MakeTMP("Tagline", brandCard,
            "Immersive VR training solutions for modern\nbusinesses in Tunisia and beyond.",
            13f, FontStyles.Normal, WHITE_50);
        Place(tagline, 0f, -48f, 272f, 60f); Align(tagline, TextAlignmentOptions.Center);
        tagline.enableWordWrapping = true; tagline.lineSpacing = 3f;

        // Stats Row  320x90 at (0, 14) inside left
        var statsRow = Make("StatsRow", left); Place(statsRow, 0f, 14f, 320f, 90f);
        Img(statsRow, Color.clear);

        string[] statNums   = { "12+", "50+", "3K+" };
        string[] statLabels = { "Modules", "Companies", "Sessions" };
        float[]  statX      = { -104f, 0f, 104f };
        for (int i = 0; i < 3; i++)
        {
            var sc = Make($"StatCard_{i}", statsRow); Place(sc, statX[i], 0f, 96f, 90f);
            Img(sc, BLUE_DIM); Border(sc, BLUE_BORDER);
            var sn = MakeTMP("Num", sc, statNums[i], 24f, FontStyles.Bold, WHITE);
            Place(sn, 0f, 18f, 90f, 32f); Align(sn, TextAlignmentOptions.Center);
            var sl = MakeTMP("Lbl", sc, statLabels[i], 11f, FontStyles.Normal, WHITE_40);
            Place(sl, 0f, -14f, 90f, 18f); Align(sl, TextAlignmentOptions.Center);
        }

        // =====================================================================
        //  RIGHT COLUMN  780x548 at x=188, y=-20
        // =====================================================================
        var right = Make("RightColumn", canvasRoot); Place(right, 188f, -20f, 780f, 548f);
        Img(right, Color.clear);

        // About Card  780x128 at (0, 210) inside right
        var aboutCard = Make("AboutCard", right); Place(aboutCard, 0f, 210f, 780f, 128f);
        Img(aboutCard, CARD); Border(aboutCard, CARD_BORDER);
        SectionTitle("AboutTitle", aboutCard, "About Us", -240f, 44f, 200f);
        AboutText = MakeTMP("AboutText", aboutCard,
            "Tynass is a Tunisian technology company specializing in immersive VR training for professional environments. We build high-fidelity simulation modules that help businesses train their teams faster, safer, and more effectively.",
            14f, FontStyles.Normal, WHITE_68);
        Place(AboutText, 0f, -12f, 720f, 80f); Align(AboutText, TextAlignmentOptions.Left);
        AboutText.enableWordWrapping = true; AboutText.lineSpacing = 3f;

        // Contact Card  780x188 at (0, 46) inside right
        var contactCard = Make("ContactCard", right); Place(contactCard, 0f, 46f, 780f, 188f);
        Img(contactCard, CARD); Border(contactCard, CARD_BORDER);
        SectionTitle("ContactTitle", contactCard, "Contact Information", -240f, 74f, 260f);

        // Email row  y=28
        ContactEmail = BuildContactRow(contactCard, "✉", "EMAIL", "Contact@tynassit.com", 28f);
        // Address row  y=-18
        ContactAddress = BuildContactRow(contactCard, "📍", "ADDRESS", "Cité de la Culture MED V, BEB BHAR 1001", -20f);
        // Phone row  y=-66
        ContactPhone = BuildContactRow(contactCard, "📞", "PHONE", "+216 99 104 604", -68f);

        // Bottom Row  780x120 at (0, -100) inside right
        var bottomRow = Make("BottomRow", right); Place(bottomRow, 0f, -108f, 780f, 120f);
        Img(bottomRow, Color.clear);

        // Mission Card  380x120 at (-200, 0)
        var missionCard = Make("MissionCard", bottomRow); Place(missionCard, -200f, 0f, 380f, 120f);
        Img(missionCard, CARD); Border(missionCard, CARD_BORDER);
        SectionTitle("MissionTitle", missionCard, "Our Mission", -120f, 42f, 160f);
        var missionText = MakeTMP("MissionText", missionCard,
            "Making professional VR training accessible to every Tunisian business, one simulation at a time.",
            13f, FontStyles.Normal, WHITE_60);
        Place(missionText, 0f, -14f, 340f, 56f); Align(missionText, TextAlignmentOptions.Left);
        missionText.enableWordWrapping = true; missionText.lineSpacing = 3f;

        // Specialties Card  380x120 at (200, 0)
        var specCard = Make("SpecialtiesCard", bottomRow); Place(specCard, 200f, 0f, 380f, 120f);
        Img(specCard, CARD); Border(specCard, CARD_BORDER);
        SectionTitle("SpecTitle", specCard, "Specialties", -120f, 42f, 160f);

        string[] badges = { "VR Training", "Customs", "Safety Sim", "Meta Quest" };
        float[] bx = { -126f, -26f, -126f, -26f };
        float[] by = { -6f,   -6f,  -32f,  -32f  };
        for (int i = 0; i < badges.Length; i++)
        {
            var b = Make($"Badge_{i}", specCard); Place(b, bx[i], by[i], 96f, 24f);
            Img(b, BLUE_DIM); Border(b, BLUE_BORDER);
            var bl = MakeTMP("L", b, badges[i], 11f, FontStyles.Bold, ACCENT);
            Place(bl, 0f, 0f, 96f, 24f); Align(bl, TextAlignmentOptions.Center);
        }
    }

    // ── Section title with line ───────────────────────────────────────────────
    void SectionTitle(string name, RectTransform parent, string text, float x, float y, float w)
    {
        var t = MakeTMP(name, parent, text, 11f, FontStyles.Bold, new Color(1f,1f,1f,0.38f));
        Place(t, x, y, w, 18f); Align(t, TextAlignmentOptions.Left);
        t.characterSpacing = 1.8f;
        // Separator line after title
        var line = Make(name + "Line", parent);
        Place(line, x + w/2f + 60f, y, 400f, 1f);
        Img(line, WHITE_08);
    }

    // ── Contact row ───────────────────────────────────────────────────────────
    TMP_Text BuildContactRow(RectTransform parent, string icon, string label, string value, float y)
    {
        // Icon wrap 48x48
        var iconWrap = Make($"Icon_{label}", parent); Place(iconWrap, -320f, y, 48f, 48f);
        Img(iconWrap, BLUE_DIM); Border(iconWrap, BLUE_BORDER);
        var iconLbl = MakeTMP("Icon", iconWrap, icon, 18f, FontStyles.Normal, WHITE);
        Place(iconLbl, 0f, 0f, 48f, 48f); Align(iconLbl, TextAlignmentOptions.Center);

        // Label (small caps)
        var lbl = MakeTMP($"Label_{label}", parent, label, 10f, FontStyles.Bold, WHITE_35);
        Place(lbl, -50f, y + 10f, 200f, 16f); Align(lbl, TextAlignmentOptions.Left);
        lbl.characterSpacing = 1f;

        // Value
        var val = MakeTMP($"Value_{label}", parent, value, 14f, FontStyles.Normal, WHITE);
        Place(val, -20f, y - 8f, 560f, 22f); Align(val, TextAlignmentOptions.Left);
        return val;
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
