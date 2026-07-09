using UnityEngine;
using System.Collections.Generic;

public class WaypointTrigger : MonoBehaviour
{
    public enum WaypointType
    {
        Lesson1_Skip,
        Lesson1_Pass,
        Lesson2_Skip,
        Lesson2_Pass
    }

    [SerializeField] private WaypointType waypointType;

    [Header("Auto Activation")]
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private List<GameObject> toDeactivate;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Notifier le validator
        switch (waypointType)
        {
            case WaypointType.Lesson1_Skip: LessonTaskValidator.Instance.OnLesson1Skipped(); break;
            case WaypointType.Lesson1_Pass: LessonTaskValidator.Instance.OnWaypointLesson1Reached(); break;
            case WaypointType.Lesson2_Skip: LessonTaskValidator.Instance.OnLesson2Skipped(); break;
            case WaypointType.Lesson2_Pass: LessonTaskValidator.Instance.OnWaypointLesson2Reached(); break;
        }

        // Parcourir et activer/désactiver automatiquement
        ApplyActivations();

        // Se désactiver lui-même
        gameObject.SetActive(false);
    }

    private void ApplyActivations()
    {
        foreach (GameObject go in toActivate)
            if (go != null) go.SetActive(true);

        foreach (GameObject go in toDeactivate)
            if (go != null) go.SetActive(false);
    }
}