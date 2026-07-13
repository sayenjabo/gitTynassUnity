using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Employee Sign In Layout
/// Pure UI script. No logic. No API. No navigation.
/// Two steps:
///   Step 1 — Enter access code (e.g. TI-001) via TMP_InputField
///   Step 2 — Enter 4-digit PIN via numpad
/// </summary>
public class EmployeeSignInLayout : MonoBehaviour
{
    [Header("Canvas Root — 1280x720 World Space Canvas")]
    [SerializeField] RectTransform canvasRoot;

    [Header("Font")]
    [SerializeField] TMP_FontAsset font;

    [Header("Built refs — auto-filled, read by EmployeeSignInPanel.cs")]
    // Step 1
    public GameObject       Step1Root;
    public TMP_InputField   AccessCodeInput;
    public Button           NextBtn;
    public TMP_Text         NextBtnLabel;
    public TMP_Text         Step1ErrorText;

    // Step 2
    public GameObject       Step2Root;
    public TMP_Text         EmployeeNameText;
    public TMP_Text         EmployeeRoleText;
    public TMP_Text         EmployeeInitialsText;
    public Image            EmployeeAvatarBg;
    public Image            Dot0, Dot1, Dot2, Dot3;
    public Button           Btn0, Btn1, Btn2, Btn3, Btn4;
    public Button           Btn5, Btn6, Btn7, Btn8, Btn9;
    public Button           BtnDelete;
    public Button           ConfirmBtn;
    public TMP_Text         ConfirmBtnLabel;
    public Button           BackBtn;
    public TMP_Text         Step2ErrorText;

    // ── Colours ───────────────────────────────────────────────────────────────
    static readonly Color BG           = C("0A1628");
    static readonly Color BLUE         = C("0048FF");
    static readonly Color BLUE_DIM     = new Color(0f, 0.282f, 1f, 0.12f);
    static readonly Color BLUE_BORDER  = new Color(0f, 0.282f, 1f, 0.35f);
    static readonly Color CARD         = new Color(1f, 1f, 1f, 0.06f);
    static readonly Color CARD_BORDER  = new Color(1f, 1f, 1f, 0.10f);
    static readonly Color INPUT_BG     = new Color(1f, 1f, 1f, 0.07f);
    static readonly Color INPUT_BORDER = new Color(1f, 1f, 1f, 0.11f);
    static readonly Color NUMPAD_BTN   = new Color(1f, 1f, 1f, 0.07f);
    static readonly Color DOT_EMPTY    = new Color(1f, 1f, 1f, 0.20f);
    static readonly Color DOT_FILLED   = C("0048FF");
    static readonly Color WHITE        = Color.white;
    static readonly Color WHITE_75     = new Color(1f, 1f, 1f, 0.75f);
    static readonly Color WHITE_55     = new Color(1f, 1f, 1f, 0.55f);
    static readonly Color WHITE_40     = new Color(1f, 1f, 1f, 0.40f);
    static readonly Color WHITE_20     = new Color(1f, 1f, 1f, 0.20f);
    static readonly Color RED          = new Color(1f, 0.4f, 0.4f, 1f);

    void Awake() => Build();

