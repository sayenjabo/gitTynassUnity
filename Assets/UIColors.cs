using UnityEngine;

/// <summary>
/// TynassIt — Shared colour palette.
/// Use these in every panel script to stay consistent.
/// </summary>
public static class UIColors
{
    // ─── Backgrounds ──────────────────────────────────────────────────────────
    public static readonly Color Background = Hex("#0A1628");
    public static readonly Color Card = new Color(1f, 1f, 1f, 0.06f);
    public static readonly Color CardBorder = new Color(1f, 1f, 1f, 0.12f);
    public static readonly Color InputBg = new Color(1f, 1f, 1f, 0.07f);
    public static readonly Color InputBorder = new Color(1f, 1f, 1f, 0.11f);
    public static readonly Color InputFocus = new Color(0f, 0.282f, 1f, 0.20f);

    // ─── Brand blue ───────────────────────────────────────────────────────────
    public static readonly Color Blue = Hex("#0048FF");
    public static readonly Color BlueDim = new Color(0f, 0.282f, 1f, 0.15f);
    public static readonly Color BlueBorder = new Color(0f, 0.282f, 1f, 0.30f);
    public static readonly Color BlueGlow = new Color(0f, 0.282f, 1f, 0.20f);
    public static readonly Color Accent = Hex("#98B5FF");

    // ─── Text ─────────────────────────────────────────────────────────────────
    public static readonly Color White = Color.white;
    public static readonly Color WhiteDim = new Color(1f, 1f, 1f, 0.45f);
    public static readonly Color WhiteMid = new Color(1f, 1f, 1f, 0.65f);
    public static readonly Color WhiteSub = new Color(1f, 1f, 1f, 0.35f);
    public static readonly Color Placeholder = new Color(1f, 1f, 1f, 0.20f);
    public static readonly Color Divider = new Color(1f, 1f, 1f, 0.09f);

    // ─── Semantic ─────────────────────────────────────────────────────────────
    public static readonly Color Success = Hex("#00AA44");
    public static readonly Color SuccessDim = new Color(0f, 0.667f, 0.267f, 0.12f);
    public static readonly Color SuccessBorder = new Color(0f, 0.667f, 0.267f, 0.30f);
    public static readonly Color Error = new Color(1f, 0.40f, 0.40f, 1f);
    public static readonly Color Warning = Hex("#FFB300");
    public static readonly Color WarningDim = new Color(1f, 0.70f, 0f, 0.10f);
    public static readonly Color WarningBorder = new Color(1f, 0.70f, 0f, 0.25f);

    // ─── Guest ────────────────────────────────────────────────────────────────
    public static readonly Color GuestText = Hex("#FFB300");
    public static readonly Color GuestBg = new Color(1f, 0.706f, 0f, 0.06f);
    public static readonly Color GuestBorder = new Color(1f, 0.706f, 0f, 0.25f);

    // ─── Helper ───────────────────────────────────────────────────────────────
    public static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }

    /// Returns Blue if active, transparent if not — for tab backgrounds.
    public static Color TabBg(bool active) => active ? Blue : Color.clear;
    public static Color TabText(bool active) => active ? White : WhiteDim;
}