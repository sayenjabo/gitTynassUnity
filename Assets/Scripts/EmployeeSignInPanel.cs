using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TynassIt — Employee Sign In Panel Logic
/// Handles:
///   Step 1 — Access code input → validate with backend
///   Step 2 — PIN numpad → confirm login
/// Requires EmployeeSignInLayout on the same GameObject.
/// </summary>
public class EmployeeSignInPanel : MonoBehaviour
{
    [Header("Next Panel")]
    [SerializeField] private GameObject modulesPanel;

    private EmployeeSignInLayout _layout;

    // State
    private string _accessCode = "";
    private string _pin = "";
    private EmployeeLoginData _employee;
    private bool _busy = false;

    void Awake()
    {
        _layout = GetComponent<EmployeeSignInLayout>();
    }

    void Start()
    {
        // Step 1 buttons
        _layout.NextBtn.onClick.AddListener(() => _ = OnNextClicked());

        // Step 2 numpad
        _layout.Btn0.onClick.AddListener(() => OnPinDigit("0"));
        _layout.Btn1.onClick.AddListener(() => OnPinDigit("1"));
        _layout.Btn2.onClick.AddListener(() => OnPinDigit("2"));
        _layout.Btn3.onClick.AddListener(() => OnPinDigit("3"));
        _layout.Btn4.onClick.AddListener(() => OnPinDigit("4"));
        _layout.Btn5.onClick.AddListener(() => OnPinDigit("5"));
        _layout.Btn6.onClick.AddListener(() => OnPinDigit("6"));
        _layout.Btn7.onClick.AddListener(() => OnPinDigit("7"));
        _layout.Btn8.onClick.AddListener(() => OnPinDigit("8"));
        _layout.Btn9.onClick.AddListener(() => OnPinDigit("9"));
        _layout.BtnDelete.onClick.AddListener(OnPinDelete);
        _layout.ConfirmBtn.onClick.AddListener(() => _ = OnConfirmClicked());
        _layout.BackBtn.onClick.AddListener(OnBackClicked);

        ShowStep1();
    }

    // ─────────────────────────────────────────
    // STEP 1 — Access Code
    // ─────────────────────────────────────────
    void ShowStep1()
    {
        _layout.Step1Root.SetActive(true);
        _layout.Step2Root.SetActive(false);
        _layout.AccessCodeInput.text = "";
        HideError1();
    }

    async Task OnNextClicked()
    {
        if (_busy) return;

        _accessCode = _layout.AccessCodeInput.text.Trim().ToUpper();

        if (string.IsNullOrEmpty(_accessCode))
        {
            ShowError1("Please enter your access code.");
            return;
        }

        // Validate access code format (XX-000)
        if (!System.Text.RegularExpressions.Regex.IsMatch(_accessCode, @"^[A-Z]{2}-\d{3}$"))
        {
            ShowError1("Invalid format. Example: TI-001");
            return;
        }

        // We don't call the API yet — just move to PIN step
        // The full login happens in Step 2 with both code + PIN
        _pin = "";
        UpdateDots();
        ShowStep2(_accessCode);
    }

    // ─────────────────────────────────────────
    // STEP 2 — PIN
    // ─────────────────────────────────────────
    void ShowStep2(string accessCode)
    {
        _layout.Step1Root.SetActive(false);
        _layout.Step2Root.SetActive(true);

        // Show access code as name placeholder until login
        _layout.EmployeeNameText.text = accessCode;
        _layout.EmployeeRoleText.text = "Enter your PIN to confirm";
        _layout.EmployeeInitialsText.text = accessCode.Substring(0, 2);
        _layout.ConfirmBtnLabel.text = "Confirm";

        HideError2();
        UpdateDots();
    }

    void OnPinDigit(string digit)
    {
        if (_pin.Length >= 4) return;
        _pin += digit;
        UpdateDots();
    }

    void OnPinDelete()
    {
        if (_pin.Length == 0) return;
        _pin = _pin.Substring(0, _pin.Length - 1);
        UpdateDots();
    }

    void UpdateDots()
    {
        Color filled = ColorFromHex("0048FF");
        Color empty  = new Color(1f, 1f, 1f, 0.20f);

        _layout.Dot0.color = _pin.Length > 0 ? filled : empty;
        _layout.Dot1.color = _pin.Length > 1 ? filled : empty;
        _layout.Dot2.color = _pin.Length > 2 ? filled : empty;
        _layout.Dot3.color = _pin.Length > 3 ? filled : empty;
    }

    async Task OnConfirmClicked()
    {
        if (_busy) return;
        if (_pin.Length < 4)
        {
            ShowError2("Please enter your 4-digit PIN.");
            return;
        }

        _busy = true;
        SetConfirmLoading(true);
        HideError2();

        string deviceToken = PlayerPrefs.GetString("device_token", null);

        if (string.IsNullOrEmpty(deviceToken))
        {
            ShowError2("Device not activated. Please restart the app.");
            _busy = false;
            SetConfirmLoading(false);
            return;
        }

        var response = await TynassApiClient.Instance.EmployeeLogin(_accessCode, _pin, deviceToken);

        _busy = false;
        SetConfirmLoading(false);

        if (response?.employee != null)
        {
            // Store employee data in AppSession
            AppSession.EmployeeId       = response.employee.id;
            AppSession.EmployeeName     = response.employee.name;
            AppSession.EmployeeRole     = response.employee.jobTitle;
            AppSession.EmployeeInitials = GetInitials(response.employee.name);

            Debug.Log($"[EmployeeSignIn] Logged in as {response.employee.name}");

            // Go to modules panel
            if (modulesPanel != null)
            {
                gameObject.SetActive(false);
                modulesPanel.SetActive(true);
            }
        }
        else
        {
            ShowError2("Invalid access code or PIN. Please try again.");
            _pin = "";
            UpdateDots();
        }
    }

    void OnBackClicked()
    {
        _pin = "";
        UpdateDots();
        ShowStep1();
    }

    // ─────────────────────────────────────────
    // HELPERS
    // ─────────────────────────────────────────
    void ShowError1(string msg)
    {
        _layout.Step1ErrorText.text = msg;
        _layout.Step1ErrorText.gameObject.SetActive(true);
    }

    void HideError1() => _layout.Step1ErrorText.gameObject.SetActive(false);

    void ShowError2(string msg)
    {
        _layout.Step2ErrorText.text = msg;
        _layout.Step2ErrorText.gameObject.SetActive(true);
    }

    void HideError2() => _layout.Step2ErrorText.gameObject.SetActive(false);

    void SetConfirmLoading(bool loading)
    {
        _layout.ConfirmBtn.interactable = !loading;
        _layout.ConfirmBtnLabel.text    = loading ? "Verifying..." : "Confirm";
    }

    string GetInitials(string name)
    {
        var parts = name.Trim().Split(' ');
        if (parts.Length >= 2)
            return $"{parts[0][0]}{parts[1][0]}".ToUpper();
        return name.Substring(0, Mathf.Min(2, name.Length)).ToUpper();
    }

    Color ColorFromHex(string hex)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out Color c);
        return c;
    }
}
