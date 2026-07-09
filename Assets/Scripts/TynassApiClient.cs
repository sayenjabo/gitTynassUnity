using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
public class AdminData
{
    public string _id;
    public string name;
    public string email;
    public string role;
    public bool isActive;
    public string createdAt;
    public string updatedAt;
    public int __v;
}

[Serializable]
public class CompanyData
{
    public string _id;
    public string companyName;
    public string email;
    public List<string> assignedTrainings;
    public bool isActive;
    public string createdAt;
    public string updatedAt;
    public int __v;
}

[Serializable]
public class TrainingData
{
    public string _id;
    public string title;
    public string description;
    public string category;
    public string thumbnailUrl;
    public bool isActive;
    public string createdAt;
    public string updatedAt;
    public int __v;
}

[Serializable]
public class EvaluationCriteriaData
{
    public string criteriaName;
    public bool passed;
    public int score;
    public string notes;
}

[Serializable]
public class SessionData
{
    public string _id;
    public string company;
    public string training;
    public string startedAt;
    public string completedAt;
    public int durationSeconds;
    public int score;
    public bool passed;
    public int attemptNumber;
    public List<EvaluationCriteriaData> evaluationCriteria;
    public string notes;
    public string createdAt;
    public string updatedAt;
    public int __v;
    public CompanyPopulated companyData;
    public TrainingPopulated trainingData;
}

[Serializable]
public class CompanyPopulated
{
    public string _id;
    public string companyName;
    public string email;
}

[Serializable]
public class TrainingPopulated
{
    public string _id;
    public string title;
    public string category;
}

[Serializable]
public class TrainingStats
{
    public string trainingId;
    public string title;
    public string category;
    public int totalSessions;
    public float avgScore;
    public int avgDurationSeconds;
    public int passCount;
    public int failCount;
    public float passRate;
}

[Serializable]
public class CompanyStats
{
    public string companyId;
    public string companyName;
    public string email;
    public int totalSessions;
    public float avgScore;
    public int avgDurationSeconds;
    public int passCount;
    public int failCount;
    public float passRate;
    public string lastActivity;
    public int uniqueTrainingsUsed;
}

[Serializable]
public class CompanyStatsDetail
{
    public CompanyBasicInfo company;
    public List<TrainingStats> breakdown;
}

[Serializable]
public class CompanyBasicInfo
{
    public string _id;
    public string companyName;
    public string email;
}

[Serializable]
public class AdminLoginData
{
    public string _id;
    public string name;
    public string email;
    public string role;
}

[Serializable]
public class CompanyLoginData
{
    public string _id;
    public string companyName;
    public string email;
}

[Serializable]
public class AdminLoginResponse
{
    public string message;
    public string token;
    public AdminLoginData admin;
}

[Serializable]
public class CompanyLoginResponse
{
    public string message;
    public string token;
    public CompanyLoginData company;
    public List<TrainingData> trainings;
}

[Serializable]
public class LoginPayload
{
    public string email;
    public string password;
}

[Serializable]
public class ApiMessage
{
    public string message;
}

[Serializable]
public class CreateCompanyResponse
{
    public string message;
    public CreatedCompanyData company;
}

[Serializable]
public class CreatedCompanyData
{
    public string id;
    public string companyName;
    public string email;
}

[Serializable]
public class AssignTrainingsResponse
{
    public string message;
    public List<TrainingData> assignedTrainings;
}

[Serializable]
internal class TrainingListWrapper { public List<TrainingData> trainings; }

[Serializable]
internal class TrainingSingleWrapper { public TrainingData training; }

[Serializable]
internal class TrainingCreateWrapper { public string message; public TrainingData training; }

[Serializable]
internal class CompanyListWrapper { public List<CompanyData> companies; }

[Serializable]
internal class CompanySingleWrapper { public CompanyData company; }

[Serializable]
internal class SessionListWrapper { public List<SessionData> sessions; }

[Serializable]
internal class SessionCreateWrapper { public string message; public SessionData session; }

[Serializable]
internal class TrainingStatsWrapper { public List<TrainingStats> stats; }

[Serializable]
internal class CompanyStatsWrapper { public List<CompanyStats> stats; }

public class TynassApiClient : MonoBehaviour
{
    public static TynassApiClient Instance { get; private set; }

