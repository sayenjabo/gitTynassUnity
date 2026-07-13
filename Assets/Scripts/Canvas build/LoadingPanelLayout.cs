// LoadingPanelLayout.cs
// Pure UI script — crée le panel de chargement au runtime
// Attacher sur un GameObject vide "LoadingPanel" dans UICanvas

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanelLayout : MonoBehaviour
{
    [Header("Canvas Root")]
    [SerializeField] RectTransform canvasRoot;

    [Header("Font")]
    [SerializeField] TMP_FontAsset font;

    [Header("Built refs")]
    public TMP_Text StatusText;
    public GameObject Spinner;

    static readonly Color BG    = C("0A1628");
    static readonly Color BLUE  = C("0048FF");
    static readonly Color WHITE = Color.white;
    static readonly Color WHITE_50 = new Color(1f, 1f, 1f, 0.50f);

    void Awake() => Build();

    void Build()
    {
        if (canvasRoot == null)
        {
            Debug.LogError("[LoadingPanelLayout] Canvas Root not assigned.");
            return;
        }

        // ── Background ────────────────────────────────────────────────────────
        var bg = Make("Background", canvasRoot);
        Stretch(bg);
        Img(bg, BG);

        // ── Logo Badge — 72x72 bleu centré en haut ───────────────────────────
        var badge = Make("LogoBadge", canvasRoot);
        Place(badge, 0f, 80f, 72f, 72f);
        Img(badge, BLUE);

        var logoT = MakeTMP("LogoT", badge, "T", 32f, FontStyles.Bold, WHITE);
        Place(logoT, 0f, 0f, 72f, 72f);
        Align(logoT, TextAlignmentOptions.Center);

        // ── Brand Name ───────────────────────────────────────────────────────
        var brand = MakeTMP("BrandName", canvasRoot, "TynassIt", 24f, FontStyles.Bold, WHITE);
        Place(brand, 0f, 20f, 300f, 36f);
        Align(brand, TextAlignmentOptions.Center);

        // ── Spinner (simple cercle animé) ─────────────────────────────────────
        var spinnerGo = Make("Spinner", canvasRoot);
        Place(spinnerGo, 0f, -40f, 48f, 48f);
        Img(spinnerGo, new Color(0f, 0.282f, 1f, 0.30f));
        Spinner = spinnerGo.gameObject;

        // Ajouter rotation animation
        spinnerGo.gameObject.AddComponent<SpinnerRotator>();

        // ── Status Text ───────────────────────────────────────────────────────
        StatusText = MakeTMP("StatusText", canvasRoot,
            "Connexion en cours...", 14f, FontStyles.Normal, WHITE_50);
        Place(StatusText, 0f, -100f, 500f, 24f);
        Align(StatusText, TextAlignmentOptions.Center);
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

// ── Spinner Rotator (composant simple) ───────────────────────────────────────
public class SpinnerRotator : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 0f, -180f * Time.deltaTime);
    }
}
