using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APITestSuite : MonoBehaviour
{
    [Header("Server Config")]
    [SerializeField] private string serverUrl = "https://your-deployed-server.com";
    [SerializeField] private TMP_InputField urlInput;

    [Header("Test Credentials")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    [Header("UI Output")]
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private ScrollRect scrollRect;

    [Header("Test Buttons")]
    [SerializeField] private Button testConnectionButton;
    [SerializeField] private Button adminLoginButton;
    [SerializeField] private Button companyLoginButton;
    [SerializeField] private Button getTrainingsButton;
    [SerializeField] private Button runAllTestsButton;

    private TynassApiClient apiClient;

    void Start()
    {
        apiClient = TynassApiClient.Instance;

        if (urlInput != null)
            urlInput.text = serverUrl;

        if (apiClient != null)
            apiClient.SetBaseUrl(serverUrl + "/api");

        if (testConnectionButton) testConnectionButton.onClick.AddListener(() => _ = TestConnection());
        if (adminLoginButton) adminLoginButton.onClick.AddListener(() => _ = TestAdminLogin());
        if (companyLoginButton) companyLoginButton.onClick.AddListener(() => _ = TestCompanyLogin());
        if (getTrainingsButton) getTrainingsButton.onClick.AddListener(() => _ = TestGetTrainings());
        if (runAllTestsButton) runAllTestsButton.onClick.AddListener(() => _ = RunAllTests());

        LogMessage("API Test Suite Ready");
        LogMessage("Server: " + serverUrl);
        LogMessage("Click Run All Tests or individual test buttons");
    }

    void LogMessage(string message, bool isError = false)
    {
        string prefix = isError ? "[FAIL] " : "[OK] ";
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string logEntry = string.Format("[{0}] {1}{2}", timestamp, prefix, message);

        Debug.Log(logEntry);

        if (resultText != null)
        {
            resultText.text += logEntry + "\n";
            Canvas.ForceUpdateCanvases();
            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = 0f;
        }
    }

    void ClearLog()
    {
        if (resultText != null)
            resultText.text = "";
    }

    public async Task TestConnection()
    {
        ClearLog();
        LogMessage("Testing server connection...");

        try
        {
            using (var request = UnityWebRequest.Get(serverUrl + "/api/admin/trainings"))
            {
                request.SetRequestHeader("Authorization", "Bearer test-token");
                request.timeout = 10;

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (request.responseCode == 401 || request.responseCode == 403)
                {
                    LogMessage("Server is ONLINE (auth required - expected)");
                    LogMessage("Response code: " + request.responseCode);
                }
                else if (request.result == UnityWebRequest.Result.Success)
                {
                    LogMessage("Server is ONLINE and responding");
                }
                else
                {
                    LogMessage("Server connection failed: " + request.error, true);
                    LogMessage("Response code: " + request.responseCode, true);
                }
            }
        }
        catch (Exception e)
        {
            LogMessage("Connection error: " + e.Message, true);
        }
    }

    public async Task TestAdminLogin()
    {
        ClearLog();
        LogMessage("Testing Admin Login...");

        string email = emailInput?.text ?? "admin@tynassit.com";
        string password = passwordInput?.text ?? "yourpassword";

        LogMessage("Attempting login: " + email);

        var response = await apiClient.AdminLogin(email, password);

        if (response?.token != null)
        {
            LogMessage("Admin Login SUCCESSFUL");
            LogMessage("Admin: " + response.admin.name);
            LogMessage("Role: " + response.admin.role);
            LogMessage("Token: " + response.token.Substring(0, Math.Min(20, response.token.Length)) + "...");
        }
        else
        {
            LogMessage("Admin Login FAILED", true);
            LogMessage("Check credentials and server URL", true);
        }
    }

    public async Task TestCompanyLogin()
    {
        ClearLog();
        LogMessage("Testing Company Login...");

        string email = emailInput?.text ?? "contact@douane.tn";
        string password = passwordInput?.text ?? "yourpassword";

        LogMessage("Attempting login: " + email);

        var response = await apiClient.CompanyLogin(email, password);

        if (response?.token != null)
        {
            LogMessage("Company Login SUCCESSFUL");
            LogMessage("Company: " + response.company.companyName);
            LogMessage("Trainings assigned: " + (response.trainings?.Count ?? 0));

            if (response.trainings != null)
            {
                foreach (var training in response.trainings)
                {
                    LogMessage("  Training: " + training.title + " (" + training.category + ")");
                }
            }
        }
        else
        {
            LogMessage("Company Login FAILED", true);
            LogMessage("Check credentials and server URL", true);
        }
    }

    public async Task TestGetTrainings()
    {
        ClearLog();
        LogMessage("Testing Get All Trainings...");

        string email = emailInput?.text ?? "admin@tynassit.com";
        string password = passwordInput?.text ?? "yourpassword";

        var loginResponse = await apiClient.AdminLogin(email, password);

        if (loginResponse?.token == null)
        {
            LogMessage("Must be logged in as admin first", true);
            return;
        }

        var trainings = await apiClient.GetAllTrainings();

        if (trainings != null && trainings.Count > 0)
        {
            LogMessage("Retrieved " + trainings.Count + " trainings:");
            foreach (var training in trainings)
            {
                LogMessage("  Training: " + training.title);
                LogMessage("    Category: " + training.category);
                LogMessage("    Active: " + training.isActive);
                if (!string.IsNullOrEmpty(training.description))
                {
                    string desc = training.description;
                    if (desc.Length > 50) desc = desc.Substring(0, 50) + "...";
                    LogMessage("    Description: " + desc);
                }
            }
        }
        else if (trainings != null && trainings.Count == 0)
        {
            LogMessage("No trainings found in database");
        }
        else
        {
            LogMessage("Failed to fetch trainings", true);
        }
    }

    public async Task TestGetCompanies()
    {
        LogMessage("--- Testing Get Companies ---");

        if (!apiClient.IsAuthenticated)
        {
            LogMessage("Need admin login first", true);
            return;
        }

        var companies = await apiClient.GetAllCompanies();

        if (companies != null && companies.Count > 0)
        {
            LogMessage("Retrieved " + companies.Count + " companies:");
            foreach (var company in companies)
            {
                LogMessage("  Company: " + company.companyName);
                LogMessage("    Email: " + company.email);
                LogMessage("    Active: " + company.isActive);
            }
        }
        else
        {
            LogMessage("No companies found or failed to fetch", true);
        }
    }

    public async Task TestSubmitSession()
    {
        LogMessage("--- Testing Session Submission ---");

        var loginResponse = await apiClient.CompanyLogin("contact@douane.tn", "yourpassword");

        if (loginResponse?.token == null)
        {
            LogMessage("Company login required for session test", true);
            return;
        }

        if (loginResponse.trainings == null || loginResponse.trainings.Count == 0)
        {
            LogMessage("Company has no trainings assigned", true);
            return;
        }

        string trainingId = loginResponse.trainings[0]._id;
        LogMessage("Submitting session for: " + loginResponse.trainings[0].title);

        var evaluationCriteria = new List<EvaluationCriteriaData>
        {
            new EvaluationCriteriaData
            {
                criteriaName = "Correct inspection procedure",
                passed = true,
                score = 90,
                notes = "Test submission"
            },
            new EvaluationCriteriaData
            {
                criteriaName = "Decision accuracy",
                passed = true,
                score = 85,
                notes = null
            }
        };

        var session = await apiClient.SubmitSession(
            trainingId: trainingId,
            startedAt: DateTime.UtcNow.AddMinutes(-15),
            completedAt: DateTime.UtcNow,
            score: 87,
            passed: true,
            evaluationCriteria: evaluationCriteria,
            notes: "Unity API test submission"
        );

        if (session != null)
        {
            LogMessage("Session submitted successfully");
            LogMessage("Session ID: " + session._id);
            LogMessage("Attempt #: " + session.attemptNumber);
            LogMessage("Score: " + session.score + "%");
        }
        else
        {
            LogMessage("Session submission failed", true);
        }
    }

    public async Task TestGetSessionHistory()
    {
        LogMessage("--- Testing Session History ---");

        if (!apiClient.IsAuthenticated || apiClient.UserType != "company")
        {
            var loginResponse = await apiClient.CompanyLogin("contact@douane.tn", "yourpassword");
            if (loginResponse?.token == null)
            {
                LogMessage("Company login required", true);
                return;
            }
        }

        var sessions = await apiClient.GetMySessions();

        if (sessions != null && sessions.Count > 0)
        {
            LogMessage("Retrieved " + sessions.Count + " sessions:");
            foreach (var session in sessions)
            {
                LogMessage("  Session ID: " + session._id);
                LogMessage("    Score: " + session.score + "%");
                LogMessage("    Passed: " + session.passed);
                LogMessage("    Attempt: " + session.attemptNumber);
                LogMessage("    Duration: " + session.durationSeconds + "s");
            }
        }
        else
        {
            LogMessage("No sessions found");
        }
    }

    public async Task RunAllTests()
    {
        ClearLog();
        LogMessage("RUNNING ALL API TESTS");
        LogMessage("========================");

        await TestConnection();
        await Task.Delay(1000);

        LogMessage("--- Admin Authentication ---");
        await TestAdminLogin();
        await Task.Delay(500);

        await TestGetTrainings();
        await Task.Delay(500);

        await TestGetCompanies();
        await Task.Delay(500);

        LogMessage("--- Company Authentication ---");
        await TestCompanyLogin();
        await Task.Delay(500);

        await TestSubmitSession();
        await Task.Delay(500);

        await TestGetSessionHistory();

        LogMessage("========================");
        LogMessage("ALL TESTS COMPLETED");
    }
}