    void Build()
    {
        if (canvasRoot == null) { Debug.LogError("[EmployeeSignInLayout] Canvas Root not assigned."); return; }

        // Destroy existing children
        for (int i = canvasRoot.childCount - 1; i >= 0; i--)
            DestroyImmediate(canvasRoot.GetChild(i).gameObject);

        // ── Background ────────────────────────────────────────────────────────
        var bg = Make("Background", canvasRoot);
        Stretch(bg); Img(bg, BG);

        // ── Glow effects ──────────────────────────────────────────────────────
        var glow1 = Make("Glow1", canvasRoot);
        Place(glow1, 500f, 200f, 600f, 600f);
        Img(glow1, new Color(0f, 0.282f, 1f, 0.08f));

        var glow2 = Make("Glow2", canvasRoot);
        Place(glow2, -500f, -150f, 400f, 400f);
        Img(glow2, new Color(0f, 0.282f, 1f, 0.06f));

        // ── Top bar ───────────────────────────────────────────────────────────
        var topbar = Make("Topbar", canvasRoot);
        Place(topbar, 0f, 327f, 1280f, 66f);
        Img(topbar, new Color(1f, 1f, 1f, 0.03f));

        var logoBadge = Make("LogoBadge", topbar);
        Place(logoBadge, -596f, 0f, 46f, 46f);
        Img(logoBadge, BLUE);
        var logoT = MakeTMP("LogoT", logoBadge, "T", 20f, FontStyles.Bold, WHITE);
        Place(logoT, 0f, 0f, 46f, 46f); Align(logoT, TextAlignmentOptions.Center);

        var brandName = MakeTMP("BrandName", topbar, "TynassIt", 18f, FontStyles.Bold, WHITE);
        Place(brandName, -548f, 4f, 100f, 28f); Align(brandName, TextAlignmentOptions.Left);

        var brandSub = MakeTMP("BrandSub", topbar, "TRAINING PLATFORM", 9f, FontStyles.Normal, WHITE_40);
        Place(brandSub, -548f, -10f, 140f, 16f); Align(brandSub, TextAlignmentOptions.Left);

        // ── Separator ─────────────────────────────────────────────────────────
        var sep = Make("Separator", canvasRoot);
        Place(sep, 0f, 294f, 1280f, 1f);
        Img(sep, new Color(1f, 1f, 1f, 0.06f));

        // ── STEP 1 ROOT ───────────────────────────────────────────────────────
        var step1Go = new GameObject("Step1Root", typeof(RectTransform));
        step1Go.transform.SetParent(canvasRoot, false);
        var step1Rt = step1Go.GetComponent<RectTransform>();
        step1Rt.anchorMin = Vector2.zero; step1Rt.anchorMax = Vector2.one;
        step1Rt.offsetMin = step1Rt.offsetMax = Vector2.zero;
        Step1Root = step1Go;

        // Card
        var card1 = Make("Card", step1Rt);
        Place(card1, 0f, -20f, 480f, 340f);
        Img(card1, BLUE_DIM); Border(card1, BLUE_BORDER);

        // Icon
        var icon1 = MakeTMP("Icon", card1, "👤", 40f, FontStyles.Normal, WHITE);
        Place(icon1, 0f, 120f, 60f, 60f); Align(icon1, TextAlignmentOptions.Center);

        // Title
        var title1 = MakeTMP("Title", card1, "Who's training today?", 22f, FontStyles.Bold, WHITE);
        Place(title1, 0f, 68f, 440f, 34f); Align(title1, TextAlignmentOptions.Center);

        // Subtitle
        var sub1 = MakeTMP("Sub", card1, "Enter your access code to continue", 14f, FontStyles.Normal, WHITE_55);
        Place(sub1, 0f, 36f, 440f, 22f); Align(sub1, TextAlignmentOptions.Center);

        // Input label
        var inputLabel = MakeTMP("InputLabel", card1, "ACCESS CODE", 11f, FontStyles.Bold, WHITE_40);
        Place(inputLabel, -10f, 4f, 440f, 18f); Align(inputLabel, TextAlignmentOptions.Left);

        // Access code input
        AccessCodeInput = MakeInput(card1, "AccessCodeInput", "e.g. TI-001", -30f, 440f, 52f);

        // Next button
        var nextRect = Make("NextBtn", card1);
        Place(nextRect, 0f, -88f, 440f, 52f);
        Img(nextRect, BLUE);
        NextBtn = nextRect.gameObject.AddComponent<Button>();
        NextBtnLabel = MakeTMP("NextLabel", nextRect, "Continue →", 16f, FontStyles.Bold, WHITE);
        Place(NextBtnLabel, 0f, 0f, 440f, 52f); Align(NextBtnLabel, TextAlignmentOptions.Center);

        // Error text step 1
        Step1ErrorText = MakeTMP("ErrorText", card1, "", 13f, FontStyles.Normal, RED);
        Place(Step1ErrorText, 0f, -138f, 440f, 22f);
        Align(Step1ErrorText, TextAlignmentOptions.Center);
        Step1ErrorText.gameObject.SetActive(false);

        // ── STEP 2 ROOT ───────────────────────────────────────────────────────
        var step2Go = new GameObject("Step2Root", typeof(RectTransform));
        step2Go.transform.SetParent(canvasRoot, false);
        var step2Rt = step2Go.GetComponent<RectTransform>();
        step2Rt.anchorMin = Vector2.zero; step2Rt.anchorMax = Vector2.one;
        step2Rt.offsetMin = step2Rt.offsetMax = Vector2.zero;
        Step2Root = step2Go;
        step2Go.SetActive(false);

        // Card
        var card2 = Make("Card", step2Rt);
        Place(card2, 0f, -10f, 400f, 520f);
        Img(card2, CARD); Border(card2, CARD_BORDER);

        // Avatar
        var avatarBg = Make("AvatarBg", card2);
        Place(avatarBg, 0f, 200f, 72f, 72f);
        EmployeeAvatarBg = Img(avatarBg, BLUE);

        EmployeeInitialsText = MakeTMP("Initials", avatarBg, "AM", 24f, FontStyles.Bold, WHITE);
        Place(EmployeeInitialsText, 0f, 0f, 72f, 72f); Align(EmployeeInitialsText, TextAlignmentOptions.Center);

        // Name & Role
        EmployeeNameText = MakeTMP("Name", card2, "Ahmed Maalej", 20f, FontStyles.Bold, WHITE);
        Place(EmployeeNameText, 0f, 152f, 360f, 30f); Align(EmployeeNameText, TextAlignmentOptions.Center);

        EmployeeRoleText = MakeTMP("Role", card2, "Inspector", 14f, FontStyles.Normal, WHITE_55);
        Place(EmployeeRoleText, 0f, 124f, 360f, 22f); Align(EmployeeRoleText, TextAlignmentOptions.Center);

        // Separator line
        var sep2 = Make("Sep", card2);
        Place(sep2, 0f, 104f, 340f, 1f); Img(sep2, new Color(1f, 1f, 1f, 0.10f));

        // PIN label
        var pinLabel = MakeTMP("PinLabel", card2, "ENTER YOUR 4-DIGIT PIN", 11f, FontStyles.Bold, WHITE_40);
        Place(pinLabel, 0f, 80f, 360f, 18f); Align(pinLabel, TextAlignmentOptions.Center);

        // Dots
        var dotsRow = Make("DotsRow", card2);
        Place(dotsRow, 0f, 52f, 200f, 20f);

        Dot0 = MakeDot("Dot0", dotsRow, -75f);
        Dot1 = MakeDot("Dot1", dotsRow, -25f);
        Dot2 = MakeDot("Dot2", dotsRow, 25f);
        Dot3 = MakeDot("Dot3", dotsRow, 75f);

        // Numpad
        var numpad = Make("Numpad", card2);
        Place(numpad, 0f, -90f, 340f, 260f);

        Btn1 = MakeNumpadBtn("Btn1", numpad, "1", -110f, 96f);
        Btn2 = MakeNumpadBtn("Btn2", numpad, "2",    0f, 96f);
        Btn3 = MakeNumpadBtn("Btn3", numpad, "3",  110f, 96f);
        Btn4 = MakeNumpadBtn("Btn4", numpad, "4", -110f, 32f);
        Btn5 = MakeNumpadBtn("Btn5", numpad, "5",    0f, 32f);
        Btn6 = MakeNumpadBtn("Btn6", numpad, "6",  110f, 32f);
        Btn7 = MakeNumpadBtn("Btn7", numpad, "7", -110f, -32f);
        Btn8 = MakeNumpadBtn("Btn8", numpad, "8",    0f, -32f);
        Btn9 = MakeNumpadBtn("Btn9", numpad, "9",  110f, -32f);
        Btn0 = MakeNumpadBtn("Btn0", numpad, "0",    0f, -96f);
        BtnDelete = MakeNumpadBtn("BtnDelete", numpad, "⌫", 110f, -96f);

        // Error text step 2
        Step2ErrorText = MakeTMP("ErrorText", card2, "", 13f, FontStyles.Normal, RED);
        Place(Step2ErrorText, 0f, -230f, 360f, 22f);
        Align(Step2ErrorText, TextAlignmentOptions.Center);
        Step2ErrorText.gameObject.SetActive(false);

        // Confirm button
        var confirmRect = Make("ConfirmBtn", card2);
        Place(confirmRect, 55f, -260f, 200f, 48f);
        Img(confirmRect, BLUE);
        ConfirmBtn = confirmRect.gameObject.AddComponent<Button>();
        ConfirmBtnLabel = MakeTMP("ConfirmLabel", confirmRect, "Enter as Ahmed", 14f, FontStyles.Bold, WHITE);
        Place(ConfirmBtnLabel, 0f, 0f, 200f, 48f); Align(ConfirmBtnLabel, TextAlignmentOptions.Center);

        // Back button
        var backRect = Make("BackBtn", card2);
        Place(backRect, -95f, -260f, 110f, 48f);
        Img(backRect, CARD); Border(backRect, CARD_BORDER);
        BackBtn = backRect.gameObject.AddComponent<Button>();
        var backLabel = MakeTMP("BackLabel", backRect, "← Back", 14f, FontStyles.Bold, WHITE_55);
        Place(backLabel, 0f, 0f, 110f, 48f); Align(backLabel, TextAlignmentOptions.Center);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    RectTransform Make(string name, RectTransform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go.GetComponent<RectTransform>();
    }
    void Place(TMP_Text t, float x, float y, float w, float h)
    => Place(t.GetComponent<RectTransform>(), x, y, w, h);

    void Place(RectTransform rt, float x, float y, float w, float h)
    {
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(x, y);
        rt.sizeDelta = new Vector2(w, h);
    }

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

    TMP_InputField MakeInput(RectTransform parent, string name, string placeholder, float y, float w, float h)
    {
        var wrap = Make(name, parent);
        Place(wrap, 0f, y, w, h);
        Img(wrap, INPUT_BG); Border(wrap, INPUT_BORDER);

        var vp = Make("Viewport", wrap);
        Place(vp, 0f, 0f, w - 20f, h);

        var textComp = MakeTMP("Text", vp, "", 16f, FontStyles.Normal, WHITE);
        Place(textComp, 0f, 0f, w - 20f, h); Align(textComp, TextAlignmentOptions.MidlineLeft);

        var phComp = MakeTMP("Placeholder", vp, placeholder, 16f, FontStyles.Italic, WHITE_20);
        Place(phComp, 0f, 0f, w - 20f, h); Align(phComp, TextAlignmentOptions.MidlineLeft);

        var field = wrap.gameObject.AddComponent<TMP_InputField>();
        field.textViewport = vp;
        field.textComponent = textComp;
        field.placeholder = phComp;
        field.caretColor = BLUE;
        field.caretWidth = 2;
        field.selectionColor = new Color(0f, 0.282f, 1f, 0.35f);

        return field;
    }

    Image MakeDot(string name, RectTransform parent, float x)
    {
        var dot = Make(name, parent);
        Place(dot, x, 0f, 14f, 14f);
        return Img(dot, DOT_EMPTY);
    }

    Button MakeNumpadBtn(string name, RectTransform parent, string label, float x, float y)
    {
        var btnRect = Make(name, parent);
        Place(btnRect, x, y, 100f, 54f);
        Img(btnRect, NUMPAD_BTN); Border(btnRect, new Color(1f, 1f, 1f, 0.10f));
        var btn = btnRect.gameObject.AddComponent<Button>();
        var txt = MakeTMP("Label", btnRect, label, 20f, FontStyles.Bold, WHITE);
        Place(txt, 0f, 0f, 100f, 54f); Align(txt, TextAlignmentOptions.Center);
        return btn;
    }

    static Color C(string hex) { ColorUtility.TryParseHtmlString("#" + hex, out Color c); return c; }
}
