// SessionManager.cs — Singleton persistant entre les scènes
public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance { get; private set; }

    // ─── Données de session ──────────────────────────────────
    public string TrainingId { get; private set; }
    public string EmployeeName { get; private set; }
    public string EmployeeUniqueId { get; private set; }
    public DateTime StartedAt { get; private set; }
    public float TotalScore { get; private set; }
    public bool Passed { get; private set; }
    public List<EvaluationCriteria> Criteria { get; private set; }

    void Awake()
    {
        // Singleton persistant entre les scènes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Criteria = new List<EvaluationCriteria>();
    }

    public void StartSession(string trainingId,
                             string employeeName,
                             string employeeUniqueId)
    {
        TrainingId = trainingId;
        EmployeeName = employeeName;
        EmployeeUniqueId = employeeUniqueId;
        StartedAt = DateTime.UtcNow;
        TotalScore = 0f;
        Passed = false;
        Criteria.Clear();
    }

    public void AddCriteria(string name, bool passed, float score)
    {
        Criteria.Add(new EvaluationCriteria
        {
            criteriaName = name,
            passed = passed,
            score = score
        });
    }

    public void FinalizeSession(float score, bool passed)
    {
        TotalScore = score;
        Passed = passed;
    }
}

[Serializable]
public class EvaluationCriteria
{
    public string criteriaName;
    public bool passed;
    public float score;
}