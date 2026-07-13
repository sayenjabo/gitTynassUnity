// TrainingMenuController.cs
/*
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingMenuController : MonoBehaviour
{
    public static TrainingMenuController Instance
    {
        get;
        private set;
    }

    [SerializeField] private Transform trainingsContainer;
    [SerializeField] private GameObject trainingCardPrefab;

    private TrainingData[] _trainings;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadTrainings(TrainingData[] trainings)
    {
        _trainings = trainings;

        // Nettoyer les anciennes cartes
        foreach (Transform child in trainingsContainer)
            Destroy(child.gameObject);

        // Créer une carte par formation
        foreach (var training in trainings)
        {
            var card = Instantiate(trainingCardPrefab,
                trainingsContainer);
            card.GetComponent<TrainingCard>()
                .Initialize(training, OnTrainingSelected);
        }
    }

    private void OnTrainingSelected(TrainingData training)
    {
        // Demander nom + ID employé avant de lancer
        EmployeeInputController.Instance
            .Show(training, OnEmployeeConfirmed);
    }

    private void OnEmployeeConfirmed(string name,
                                     string uniqueId,
                                     TrainingData training)
    {
        SessionManager.Instance.StartSession(
            training._id, name, uniqueId);
        SceneManager.LoadScene("SimulationScene");
    }
}*/