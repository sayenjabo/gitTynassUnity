using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Employee Picker Layout
/// Pure UI script. No logic. No API. No navigation.
/// Builds the entire Employee Picker panel from scratch at runtime.
/// Canvas: 1280x720 World Space
/// </summary>
public class EmployeePickerLayout : MonoBehaviour
{
    [Header("Canvas Root — assign your 1280x720 Canvas RectTransform")]
    [SerializeField] RectTransform canvasRoot;

    [Header("Font — assign your TMP Font Asset")]
    [SerializeField] TMP_FontAsset font;

    [Header("Built refs — auto-filled, read by EmployeePickerPanel.cs")]
    public TMP_Text CompanyNameText;
    public TMP_Text CompanyAvatarText;
    public TMP_InputField SearchInput;
    public Transform UserListContent;
    public GameObject UserItemPrefab;
    public GameObject CodeCard;
    public GameObject GuestCard;
    public Image PreviewAvatar;
    public TMP_Text PreviewAvatarInitials;
    public TMP_Text PreviewName;
    public TMP_Text PreviewRole;
    public Image Dot0;
    public Image Dot1;
    public Image Dot2;
    public Image Dot3;
    public Button Btn0;
    public Button Btn1;
    public Button Btn2;
    public Button Btn3;
    public Button Btn4;
    public Button Btn5;
    public Button Btn6;
    public Button Btn7;
    public Button Btn8;
    public Button Btn9;
    public Button BtnDelete;
    public Button BackBtn;
    public Button ConfirmBtn;
    public TMP_Text ConfirmLabel;
    public TMP_Text ErrorText;

    // ─────────────────────────────────────────────────────────────────────────
    void Awake() => Build();

