using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Sign In Layout
/// Pure UI script. No logic. No API. No navigation.
/// Builds the entire Sign In panel from scratch at runtime.
///
/// HOW TO USE:
///   1. Create an empty GameObject inside your World Space Canvas
///   2. Attach this script to it
///   3. Assign your Canvas RectTransform in "Canvas Root"
///   4. Assign your TMP Font Asset
///   5. Press Play — everything is built automatically
///
/// The script destroys all existing children of Canvas Root first,
/// then rebuilds everything clean.
/// </summary>
public class SignInLayout : MonoBehaviour
{
    [Header("Canvas Root — assign your 1280x720 Canvas RectTransform")]
    [SerializeField] RectTransform canvasRoot;

    [Header("Font — assign your TMP Font Asset")]
    [SerializeField] TMP_FontAsset font;

    [Header("Built refs — auto-filled, read by SignInPanel.cs")]
    public Button TabSignInBtn;
    public Button TabSignUpBtn;
    public Image TabSignInBg;
    public Image TabSignUpBg;
    public TMP_Text TabSignInLabel;
    public TMP_Text TabSignUpLabel;
    public TMP_InputField EmailInput;
    public TMP_InputField PasswordInput;
    public Button ForgotPasswordBtn;
    public Button SignInBtn;
    public TMP_Text SignInBtnLabel;
    public Button QuestMailBtn;
    public TMP_Text ErrorText;

    // ─────────────────────────────────────────────────────────────────────────
    void Awake() => Build();

