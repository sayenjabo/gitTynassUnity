using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — User Item View Layout
/// Pure UI script. Builds a single user row for the employee picker list.
/// Attach this to the prefab that gets instantiated in the ScrollView.
///
/// The EmployeePickerLayout or EmployeePickerPanel instantiates this prefab,
/// then calls Setup() with employee data.
/// </summary>

/*
Canvas
├── Background              stretch, #0A1628
├── Glow1                   600×600, blue 13%
├── Glow2                   400×400, blue 9%
├── Topbar                  1280×66, y=327
│   ├── LogoBadge           46×46, #0048FF
│   │   └── LogoT           "T"
│   ├── BrandName           "TynassIt"
│   ├── BrandSub            "TRAINING PLATFORM"
│   ├── SearchField         300×52 InputField
│   └── CompanyPill         210×44
│       ├── CompanyAvatar   32×32 circle
│       └── CompanyName
├── Separator               1280×1 line
├── SectionHeader           1176×30, y=258
│   ├── SectionTitle        "Training Modules"
│   └── SectionCount        "4 modules assigned"
├── ModulesGrid             1176×400, y=30
│   ├── ModuleCard_CustomsInspection   276×400
│   │   ├── Thumb + ThumbOverlay + Icon + Badge
│   │   ├── ModuleName / ModuleDesc
│   │   ├── MetaDot + Duration + ProgressPct
│   │   └── ProgressBg → ProgressFill (width = pct%)
│   ├── ModuleCard_ElectricalSafety
│   ├── ModuleCard_FireEmergencyResponse
│   └── ModuleCard_ConstructionSiteSafety  (CanvasGroup alpha=0.45, locked)
└── BottomNav               1280×72, y=-304
    ├── SettingsBtn          200×56, white 7%
    ├── StartSimBtn          260×56, #0048FF
    └── OverviewBtn          230×56, blue 15%
*/ 

public class UserItemViewLayout : MonoBehaviour
{
    [Header("Font — assign your TMP Font Asset")]
    [SerializeField] TMP_FontAsset font;

    [Header("Built refs — read by UserItemView or EmployeePickerPanel")]
    public Image AvatarBg;
    public TMP_Text AvatarText;
    public TMP_Text NameText;
    public TMP_Text RoleText;
    public GameObject GuestTag;
    public TMP_Text GuestTagText;
    public GameObject NoCodeTag;
    public TMP_Text NoCodeTagText;
    public TMP_Text ProgressText;
    public Image BorderImage;
    public Image BackgroundImage;
    public Button Button;

    // ─────────────────────────────────────────────────────────────────────────
    void Awake() => Build();