    void Build()
    {
        if (canvasRoot == null)
        {
            Debug.LogError("[EmployeePickerLayout] Canvas Root not assigned.");
            return;
        }

        for (int i = canvasRoot.childCount - 1; i >= 0; i--)
            Destroy(canvasRoot.GetChild(i).gameObject);

        // ── Background 1280x720 ───────────────────────────────────────────────
        var bg = Make("Background", canvasRoot);
        Stretch(bg);
        Img(bg, C("0A1628"));

        // ── Glow 1 — top right, 700x700 ──────────────────────────────────────
        var glow1 = Make("Glow1", canvasRoot);
        Place(glow1, 360f, 240f, 700f, 700f);
        Img(glow1, new Color(0f, 0.282f, 1f, 0.13f));

        // ── Glow 2 — bottom left, 450x450 ────────────────────────────────────
        var glow2 = Make("Glow2", canvasRoot);
        Place(glow2, -380f, -180f, 450f, 450f);
        Img(glow2, new Color(0f, 0.282f, 1f, 0.09f));

        // =====================================================================
        //  TOP BAR — 22px from top, 52px from sides
        // =====================================================================
        var topBar = Make("TopBar", canvasRoot);
        // Full width minus padding: 1280 - 52*2 = 1176
        // Position: 22px from top = 360 - 22 = 338 from center (half of 720 is 360, minus 22 padding-top)
        Place(topBar, 0f, 338f, 1176f, 46f);

        // ── Logo Row (left) ──────────────────────────────────────────────────
        var logoRow = Make("LogoRow", topBar);
        Place(logoRow, -546f, 0f, 220f, 46f);

        var logoBadge = Make("LogoBadge", logoRow);
        Place(logoBadge, -87f, 0f, 46f, 46f);
        Img(logoBadge, C("0048FF"));

        var logoT = MakeTMP("LogoT", logoBadge, "T", 20f, FontStyles.Bold, Color.white);
        Place(logoT, 0f, 0f, 46f, 46f);
        Align(logoT, TextAlignmentOptions.Center);

        var brandGroup = Make("BrandGroup", logoRow);
        Place(brandGroup, 54f, 0f, 150f, 40f);

        var brandName = MakeTMP("BrandName", brandGroup, "TynassIt", 17f, FontStyles.Bold, Color.white);
        Place(brandName, 0f, 12f, 150f, 24f);
        Align(brandName, TextAlignmentOptions.Left);

        var brandSub = MakeTMP("BrandSub", brandGroup, "TRAINING PLATFORM", 10f, FontStyles.Normal, new Color(1f, 1f, 1f, 0.35f));
        Place(brandSub, 0f, -8f, 150f, 16f);
        Align(brandSub, TextAlignmentOptions.Left);
        brandSub.characterSpacing = 2f;

        // ── Company Pill (right) ─────────────────────────────────────────────
        var companyPill = Make("CompanyPill", topBar);
        Place(companyPill, 480f, 0f, 180f, 40f);
        Img(companyPill, new Color(1f, 1f, 1f, 0.07f));
        Border(companyPill, new Color(1f, 1f, 1f, 0.11f));

        var companyAvatar = Make("CompanyAvatar", companyPill);
        Place(companyAvatar, -62f, 0f, 28f, 28f);
        Img(companyAvatar, C("0048FF"));

        CompanyAvatarText = MakeTMP("CompanyAvatarTxt", companyAvatar, "AC", 11f, FontStyles.Bold, Color.white);
        Place(CompanyAvatarText, 0f, 0f, 28f, 28f);
        Align(CompanyAvatarText, TextAlignmentOptions.Center);

        CompanyNameText = MakeTMP("CompanyName", companyPill, "Acme Corp", 13f, FontStyles.Bold, new Color(1f, 1f, 1f, 0.75f));
        Place(CompanyNameText, 32f, 0f, 120f, 40f);
        Align(CompanyNameText, TextAlignmentOptions.Left);

        // =====================================================================
        //  MAIN AREA — below top bar, gap 20px
        // =====================================================================
        // Top bar bottom = 338 - 23 = 315
        // Gap = 20
        // Main area top = 315 - 20 = 295
        // Bottom nav height = 70, padding-bottom = 20
        // Main area height = 295 - (-360 + 70 + 20) = 295 - (-270) = 565
        var mainArea = Make("MainArea", canvasRoot);
        Place(mainArea, 0f, -45f, 1176f, 500f);

        // =====================================================================
        //  LEFT PANEL — 420px wide
        // =====================================================================
        var listPanel = Make("ListPanel", mainArea);
        Place(listPanel, -378f, 0f, 420f, 500f);

        // Title
        var listTitle = MakeTMP("ListTitle", listPanel, "Who's training today?", 20f, FontStyles.Bold, Color.white);
        Place(listTitle, 0f, 234f, 420f, 28f);
        Align(listTitle, TextAlignmentOptions.Left);

        // Subtitle
        var listSub = MakeTMP("ListSub", listPanel, "Select your profile from the list", 13f, FontStyles.Normal, new Color(1f, 1f, 1f, 0.38f));
        Place(listSub, 0f, 212f, 420f, 20f);
        Align(listSub, TextAlignmentOptions.Left);

        // Search
        SearchInput = MakeInputField("SearchInput", listPanel,
            0f, 168f, 420f, 48f, "Search your name...");

        // Scroll View
        var scrollView = Make("ScrollView", listPanel);
        // from 168-24=144 (search bottom) down to -250 (list bottom)
        // height = 144 - (-250) = 394, center = (144 + (-250))/2 = -53
        Place(scrollView, 0f, -53f, 420f, 394f);

        var viewport = Make("Viewport", scrollView);
        Stretch(viewport);
        viewport.gameObject.AddComponent<RectMask2D>();
        Img(viewport, Color.clear);

        UserListContent = Make("Content", viewport).transform;
        ((RectTransform)UserListContent).anchorMin = new Vector2(0f, 1f);
        ((RectTransform)UserListContent).anchorMax = new Vector2(1f, 1f);
        ((RectTransform)UserListContent).pivot = new Vector2(0.5f, 1f);
        ((RectTransform)UserListContent).anchoredPosition = Vector2.zero;
        ((RectTransform)UserListContent).sizeDelta = new Vector2(0f, 0f);

        // =====================================================================
        //  RIGHT PANEL — centered content
        // =====================================================================
        var codePanel = Make("CodePanel", mainArea);
        Place(codePanel, 378f, 0f, 500f, 500f);

        // ── Code Card ────────────────────────────────────────────────────────
        CodeCard = Make("CodeCard", codePanel).gameObject;
        var codeCardRect = CodeCard.GetComponent<RectTransform>();
        Place(codeCardRect, 0f, 0f, 380f, 410f);
        Img(codeCardRect, new Color(1f, 1f, 1f, 0.06f));
        Border(codeCardRect, new Color(1f, 1f, 1f, 0.10f));

        // Preview Avatar — top area
        PreviewAvatar = Img(Make("PreviewAvatar", codeCardRect), C("0048FF"));
        Place(PreviewAvatar.GetComponent<RectTransform>(), 0f, 167f, 72f, 72f);

        PreviewAvatarInitials = MakeTMP("PreviewAvatarTxt", PreviewAvatar.GetComponent<RectTransform>(), "AM", 24f, FontStyles.Bold, Color.white);
        Place(PreviewAvatarInitials, 0f, 0f, 72f, 72f);
        Align(PreviewAvatarInitials, TextAlignmentOptions.Center);

        PreviewName = MakeTMP("PreviewName", codeCardRect, "Ahmed Maalej", 18f, FontStyles.Bold, Color.white);
        Place(PreviewName, 0f, 123f, 340f, 26f);
        Align(PreviewName, TextAlignmentOptions.Center);

        PreviewRole = MakeTMP("PreviewRole", codeCardRect, "Inspector", 13f, FontStyles.Normal, new Color(1f, 1f, 1f, 0.40f));
        Place(PreviewRole, 0f, 105f, 340f, 20f);
        Align(PreviewRole, TextAlignmentOptions.Center);

        // Divider
        var codeDivider = Make("CodeDivider", codeCardRect);
        Place(codeDivider, 0f, 72f, 300f, 1f);
        Img(codeDivider, new Color(1f, 1f, 1f, 0.08f));

        // Code Label
        var codeLabel = MakeTMP("CodeLabel", codeCardRect, "ENTER YOUR 4-DIGIT ACCESS CODE", 11f, FontStyles.Bold, new Color(1f, 1f, 1f, 0.45f));
        Place(codeLabel, 0f, 48f, 340f, 18f);
        Align(codeLabel, TextAlignmentOptions.Center);
        codeLabel.characterSpacing = 1.5f;

        // Code Dots — 4 dots, gap 14, total ~114px
        var dotsContainer = Make("CodeDots", codeCardRect);
        Place(dotsContainer, 0f, 14f, 120f, 18f);

        Dot0 = Img(Make("Dot0", dotsContainer), new Color(1f, 1f, 1f, 0.2f));
        Place(Dot0.GetComponent<RectTransform>(), -45f, 0f, 18f, 18f);

        Dot1 = Img(Make("Dot1", dotsContainer), new Color(1f, 1f, 1f, 0.2f));
        Place(Dot1.GetComponent<RectTransform>(), -15f, 0f, 18f, 18f);

        Dot2 = Img(Make("Dot2", dotsContainer), new Color(1f, 1f, 1f, 0.2f));
        Place(Dot2.GetComponent<RectTransform>(), 15f, 0f, 18f, 18f);

        Dot3 = Img(Make("Dot3", dotsContainer), new Color(1f, 1f, 1f, 0.2f));
        Place(Dot3.GetComponent<RectTransform>(), 45f, 0f, 18f, 18f);

        // Numpad — 3x4 grid, 100x58 buttons, gap 10
        // Total: 3*100 + 2*10 = 320 wide, 4*58 + 3*10 = 262 tall
        var numpadGrid = Make("NumpadGrid", codeCardRect);
        Place(numpadGrid, 0f, -118f, 320f, 262f);

        // Row 1: 1 2 3
        Btn1 = MakeNumpadBtn("Btn1", numpadGrid, "1", -110f, 102f);
        Btn2 = MakeNumpadBtn("Btn2", numpadGrid, "2", 0f, 102f);
        Btn3 = MakeNumpadBtn("Btn3", numpadGrid, "3", 110f, 102f);

        // Row 2: 4 5 6
        Btn4 = MakeNumpadBtn("Btn4", numpadGrid, "4", -110f, 34f);
        Btn5 = MakeNumpadBtn("Btn5", numpadGrid, "5", 0f, 34f);
        Btn6 = MakeNumpadBtn("Btn6", numpadGrid, "6", 110f, 34f);

        // Row 3: 7 8 9
        Btn7 = MakeNumpadBtn("Btn7", numpadGrid, "7", -110f, -34f);
        Btn8 = MakeNumpadBtn("Btn8", numpadGrid, "8", 0f, -34f);
        Btn9 = MakeNumpadBtn("Btn9", numpadGrid, "9", 110f, -34f);

        // Row 4: (empty) 0 del
        Btn0 = MakeNumpadBtn("Btn0", numpadGrid, "0", 0f, -102f);
        BtnDelete = MakeNumpadBtn("BtnDelete", numpadGrid, "⌫", 110f, -102f);
        var delLabel = BtnDelete.GetComponentInChildren<TMP_Text>();
        if (delLabel != null)
        {
            delLabel.fontSize = 16f;
            delLabel.color = new Color(1f, 1f, 1f, 0.5f);
        }

        // ── Guest Card (hidden) ──────────────────────────────────────────────
        GuestCard = Make("GuestCard", codePanel).gameObject;
        var guestCardRect = GuestCard.GetComponent<RectTransform>();
        Place(guestCardRect, 0f, 0f, 380f, 340f);
        Img(guestCardRect, new Color(1f, 0.702f, 0f, 0.06f));
        Border(guestCardRect, new Color(1f, 0.702f, 0f, 0.20f));
        GuestCard.SetActive(false);

        var guestIcon = MakeTMP("GuestIcon", guestCardRect, "👤", 56f, FontStyles.Normal, Color.white);
        Place(guestIcon, 0f, 130f, 80f, 80f);
        Align(guestIcon, TextAlignmentOptions.Center);

        var guestTitle = MakeTMP("GuestTitle", guestCardRect, "Guest Mode", 20f, FontStyles.Bold, C("FFB300"));
        Place(guestTitle, 0f, 82f, 340f, 30f);
        Align(guestTitle, TextAlignmentOptions.Center);

        var guestDesc = MakeTMP("GuestDesc", guestCardRect,
            "You can explore all training modules freely without an account. Your progress will not be saved to the database.",
            14f, FontStyles.Normal, new Color(1f, 1f, 1f, 0.5f));
        Place(guestDesc, 0f, 22f, 340f, 70f);
        Align(guestDesc, TextAlignmentOptions.Center);

        var guestWarning = Make("GuestWarning", guestCardRect);
        Place(guestWarning, 0f, -70f, 340f, 74f);
        Img(guestWarning, new Color(1f, 0.702f, 0f, 0.08f));
        Border(guestWarning, new Color(1f, 0.702f, 0f, 0.2f));

        var warnIcon = MakeTMP("WarnIcon", guestWarning, "⚠️", 16f, FontStyles.Normal, new Color(1f, 0.702f, 0f, 0.8f));
        Place(warnIcon, -148f, 16f, 24f, 24f);
        Align(warnIcon, TextAlignmentOptions.Center);

        var warnText = MakeTMP("WarnText", guestWarning,
            "Progress, scores, and session data will not be recorded. This mode is for demonstration and exploration only.",
            12f, FontStyles.Normal, new Color(1f, 0.702f, 0f, 0.8f));
        Place(warnText, 20f, 0f, 300f, 74f);
        Align(warnText, TextAlignmentOptions.Left);

        // =====================================================================
        //  ERROR TEXT
        // =====================================================================
        ErrorText = MakeTMP("ErrorText", canvasRoot, "", 13f, FontStyles.Normal, new Color(1f, 0.4f, 0.4f, 1f));
        Place(ErrorText, 0f, -288f, 600f, 24f);
        Align(ErrorText, TextAlignmentOptions.Center);
        ErrorText.gameObject.SetActive(false);

        // =====================================================================
        //  BOTTOM NAV — 20px from bottom, centered
        // =====================================================================
        // Bottom of canvas = -360, padding 20 = -340
        // Buttons height = 52
        // Bottom nav center Y = -340 + 26 = -314
        var bottomNav = Make("BottomNav", canvasRoot);
        Place(bottomNav, 0f, -314f, 1176f, 52f);

        // Back Button — left
        var backRect = Make("BackBtn", bottomNav);
        Place(backRect, -160f, 0f, 210f, 52f);
        Img(backRect, new Color(1f, 1f, 1f, 0.07f));
        Border(backRect, new Color(1f, 1f, 1f, 0.12f));
        BackBtn = backRect.gameObject.AddComponent<Button>();

        var backLabel = MakeTMP("BackLabel", backRect, "← Back to Login", 15f, FontStyles.Bold, new Color(1f, 1f, 1f, 0.55f));
        Place(backLabel, 0f, 0f, 210f, 52f);
        Align(backLabel, TextAlignmentOptions.Center);

        // Confirm Button — right
        var confirmRect = Make("ConfirmBtn", bottomNav);
        Place(confirmRect, 120f, 0f, 280f, 52f);
        Img(confirmRect, C("0048FF"));
        ConfirmBtn = confirmRect.gameObject.AddComponent<Button>();

        ConfirmLabel = MakeTMP("ConfirmLabel", confirmRect, "Enter as Ahmed", 16f, FontStyles.Bold, Color.white);
        Place(ConfirmLabel, 0f, 0f, 280f, 52f);
        Align(ConfirmLabel, TextAlignmentOptions.Center);
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

    TMP_InputField MakeInputField(string name, RectTransform parent,
        float x, float y, float w, float h, string placeholder)
    {
        var wrap = Make(name, parent);
        Place(wrap, x, y, w, h);
        Img(wrap, new Color(1f, 1f, 1f, 0.07f));
        Border(wrap, new Color(1f, 1f, 1f, 0.11f));

        var vp = Make("Viewport", wrap);
        vp.anchorMin = Vector2.zero;
        vp.anchorMax = Vector2.one;
        vp.offsetMin = new Vector2(18f, 0f);
        vp.offsetMax = new Vector2(-18f, 0f);
        vp.gameObject.AddComponent<RectMask2D>();

        var textGo = Make("Text", vp);
        Stretch(textGo);
        var textComp = textGo.gameObject.AddComponent<TextMeshProUGUI>();
        textComp.color = Color.white;
        textComp.fontSize = 14f;
        textComp.alignment = TextAlignmentOptions.MidlineLeft;
        if (font != null) textComp.font = font;

        var phGo = Make("Placeholder", vp);
        Stretch(phGo);
        var phComp = phGo.gameObject.AddComponent<TextMeshProUGUI>();
        phComp.text = placeholder;
        phComp.color = new Color(1f, 1f, 1f, 0.20f);
        phComp.fontSize = 14f;
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

        return field;
    }

    Button MakeNumpadBtn(string name, RectTransform parent, string label, float x, float y)
    {
        var btnRect = Make(name, parent);
        Place(btnRect, x, y, 100f, 58f);
        Img(btnRect, new Color(1f, 1f, 1f, 0.07f));
        Border(btnRect, new Color(1f, 1f, 1f, 0.10f));
        var btn = btnRect.gameObject.AddComponent<Button>();

        var txt = MakeTMP("Label", btnRect, label, 20f, FontStyles.Bold, Color.white);
        Place(txt, 0f, 0f, 100f, 58f);
        Align(txt, TextAlignmentOptions.Center);

        return btn;
    }

    Color C(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }
}