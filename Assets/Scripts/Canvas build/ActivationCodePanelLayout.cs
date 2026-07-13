using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivationCodePanelLayout : MonoBehaviour
{
    [Header("Canvas Root")]
    [SerializeField] RectTransform canvasRoot;

    [Header("Font")]
    [SerializeField] TMP_FontAsset font;

    [Header("Built refs")]
    public TMP_Text CodeText;
    public TMP_Text StatusText;
    public TMP_Text TimerText;

    static readonly Color BG          = C("0A1628");
    static readonly Color BLUE        = C("0048FF");
    static readonly Color BLUE_DIM    = new Color(0f, 0.282f, 1f, 0.12f);
    static readonly Color BLUE_BORDER = new Color(0f, 0.282f, 1f, 0.35f);
    static readonly Color WHITE       = Color.white;
    static readonly Color WHITE_65    = new Color(1f, 1f, 1f, 0.65f);
    static readonly Color WHITE_40    = new Color(1f, 1f, 1f, 0.40f);
    static readonly Color ORANGE      = C("FFB300");

    void Awake() => Build();

    void Build()
    {
        if (canvasRoot == null)
        {
            Debug.LogError("[ActivationCodePanelLayout] Canvas Root not assigned.");
            return;
        }

        // ── Background ────────────────────────────────────────────────────────
        var bg = Make("Background", canvasRoot);
        Stretch(bg);
        Img(bg, BG);

        // ── Card centrale — 500x400 ───────────────────────────────────────────
        var card = Make("Card", canvasRoot);
        Place(card, 0f, 0f, 500f, 420f);
        Img(card, BLUE_DIM);
        Border(card, BLUE_BORDER);

        // ── Icon ─────────────────────────────────────────────────────────────
        var icon = MakeTMP("Icon", card, "🔐", 48f, FontStyles.Normal, WHITE);
        Place(icon, 0f, 160f, 80f, 80f);
        Align(icon, TextAlignmentOptions.Center);

        // ── Title ─────────────────────────────────────────────────────────────
        var title = MakeTMP("Title", card, "Activation requise", 22f, FontStyles.Bold, WHITE);
        Place(title, 0f, 100f, 460f, 34f);
        Align(title, TextAlignmentOptions.Center);

        // ── Subtitle ──────────────────────────────────────────────────────────
        var sub = MakeTMP("Sub", card,
            "Ce casque n'est pas encore activé.\nMontrez ce code à votre administrateur.",
            14f, FontStyles.Normal, WHITE_65);
        Place(sub, 0f, 48f, 440f, 44f);
        Align(sub, TextAlignmentOptions.Center);
        sub.lineSpacing = 6f;

        // ── Code Box — 320x80 ─────────────────────────────────────────────────
        var codeBox = Make("CodeBox", card);
        Place(codeBox, 0f, -30f, 320f, 80f);
        Img(codeBox, new Color(0f, 0.282f, 1f, 0.20f));
        Border(codeBox, BLUE_BORDER);

        CodeText = MakeTMP("CodeText", codeBox, "------", 38f, FontStyles.Bold, WHITE);
        Place(CodeText, 0f, 0f, 320f, 80f);
        Align(CodeText, TextAlignmentOptions.Center);
        CodeText.characterSpacing = 8f;

        // ── Status Text ───────────────────────────────────────────────────────
        StatusText = MakeTMP("StatusText", card,
            "En attente de validation par l'administrateur...",
            13f, FontStyles.Normal, WHITE_40);
        Place(StatusText, 0f, -110f, 440f, 24f);
        Align(StatusText, TextAlignmentOptions.Center);

        // ── Timer Text ────────────────────────────────────────────────────────
        TimerText = MakeTMP("TimerText", card, "Expire dans 15:00", 12f, FontStyles.Normal, ORANGE);
        Place(TimerText, 0f, -145f, 440f, 20f);
        Align(TimerText, TextAlignmentOptions.Center);

        // ── Refresh hint ──────────────────────────────────────────────────────
        var hint = MakeTMP("Hint", card,
            "Vérification automatique toutes les 10 secondes",
            11f, FontStyles.Normal, WHITE_40);
        Place(hint, 0f, -178f, 440f, 18f);
        Align(hint, TextAlignmentOptions.Center);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

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

    static Color C(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }

    void Place(TMP_Text t, float x, float y, float w, float h)
    => Place(t.GetComponent<RectTransform>(), x, y, w, h);
}
