using UnityEngine;

/// <summary>
/// TynassIt — Central panel switcher.
/// Attach to a root empty GameObject in your scene.
/// Assign all panels in the Inspector.
///
/// Usage from any script:
///     PanelManager.Instance.Show(Panel.Modules);
///
/// Usage from a Button's OnClick in Inspector:
///     PanelManager → GoModules()
/// </summary>
public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    [Header("Auth")]
    [SerializeField] GameObject signInPanel;
    [SerializeField] GameObject signUpPanel;
    [SerializeField] GameObject forgotPasswordPanel;

    [Header("Onboarding")]
    [SerializeField] GameObject employeePickerPanel;

    [Header("Training")]
    [SerializeField] GameObject modulesPanel;
    [SerializeField] GameObject trainingDetailPanel;
    [SerializeField] GameObject quizPanel;
    [SerializeField] GameObject quizResultPanel;

    [Header("Other")]
    [SerializeField] GameObject aboutPanel;
    [SerializeField] GameObject settingsPanel;

    GameObject _current;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // TynassApiClient.Start() already tries to restore session.
        // If it restores, navigate accordingly. Otherwise go to SignIn.
        if (TynassApiClient.Instance.IsAuthenticated)
        {
            Show(Panel.EmployeePicker);
        }
        else
        {
            Show(Panel.SignIn);
        }
    }

    // ─── Panel enum ───────────────────────────────────────────────────────────
    public enum Panel
    {
        SignIn, SignUp, ForgotPassword,
        EmployeePicker,
        Modules, TrainingDetail,
        Quiz, QuizResult,
        About, Settings
    }

    // ─── Core switch ──────────────────────────────────────────────────────────
    public void Show(Panel panel)
    {
        if (_current != null) _current.SetActive(false);

        _current = panel switch
        {
            Panel.SignIn => signInPanel,
            Panel.SignUp => signUpPanel,
            Panel.ForgotPassword => forgotPasswordPanel,
            Panel.EmployeePicker => employeePickerPanel,
            Panel.Modules => modulesPanel,
            Panel.TrainingDetail => trainingDetailPanel,
            Panel.Quiz => quizPanel,
            Panel.QuizResult => quizResultPanel,
            Panel.About => aboutPanel,
            Panel.Settings => settingsPanel,
            _ => signInPanel
        };

        if (_current != null)
            _current.SetActive(true);
        else
            Debug.LogWarning($"[PanelManager] '{panel}' not assigned in Inspector.");
    }

    // ─── Inspector-friendly shortcuts (wire to Button OnClick directly) ───────
    public void GoSignIn() => Show(Panel.SignIn);
    public void GoSignUp() => Show(Panel.SignUp);
    public void GoForgotPassword() => Show(Panel.ForgotPassword);
    public void GoEmployeePicker() => Show(Panel.EmployeePicker);
    public void GoModules() => Show(Panel.Modules);
    public void GoTrainingDetail() => Show(Panel.TrainingDetail);
    public void GoQuiz() => Show(Panel.Quiz);
    public void GoQuizResult() => Show(Panel.QuizResult);
    public void GoAbout() => Show(Panel.About);
    public void GoSettings() => Show(Panel.Settings);

    public async void Logout()
    {
        await TynassApiClient.Instance.CompanyLogout();
        AppSession.Clear();
        Show(Panel.SignIn);
    }
}