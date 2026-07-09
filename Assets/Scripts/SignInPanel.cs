using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Sign In Panel (Company Login)
/// Unity 6 | uGUI | Meta Quest World Space Canvas 1280x720
///
/// Uses TynassApiClient.CompanyLogin() — no raw HTTP here.
///
/// ── Hierarchy expected ──────────────────────────────────────────────────────
/// SignInPanel
/// ├── Background              (Image, color #0A1628)
/// └── Card                   (Image, color white 6% alpha, border via Outline)
///     ├── LogoBadge           (Image, color #0048FF, 72x72, radius 18)
///     │   └── LogoGrid        (Image, your logo SVG as Sprite)
///     ├── BrandName           (TMP_Text)
///     ├── BrandSub            (TMP_Text)
///     ├── TabGroup            (HorizontalLayoutGroup)
///     │   ├── TabSignIn       (Button → Image bg + TMP_Text child)
///     │   └── TabSignUp       (Button → Image bg + TMP_Text child)
///     ├── EmailField          (TMP_InputField, 60px tall)
///     ├── PasswordField       (TMP_InputField, 60px tall, ContentType=Password)
///     ├── ForgotPasswordBtn   (Button, full width)
///     ├── SignInBtn           (Button, full width, color #0048FF)
///     │   └── SignInLabel     (TMP_Text child of SignInBtn)
///     ├── DividerGroup        (HorizontalLayoutGroup with left line, text, right line)
///     ├── QuestMailBtn        (Button, full width, transparent border)
///     └── ErrorText           (TMP_Text, hidden by default, red)
/// ────────────────────────────────────────────────────────────────────────────
/// </summary>
public class SignInPanel : MonoBehaviour
{
    // ─── Inspector ────────────────────────────────────────────────────────────
    [Header("Tabs")]
    [SerializeField] Button tabSignInBtn;
    [SerializeField] Button tabSignUpBtn;
    [SerializeField] Image tabSignInBg;
    [SerializeField] Image tabSignUpBg;
    [SerializeField] TMP_Text tabSignInLabel;
    [SerializeField] TMP_Text tabSignUpLabel;

    [Header("Inputs")]
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;

    [Header("Buttons")]
    [SerializeField] Button forgotPasswordBtn;
    [SerializeField] Button signInBtn;
    [SerializeField] TMP_Text signInBtnLabel;
    [SerializeField] Button questMailBtn;

    [Header("Feedback")]
    [SerializeField] TMP_Text errorText;
    [SerializeField] GameObject loadingSpinner;

    [Header("Navigation")]
    [SerializeField] GameObject signUpPanel;
    [SerializeField] GameObject forgotPasswordPanel;
    [SerializeField] GameObject employeePickerPanel;

    // ─── Internal ─────────────────────────────────────────────────────────────
    bool _busy;

    // ─── Unity lifecycle ──────────────────────────────────────────────────────
    void Awake()
    {
        // Wire buttons
        tabSignInBtn.onClick.AddListener(OnTabSignIn);
        tabSignUpBtn.onClick.AddListener(OnTabSignUp);
        forgotPasswordBtn.onClick.AddListener(OnForgotPassword);
        signInBtn.onClick.AddListener(OnSignIn);
        questMailBtn.onClick.AddListener(OnQuestMail);

        // Password field setup
        passwordInput.contentType = TMP_InputField.ContentType.Password;
        passwordInput.ForceLabelUpdate();

        HideError();
        SetLoading(false);
        SetActiveTab(true); // SignIn active by default
    }

    void OnEnable()
    {
        emailInput.text = "";
        passwordInput.text = "";
        HideError();
        SetActiveTab(true);
    }

    // ─── Tab switching ────────────────────────────────────────────────────────
    void OnTabSignIn() => SetActiveTab(true);

    void OnTabSignUp()
    {
        if (signUpPanel != null)
        {
            gameObject.SetActive(false);
            signUpPanel.SetActive(true);
        }
        else
        {
            SetActiveTab(false); // visual only if no panel assigned
        }
    }

    void SetActiveTab(bool signInActive)
    {
        // SignIn tab
        tabSignInBg.color = signInActive ? UIColors.Blue : Color.clear;
        tabSignInLabel.color = signInActive ? UIColors.White : UIColors.WhiteDim;

        // SignUp tab
        tabSignUpBg.color = signInActive ? Color.clear : UIColors.Blue;
        tabSignUpLabel.color = signInActive ? UIColors.WhiteDim : UIColors.White;
    }

    // ─── Navigation ───────────────────────────────────────────────────────────
    void OnForgotPassword()
    {
        if (forgotPasswordPanel != null)
        {
            gameObject.SetActive(false);
            forgotPasswordPanel.SetActive(true);
        }
    }

    void OnQuestMail()
    {
        // OAuth via Quest Mail — implement when ready
        Debug.Log("[SignInPanel] Quest Mail Connect — coming soon");
    }

    // ─── Sign In ──────────────────────────────────────────────────────────────
    async void OnSignIn()
    {
        if (_busy) return;

        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // Client-side validation
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("Please enter your email and password.");
            return;
        }

        SetLoading(true);
        HideError();

        // ── Call TynassApiClient — single source of truth ──────────────────
        CompanyLoginResponse response = await TynassApiClient.Instance.CompanyLogin(email, password);

        SetLoading(false);

        if (response?.token != null)
        {
            // Store company data in AppSession for all other panels to use
            AppSession.CompanyId = response.company._id;
            AppSession.CompanyName = response.company.companyName;
            AppSession.CompanyEmail = response.company.email;
            AppSession.AssignedTrainings = response.trainings;

            Debug.Log($"[SignInPanel] Logged in as {response.company.companyName}");

            // Go to employee picker
            if (employeePickerPanel != null)
            {
                gameObject.SetActive(false);
                employeePickerPanel.SetActive(true);
            }
        }
        else
        {
            // TynassApiClient already logs the raw error — show friendly message
            if (!TynassApiClient.Instance.IsAuthenticated)
            {
                ShowError("Invalid email or password. Please try again.");
            }
            else
            {
                ShowError("Connection error. Check your network.");
            }
        }
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────
    void ShowError(string msg)
    {
        if (errorText == null) return;
        errorText.text = msg;
        errorText.gameObject.SetActive(true);
    }

    void HideError()
    {
        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }

    void SetLoading(bool loading)
    {
        _busy = loading;
        if (loadingSpinner != null) loadingSpinner.SetActive(loading);
        if (signInBtn != null) signInBtn.interactable = !loading;
        if (signInBtnLabel != null) signInBtnLabel.text = loading ? "Signing in..." : "Sign In";
    }
}