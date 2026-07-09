// LoginController.cs — Attaché au Canvas de login
public class LoginController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private GameObject loadingIndicator;

    public void OnLoginButtonPressed()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        // ─── Validation locale avant appel API ────────────────
        if (string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password))
        {
            ShowError("Email et mot de passe requis.");
            return;
        }

        SetLoadingState(true);
        StartCoroutine(ApiManager.Instance.Login(
            email, password,
            onSuccess: (response) =>
            {
                SetLoadingState(false);
                // Stocker les trainings pour le menu
                TrainingMenuController.Instance
                    .LoadTrainings(response.trainings);
                // Charger la scène menu
                SceneManager.LoadScene("MenuScene");
            },
            onError: (error) =>
            {
                SetLoadingState(false);
                ShowError(error);
            }
        ));
    }

    private void ShowError(string msg)
    {
        errorText.text = msg;
        errorText.gameObject.SetActive(true);
    }

    private void SetLoadingState(bool loading)
    {
        loginButton.interactable = !loading;
        loadingIndicator.SetActive(loading);
    }
}