    [SerializeField] private string baseUrl = "http://localhost:3000/api";

    private string authToken;
    private string userType;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStoredToken(); // ADD THIS
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        bool hasSession = await TynassApiClient.Instance.TryRestoreSession();

        if (hasSession)
        {
            string userType = TynassApiClient.Instance.UserType;
            // Skip login screen, go straight to the right scene
            if (userType == "admin")
                SceneManager.LoadScene("AdminDashboard");
            else
               SceneManager.LoadScene("Companys");
        }
    }

    private void LoadStoredToken()
    {
        string storedToken = PlayerPrefs.GetString("auth_token", null);
        string storedType = PlayerPrefs.GetString("user_type", null);

        if (!string.IsNullOrEmpty(storedToken))
        {
            authToken = storedToken;
            userType = storedType;
            Debug.Log($"[Auth] Restored session as {storedType}");
        }
    }

    public async Task<bool> TryRestoreSession()
    {
        if (!IsAuthenticated) return false;

        try
        {
            if (userType == "admin")
            {
                var me = await AdminGetMe();
                if (me?.admin == null) { ClearToken(); return false; }
            }
            else if (userType == "company")
            {
                var me = await CompanyGetMe();
                if (me?.company == null) { ClearToken(); return false; }
            }

            Debug.Log($"[Auth] Session valid for {userType}");
            return true;
        }
        catch
        {
            ClearToken();
            return false;
        }
    }

    public void SetToken(string token, string type)
    {
        authToken = token;
        userType = type;

        PlayerPrefs.SetString("auth_token", token);
        PlayerPrefs.SetString("user_type", type);
        PlayerPrefs.Save();
    }

    public void ClearToken()
    {
        authToken = null;
        userType = null;

        PlayerPrefs.DeleteKey("auth_token");
        PlayerPrefs.DeleteKey("user_type");
        PlayerPrefs.Save();
    }


    public bool IsAuthenticated => !string.IsNullOrEmpty(authToken);
    public string UserType => userType;

    private void AddAuthHeader(UnityWebRequest request)
    {
        if (!string.IsNullOrEmpty(authToken))
        {
            request.SetRequestHeader("Authorization", $"Bearer {authToken}");
        }
    }

    private async Task<string> SendRequest(UnityWebRequest request, string endpoint)
    {
        AddAuthHeader(request);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[API] {request.method} {endpoint} failed: {request.responseCode}");
            if (request.downloadHandler?.data != null)
            {
                Debug.LogError($"[API] Response: {Encoding.UTF8.GetString(request.downloadHandler.data)}");
            }
            return null;
        }

        return request.downloadHandler.text;
    }

    private async Task<T> GetRequest<T>(string endpoint) where T : class
    {
        using (var request = UnityWebRequest.Get($"{baseUrl}{endpoint}"))
        {
            var json = await SendRequest(request, endpoint);
            if (json == null) return null;

            try
            {
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[API] Parse error: {e.Message}");
                return null;
            }
        }
    }

    private async Task<T> PostRequest<T>(string endpoint, object payload) where T : class
    {
        var jsonBody = JsonUtility.ToJson(payload ?? new { });

        using (var request = new UnityWebRequest($"{baseUrl}{endpoint}", "POST"))
        {
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var json = await SendRequest(request, endpoint);
            if (json == null) return null;

            try
            {
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[API] Parse error: {e.Message}");
                return null;
            }
        }
    }

    private async Task<T> PatchRequest<T>(string endpoint, object payload) where T : class
    {
        var jsonBody = JsonUtility.ToJson(payload ?? new { });

        using (var request = new UnityWebRequest($"{baseUrl}{endpoint}", "PATCH"))
        {
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var json = await SendRequest(request, endpoint);
            if (json == null) return null;

            return JsonUtility.FromJson<T>(json);
        }
    }

    private async Task<ApiMessage> DeleteRequest(string endpoint)
    {
        using (var request = UnityWebRequest.Delete($"{baseUrl}{endpoint}"))
        {
            var json = await SendRequest(request, endpoint);
            if (json == null) return null;
            return JsonUtility.FromJson<ApiMessage>(json);
        }
    }

    public async Task<AdminLoginResponse> AdminLogin(string email, string password)
    {
        var payload = new LoginPayload { email = email.ToLower().Trim(), password = password };
        var response = await PostRequest<AdminLoginResponse>("/admin/auth/login", payload);

        if (response?.token != null)
            SetToken(response.token, "admin");

        return response;
    }


    public async Task<AdminLoginResponse> AdminGetMe()
    {
        return await GetRequest<AdminLoginResponse>("/admin/auth/me");
    }

    public async Task<ApiMessage> AdminLogout()
    {
        var result = await PostRequest<ApiMessage>("/admin/auth/logout", null);
        ClearToken();
        return result;
    }

    public async Task<CompanyLoginResponse> CompanyLogin(string email, string password)
    {
        var payload = new LoginPayload { email = email.ToLower().Trim(), password = password };
        var response = await PostRequest<CompanyLoginResponse>("/company/auth/login", payload);

        if (response?.token != null)
            SetToken(response.token, "company");

        return response;
    }

    public async Task<CompanyLoginResponse> CompanyGetMe()
    {
        return await GetRequest<CompanyLoginResponse>("/company/auth/me");
    }

    public async Task<ApiMessage> CompanyForgotPassword(string email)
    {
        var payload = new { email = email.ToLower().Trim() };
        return await PostRequest<ApiMessage>("/company/auth/forgot", payload);
    }

    public async Task<ApiMessage> CompanyResetPassword(string resetToken, string newPassword)
    {
        var payload = new { password = newPassword };
        return await PostRequest<ApiMessage>($"/company/auth/reset/{resetToken}", payload);
    }

    public async Task<ApiMessage> CompanyLogout()
    {
        var result = await PostRequest<ApiMessage>("/company/auth/logout", null);
        ClearToken();
        return result;
    }

    public async Task<List<TrainingData>> GetAllTrainings()
    {
        var json = await GetRequest<string>("/admin/trainings");
        if (json == null) return new List<TrainingData>();

        var wrapper = JsonUtility.FromJson<TrainingListWrapper>(json);
        return wrapper?.trainings ?? new List<TrainingData>();
    }

    public async Task<TrainingData> GetTrainingById(string id)
    {
        var json = await GetRequest<string>($"/admin/trainings/{id}");
        if (json == null) return null;

        var wrapper = JsonUtility.FromJson<TrainingSingleWrapper>(json);
        return wrapper?.training;
    }

    public async Task<TrainingData> CreateTraining(string title, string description, string category, string thumbnailUrl = null)
    {
        var payload = new { title, description, category, thumbnailUrl };
        var json = await PostRequest<string>("/admin/trainings", payload);
        if (json == null) return null;

        var wrapper = JsonUtility.FromJson<TrainingCreateWrapper>(json);
        return wrapper?.training;
    }

    public async Task<bool> UpdateTraining(string id, string title = null, string description = null,
        string category = null, string thumbnailUrl = null, bool? isActive = null)
    {
        var payload = new Dictionary<string, object>();
        if (title != null) payload["title"] = title;
        if (description != null) payload["description"] = description;
        if (category != null) payload["category"] = category;
        if (thumbnailUrl != null) payload["thumbnailUrl"] = thumbnailUrl;
        if (isActive.HasValue) payload["isActive"] = isActive.Value;

        var json = await PatchRequest<ApiMessage>($"/admin/trainings/{id}", payload);
        return json != null;
    }

    public async Task<bool> DeleteTraining(string id)
    {
        var response = await DeleteRequest($"/admin/trainings/{id}");
        return response != null;
    }

    public async Task<List<CompanyData>> GetAllCompanies()
    {
        var json = await GetRequest<string>("/admin/companies");
        if (json == null) return new List<CompanyData>();

        var wrapper = JsonUtility.FromJson<CompanyListWrapper>(json);
        return wrapper?.companies ?? new List<CompanyData>();
    }

    public async Task<CompanyData> GetCompanyById(string id)
    {
        var json = await GetRequest<string>($"/admin/companies/{id}");
        if (json == null) return null;

        var wrapper = JsonUtility.FromJson<CompanySingleWrapper>(json);
        return wrapper?.company;
    }

    public async Task<CreateCompanyResponse> CreateCompany(string companyName, string email, string password)
    {
        var payload = new { companyName, email = email.ToLower().Trim(), password };
        return await PostRequest<CreateCompanyResponse>("/admin/companies", payload);
    }

    public async Task<bool> UpdateCompany(string id, string companyName = null, string email = null, bool? isActive = null)
    {
        var payload = new Dictionary<string, object>();
        if (companyName != null) payload["companyName"] = companyName;
        if (email != null) payload["email"] = email.ToLower().Trim();
        if (isActive.HasValue) payload["isActive"] = isActive.Value;

        var json = await PatchRequest<ApiMessage>($"/admin/companies/{id}", payload);
        return json != null;
    }

    public async Task<bool> DeleteCompany(string id)
    {
        var response = await DeleteRequest($"/admin/companies/{id}");
        return response != null;
    }

    public async Task<AssignTrainingsResponse> AssignTrainingsToCompany(string companyId, List<string> trainingIds)
    {
        var payload = new { trainingIds };
        return await PostRequest<AssignTrainingsResponse>($"/admin/companies/{companyId}/trainings", payload);
    }

    public async Task<ApiMessage> AddTrainingToCompany(string companyId, string trainingId)
    {
        var payload = new { trainingId };
        return await PostRequest<ApiMessage>($"/admin/companies/{companyId}/trainings", payload);
    }

    public async Task<ApiMessage> RemoveTrainingFromCompany(string companyId, string trainingId)
    {
        return await DeleteRequest($"/admin/companies/{companyId}/trainings/{trainingId}");
    }

    public async Task<SessionData> SubmitSession(
        string trainingId,
        DateTime startedAt,
        DateTime completedAt,
        int score,
        bool passed,
        List<EvaluationCriteriaData> evaluationCriteria = null,
        string notes = null)
    {
        var payload = new
        {
            trainingId,
            startedAt = startedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            completedAt = completedAt.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            score,
            passed,
            evaluationCriteria = evaluationCriteria ?? new List<EvaluationCriteriaData>(),
            notes
        };

        var json = await PostRequest<string>("/sessions/submit", payload);
        if (json == null) return null;

        var wrapper = JsonUtility.FromJson<SessionCreateWrapper>(json);
        return wrapper?.session;
    }

    public async Task<List<SessionData>> GetMySessions(string trainingId = null)
    {
        string endpoint = "/sessions/my";
        if (!string.IsNullOrEmpty(trainingId))
            endpoint += $"?trainingId={trainingId}";

        var json = await GetRequest<string>(endpoint);
        if (json == null) return new List<SessionData>();

        var wrapper = JsonUtility.FromJson<SessionListWrapper>(json);
        return wrapper?.sessions ?? new List<SessionData>();
    }

    public async Task<List<SessionData>> GetAllSessions(string companyId = null, string trainingId = null)
    {
        string endpoint = "/sessions";
        var parameters = new List<string>();
        if (!string.IsNullOrEmpty(companyId)) parameters.Add($"companyId={companyId}");
        if (!string.IsNullOrEmpty(trainingId)) parameters.Add($"trainingId={trainingId}");
        if (parameters.Count > 0) endpoint += "?" + string.Join("&", parameters);

        var json = await GetRequest<string>(endpoint);
        if (json == null) return new List<SessionData>();

        var wrapper = JsonUtility.FromJson<SessionListWrapper>(json);
        return wrapper?.sessions ?? new List<SessionData>();
    }

    public async Task<List<TrainingStats>> GetStatsByTraining()
    {
        var json = await GetRequest<string>("/sessions/stats/trainings");
        if (json == null) return new List<TrainingStats>();

        var wrapper = JsonUtility.FromJson<TrainingStatsWrapper>(json);
        return wrapper?.stats ?? new List<TrainingStats>();
    }

    public async Task<List<CompanyStats>> GetStatsByCompany()
    {
        var json = await GetRequest<string>("/sessions/stats/companies");
        if (json == null) return new List<CompanyStats>();

        var wrapper = JsonUtility.FromJson<CompanyStatsWrapper>(json);
        return wrapper?.stats ?? new List<CompanyStats>();
    }

    public async Task<CompanyStatsDetail> GetStatsForCompany(string companyId)
    {
        return await GetRequest<CompanyStatsDetail>($"/sessions/stats/companies/{companyId}");
    }
    public void SetBaseUrl(string url)
    {
        baseUrl = url;
    }
}