    void Build()
    {
        var root = GetComponent<RectTransform>();
        root.sizeDelta = new Vector2(0f, 74f); // Fixed height for each row

        // ── Background ────────────────────────────────────────────────────────
        BackgroundImage = Img(Make("Background", root), new Color(1f, 1f, 1f, 0.06f));
        Stretch(BackgroundImage.GetComponent<RectTransform>());

        // ── Border (Outline) ──────────────────────────────────────────────────
        var borderGo = Make("Border", root);
        Stretch(borderGo);
        BorderImage = Img(borderGo, Color.clear);
        Border(borderGo, new Color(1f, 1f, 1f, 0.09f));

        // ── Content Group (Horizontal Layout) ─────────────────────────────────
        var content = Make("Content", root);
        Place(content, 0f, 0f, 382f, 74f); // 420 - 16*2 padding = 388, minus margins

        // ── Avatar — 46x46 circle, positioned left ───────────────────────────
        var avatarGo = Make("AvatarBg", content);
        Place(avatarGo, -163f, 0f, 46f, 46f);
        AvatarBg = Img(avatarGo, C("0048FF"));

        AvatarText = MakeTMP("AvatarTxt", avatarGo, "AM", 15f, FontStyles.Bold, Color.white);
        Place(AvatarText, 0f, 0f, 46f, 46f);
        Align(AvatarText, TextAlignmentOptions.Center);

        // ── Info Group (Name + Role) ─────────────────────────────────────────
        var infoGroup = Make("InfoGroup", content);
        Place(infoGroup, -80f, 0f, 200f, 46f);

        NameText = MakeTMP("NameText", infoGroup, "Ahmed Maalej", 15f, FontStyles.Bold, Color.white);
        Place(NameText, 0f, 10f, 200f, 24f);
        Align(NameText, TextAlignmentOptions.Left);

        RoleText = MakeTMP("RoleText", infoGroup, "Inspector", 12f, FontStyles.Normal, new Color(1f, 1f, 1f, 0.38f));
        Place(RoleText, 0f, -12f, 200f, 20f);
        Align(RoleText, TextAlignmentOptions.Left);

        // ── Meta Group (right side) ──────────────────────────────────────────
        var metaGroup = Make("MetaGroup", content);
        Place(metaGroup, 80f, 0f, 140f, 46f);

        // Progress Text (employee only)
        ProgressText = MakeTMP("ProgressText", metaGroup, "65% overall", 12f, FontStyles.Bold, new Color(1f, 1f, 1f, 0.35f));
        Place(ProgressText, 0f, 0f, 100f, 46f);
        Align(ProgressText, TextAlignmentOptions.Right);

        // Guest Tag (guest only, hidden by default)
        GuestTag = Make("GuestTag", metaGroup).gameObject;
        var guestTagRect = GuestTag.GetComponent<RectTransform>();
        Place(guestTagRect, 0f, 0f, 72f, 26f);
        Img(guestTagRect, new Color(1f, 0.702f, 0f, 0.15f));
        Border(guestTagRect, new Color(1f, 0.702f, 0f, 0.3f));
        GuestTag.SetActive(false);

        GuestTagText = MakeTMP("GuestTagText", guestTagRect, "GUEST", 11f, FontStyles.Bold, C("FFB300"));
        Place(GuestTagText, 0f, 0f, 72f, 26f);
        Align(GuestTagText, TextAlignmentOptions.Center);

        // No Code Tag (guest only, hidden by default)
        NoCodeTag = Make("NoCodeTag", metaGroup).gameObject;
        var noCodeTagRect = NoCodeTag.GetComponent<RectTransform>();
        Place(noCodeTagRect, 0f, 0f, 72f, 26f);
        Img(noCodeTagRect, new Color(0f, 0.784f, 0.533f, 0.10f));
        Border(noCodeTagRect, new Color(0f, 0.784f, 0.533f, 0.25f));
        NoCodeTag.SetActive(false);

        NoCodeTagText = MakeTMP("NoCodeTagText", noCodeTagRect, "No code", 11f, FontStyles.Bold, C("00CC88"));
        Place(NoCodeTagText, 0f, 0f, 72f, 26f);
        Align(NoCodeTagText, TextAlignmentOptions.Center);

        // ── Button (whole row clickable) ─────────────────────────────────────
        Button = root.gameObject.AddComponent<Button>();
        Button.transition = Selectable.Transition.None;
    }

    // =========================================================================
    //  HELPERS
    // =========================================================================

    RectTransform Make(string name, RectTransform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go.GetComponent<RectTransform>();
    }

    void Place(RectTransform rt, float x, float y, float w, float h)
    {
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(x, y);
        rt.sizeDelta = new Vector2(w, h);
    }

    void Place(TMP_Text t, float x, float y, float w, float h)
        => Place(t.GetComponent<RectTransform>(), x, y, w, h);

    void Stretch(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
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
        o.effectDistance = new Vector2(1.5f, 1.5f);
    }

    TMP_Text MakeTMP(string name, RectTransform parent, string text,
        float size, FontStyles style, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.text = text;
        t.fontSize = size;
        t.fontStyle = style;
        t.color = color;
        if (font != null) t.font = font;
        return t;
    }

    void Align(TMP_Text t, TextAlignmentOptions a) => t.alignment = a;

    Color C(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }
}