    void Build()
    {
        if (canvasRoot == null)
        {
            Debug.LogError("[SignInLayout] Canvas Root not assigned. Drag your Canvas into the Canvas Root field.");
            return;
        }

        // Wipe all existing children so we start completely fresh
        for (int i = canvasRoot.childCount - 1; i >= 0; i--)
            Destroy(canvasRoot.GetChild(i).gameObject);

        // ── Background ────────────────────────────────────────────────────────
        // Stretches full 1280x720 | color #0A1628
        var bg = Make("Background", canvasRoot);
        Stretch(bg);
        Img(bg, C("0A1628"));

        // ── Glow 1 — top right ────────────────────────────────────────────────
        // 700x700 soft blue blob | position (320, 160)
        var glow1 = Make("Glow1", canvasRoot);
        Place(glow1, 320f, 160f, 700f, 700f);
        Img(glow1, new Color(0f, 0.282f, 1f, 0.13f));

        // ── Glow 2 — bottom left ──────────────────────────────────────────────
        // 500x500 soft blue blob | position (-350, -220)
        var glow2 = Make("Glow2", canvasRoot);
        Place(glow2, -350f, -220f, 500f, 500f);
        Img(glow2, new Color(0f, 0.282f, 1f, 0.09f));

        // ── Card ──────────────────────────────────────────────────────────────
        // 480x600 centered | white 6% alpha bg | white 12% border
        var card = Make("Card", canvasRoot);
        Place(card, 0f, 0f, 480f, 600f);
        Img(card, new Color(1f, 1f, 1f, 0.06f));
        Border(card, new Color(1f, 1f, 1f, 0.12f));

        // ── Logo Badge ────────────────────────────────────────────────────────
        // 72x72 | color #0048FF | position (0, 248) from card center
        var badge = Make("LogoBadge", card);
        Place(badge, 0f, 248f, 72f, 72f);
        Img(badge, C("0048FF"));

        var logoT = MakeTMP("LogoT", badge, "T", 28f, FontStyles.Bold, Color.white);
        Place(logoT, 0f, 0f, 72f, 72f);
        Align(logoT, TextAlignmentOptions.Center);

        // ── Brand Name ────────────────────────────────────────────────────────
        // "TynassIt" | 26px Bold | white | (0, 196)
        var brandName = MakeTMP("BrandName", card, "TynassIt", 26f, FontStyles.Bold, Color.white);
        Place(brandName, 0f, 196f, 340f, 38f);
        Align(brandName, TextAlignmentOptions.Center);

        // ── Brand Subtitle ────────────────────────────────────────────────────
        // "TRAINING PLATFORM" | 11px | white 40% | (0, 172)
        var brandSub = MakeTMP("BrandSub", card, "TRAINING PLATFORM", 11f, FontStyles.Normal, new Color(1f, 1f, 1f, 0.40f));
        Place(brandSub, 0f, 172f, 340f, 20f);
        Align(brandSub, TextAlignmentOptions.Center);
        brandSub.characterSpacing = 2.5f;

        // ── Tab Group ─────────────────────────────────────────────────────────
        // 400x52 | white 6% alpha pill | (0, 128)
        var tabGroup = Make("TabGroup", card);
        Place(tabGroup, 0f, 128f, 400f, 52f);
        Img(tabGroup, new Color(1f, 1f, 1f, 0.06f));

        // Tab Sign In — left | 192x44 | #0048FF active | (-100, 0) inside TabGroup
        var tabInRect = Make("Tab_SignIn", tabGroup);
        Place(tabInRect, -100f, 0f, 192f, 44f);
        TabSignInBg = Img(tabInRect, C("0048FF"));
        TabSignInBtn = tabInRect.gameObject.AddComponent<Button>();
        TabSignInLabel = MakeTMP("Label", tabInRect, "Sign In", 16f, FontStyles.Bold, Color.white);
        Place(TabSignInLabel, 0f, 0f, 192f, 44f);
        Align(TabSignInLabel, TextAlignmentOptions.Center);

        // Tab Sign Up — right | 192x44 | transparent inactive | (100, 0) inside TabGroup
        var tabUpRect = Make("Tab_SignUp", tabGroup);
        Place(tabUpRect, 100f, 0f, 192f, 44f);
        TabSignUpBg = Img(tabUpRect, Color.clear);
        TabSignUpBtn = tabUpRect.gameObject.AddComponent<Button>();
        TabSignUpLabel = MakeTMP("Label", tabUpRect, "Sign Up", 16f, FontStyles.Bold, new Color(1f, 1f, 1f, 0.45f));
        Place(TabSignUpLabel, 0f, 0f, 192f, 44f);
        Align(TabSignUpLabel, TextAlignmentOptions.Center);

        // ── Email Field ───────────────────────────────────────────────────────
        // 400x60 | white 7% bg | white 11% border | (0, 56)
        EmailInput = MakeInputField("EmailField", card,
            0f, 56f, 400f, 60f, "contact@company.com", false);

        // ── Password Field ────────────────────────────────────────────────────
        // 400x60 | white 7% bg | white 11% border | (0, -16) | Password type
        PasswordInput = MakeInputField("PasswordField", card,
            0f, -16f, 400f, 60f, "••••••••", true);

        // ── Forgot Password Button ────────────────────────────────────────────
        // 400x48 | transparent bg | white 11% border | (0, -88) | text #98B5FF
        var forgotRect = Make("ForgotPasswordBtn", card);
        Place(forgotRect, 0f, -88f, 400f, 48f);
        Img(forgotRect, Color.clear);
        Border(forgotRect, new Color(1f, 1f, 1f, 0.11f));
        ForgotPasswordBtn = forgotRect.gameObject.AddComponent<Button>();
        var forgotLabel = MakeTMP("Label", forgotRect, "Forgot password?", 14f, FontStyles.Normal, C("98B5FF"));
        Place(forgotLabel, 0f, 0f, 400f, 48f);
        Align(forgotLabel, TextAlignmentOptions.Center);

        // ── Sign In Button ────────────────────────────────────────────────────
        // 400x58 | #0048FF bg | white text 17px Bold | (0, -156)
        var signInRect = Make("SignInBtn", card);
        Place(signInRect, 0f, -156f, 400f, 58f);
        Img(signInRect, C("0048FF"));
        SignInBtn = signInRect.gameObject.AddComponent<Button>();
        SignInBtnLabel = MakeTMP("Label", signInRect, "Sign In", 17f, FontStyles.Bold, Color.white);
        Place(SignInBtnLabel, 0f, 0f, 400f, 58f);
        Align(SignInBtnLabel, TextAlignmentOptions.Center);

        // ── Divider ───────────────────────────────────────────────────────────
        // Left line 130x1 at (-135, -224) | text at (0, -224) | right line at (135, -224)
        var divLeft = Make("DividerLeft", card);
        Place(divLeft, -135f, -224f, 130f, 1f);
        Img(divLeft, new Color(1f, 1f, 1f, 0.09f));

        var divText = MakeTMP("DividerText", card, "or connect via", 11f, FontStyles.Normal, new Color(1f, 1f, 1f, 0.30f));
        Place(divText, 0f, -224f, 120f, 18f);
        Align(divText, TextAlignmentOptions.Center);

        var divRight = Make("DividerRight", card);
        Place(divRight, 135f, -224f, 130f, 1f);
        Img(divRight, new Color(1f, 1f, 1f, 0.09f));

        // ── Quest Mail Connect Button ─────────────────────────────────────────
        // 400x56 | transparent bg | white 15% border | (0, -260)
        var questRect = Make("QuestMailBtn", card);
        Place(questRect, 0f, -260f, 400f, 56f);
        Img(questRect, Color.clear);
        Border(questRect, new Color(1f, 1f, 1f, 0.15f));
        QuestMailBtn = questRect.gameObject.AddComponent<Button>();

        // Blue dot — 10x10 at (-90, 0) inside quest button
        var dot = Make("BlueDot", questRect);
        Place(dot, -90f, 0f, 10f, 10f);
        Img(dot, C("0048FF"));

        // Quest label — starts at (10, 0)
        var questLabel = MakeTMP("Label", questRect, "Quest Mail Connect", 15f, FontStyles.Bold, new Color(1f, 1f, 1f, 0.70f));
        Place(questLabel, 10f, 0f, 280f, 56f);
        Align(questLabel, TextAlignmentOptions.Left);

        // ── Error Text ────────────────────────────────────────────────────────
        // 400x24 | red text | (0, -304) | hidden by default
        ErrorText = MakeTMP("ErrorText", card, "", 13f, FontStyles.Normal, new Color(1f, 0.4f, 0.4f, 1f));
        Place(ErrorText, 0f, -304f, 400f, 24f);
        Align(ErrorText, TextAlignmentOptions.Center);
        ErrorText.gameObject.SetActive(false);
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

    // Outline component simulates a border
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

    TMP_InputField MakeInputField(string name, RectTransform parent,
        float x, float y, float w, float h, string placeholder, bool isPassword)
    {
        var wrap = Make(name, parent);
        Place(wrap, x, y, w, h);
        Img(wrap, new Color(1f, 1f, 1f, 0.07f));
        Border(wrap, new Color(1f, 1f, 1f, 0.11f));

        // Viewport with left/right padding
        var vp = Make("Viewport", wrap);
        vp.anchorMin = Vector2.zero;
        vp.anchorMax = Vector2.one;
        vp.offsetMin = new Vector2(18f, 0f);
        vp.offsetMax = new Vector2(-18f, 0f);
        vp.gameObject.AddComponent<RectMask2D>();

        // Text
        var textGo = Make("Text", vp);
        Stretch(textGo);
        var textComp = textGo.gameObject.AddComponent<TextMeshProUGUI>();
        textComp.color = Color.white;
        textComp.fontSize = 16f;
        textComp.alignment = TextAlignmentOptions.MidlineLeft;
        if (font != null) textComp.font = font;

        // Placeholder
        var phGo = Make("Placeholder", vp);
        Stretch(phGo);
        var phComp = phGo.gameObject.AddComponent<TextMeshProUGUI>();
        phComp.text = placeholder;
        phComp.color = new Color(1f, 1f, 1f, 0.20f);
        phComp.fontSize = 16f;
        phComp.fontStyle = FontStyles.Italic;
        phComp.alignment = TextAlignmentOptions.MidlineLeft;
        if (font != null) phComp.font = font;

        var field = wrap.gameObject.AddComponent<TMP_InputField>();
        field.textViewport = vp;
        field.textComponent = textComp;
        field.placeholder = phComp;
        field.caretColor = C("0048FF");
        field.caretWidth = 2;
        field.selectionColor = new Color(0f, 0.282f, 1f, 0.35f);

        if (isPassword)
        {
            field.contentType = TMP_InputField.ContentType.Password;
            field.ForceLabelUpdate();
        }

        return field;
    }

    Color C(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }
}