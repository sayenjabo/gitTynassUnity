using System;
using System.Collections;
using UnityEngine;
using Oculus.Interaction;

public class LessonTaskValidator : MonoBehaviour
{
    public static LessonTaskValidator Instance { get; private set; }

    [Header("Lesson 2 — Cube Position")]
    [SerializeField] private Transform cube;
    [SerializeField] private Transform cubeTarget;
    [SerializeField] private GameObject visualPlaceForTarget;
    [SerializeField] private float cubeTargetRadius = 0.3f;

    [Header("Lesson 2 — Door")]
    [SerializeField] private Grabbable doorGrabbable;

    [Header("Lesson 3 — Interaction")]
    [SerializeField] private GameObject testInteractionCanvas;
    [SerializeField] private GameObject informationCanvas;

    [Header("Waypoints")]
    [SerializeField] public GameObject waypointLesson1Skip;
    [SerializeField] private GameObject waypointLesson1Pass;
    [SerializeField] private GameObject waypointLesson2Skip;
    [SerializeField] private GameObject waypointLesson2Pass;

    // Events
    public static event Action OnLesson1Completed;
    public static event Action OnLesson2Completed;
    public static event Action OnLesson3Completed;

    // Flags
    private bool _lesson1WaypointReached = false;
    private bool _lesson1Skipped = false;
    private bool _lesson2CubeDone = false;
    private bool _lesson2WaypointReached = false;
    private bool _lesson2Skipped = false;
    private bool _understandPressed = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Tout désactiver au départ
        waypointLesson1Skip?.SetActive(false);
        waypointLesson1Pass?.SetActive(false);
        waypointLesson2Skip?.SetActive(false);
        waypointLesson2Pass?.SetActive(false);

        if (testInteractionCanvas != null) testInteractionCanvas.SetActive(false);
        if (informationCanvas != null) informationCanvas.SetActive(false);
    }

    // ─────────────────────────────────────────
    // WAYPOINT CALLBACKS
    // ─────────────────────────────────────────
    public void OnWaypointLesson1Reached() => _lesson1WaypointReached = true;
    public void OnLesson1Skipped() => _lesson1Skipped = true;
    public void OnWaypointLesson2Reached() => _lesson2WaypointReached = true;
    public void OnLesson2Skipped() => _lesson2Skipped = true;
    public void OnUnderstandPressed()
    {
        _understandPressed = true;
        Debug.Log("[Validator] Bouton Understand appuyé");
    }

    // ─────────────────────────────────────────
    // RESET
    // ─────────────────────────────────────────
    public void ResetLesson1()
    {
        _lesson1Skipped = false;
        _lesson1WaypointReached = false;
    }

    public void ResetLesson2()
    {
        _lesson2Skipped = false;
        _lesson2CubeDone = false;
        _lesson2WaypointReached = false;
    }

    public void ResetLesson3()
    {
        _understandPressed = false;
    }

    // ─────────────────────────────────────────
    // PROPRIÉTÉS PUBLIQUES
    // ─────────────────────────────────────────
    public bool IsLesson1Skipped => _lesson1Skipped;
    public bool IsLesson2Skipped => _lesson2Skipped;

    public GameObject WaypointLesson1Skip => waypointLesson1Skip;
    public GameObject WaypointLesson1Pass => waypointLesson1Pass;
    public GameObject WaypointLesson2Skip => waypointLesson2Skip;
    public GameObject WaypointLesson2Pass => waypointLesson2Pass;

    // ─────────────────────────────────────────
    // LESSON 1
    // ─────────────────────────────────────────
    public IEnumerator WaitForLesson1()
    {
        Debug.Log("[Validator] Lesson 1 — Démarrage");

        // NE PAS reset ici — déjà fait dans ResetLesson1()
        // Attendre skip OU pass
        yield return new WaitUntil(() => _lesson1Skipped || _lesson1WaypointReached);

        if (_lesson1Skipped)
            Debug.Log("[Validator] Lesson 1 — Skippée ⏭");
        else
            Debug.Log("[Validator] Lesson 1 — Complétée ✅");

        OnLesson1Completed?.Invoke();
    }

    // ─────────────────────────────────────────
    // LESSON 2
    // ─────────────────────────────────────────
    public IEnumerator WaitForLesson2()
    {
        Debug.Log("[Validator] Lesson 2 — Démarrage");

        if (_lesson2Skipped)
        {
            Debug.Log("[Validator] Lesson 2 — Skippée avant démarrage ⏭");
            OnLesson2Completed?.Invoke();
            yield break;
        }

        yield return new WaitUntil(() =>
        {
            if (_lesson2Skipped) return true;

            Grabbable grabbable = cube.GetComponent<Grabbable>();
            bool isReleased = grabbable != null && grabbable.SelectingPointsCount == 0;
            float distance = Vector3.Distance(cube.position, cubeTarget.position);

            if (isReleased && distance <= cubeTargetRadius)
            {
                _lesson2CubeDone = true;
                return true;
            }

            return false;
        });

        if (_lesson2Skipped)
        {
            Debug.Log("[Validator] Lesson 2 — Toutes les étapes skippées ⏭");
            OnLesson2Completed?.Invoke();
            yield break;
        }

        SnapCube();
        ActivatePass(waypointLesson2Skip, waypointLesson2Pass);

        if (visualPlaceForTarget != null)
            visualPlaceForTarget.SetActive(false);

        Debug.Log("[Validator] Lesson 2 — Cube en position ✅");

        yield return new WaitUntil(() => _lesson2WaypointReached);

        Debug.Log("[Validator] Lesson 2 — Complétée ✅");
        OnLesson2Completed?.Invoke();
    }

    // ─────────────────────────────────────────
    // LESSON 3
    // ─────────────────────────────────────────
    public IEnumerator WaitForLesson3()
    {
        Debug.Log("[Validator] Lesson 3 — Démarrage");

        // NE PAS reset ici — déjà fait dans ResetLesson3()
        // Etape 1 — Afficher Test Interaction Canvas
        testInteractionCanvas.SetActive(true);
        informationCanvas.SetActive(false);

        // Etape 2 — Attendre bouton Understand (Test canvas)
        yield return new WaitUntil(() => _understandPressed);

        // Etape 3 — Afficher Information Canvas
        _understandPressed = false;
        testInteractionCanvas.SetActive(false);
        informationCanvas.SetActive(true);

        Debug.Log("[Validator] Information canvas affiché");

        // Etape 4 — Attendre bouton Understand (Information canvas)
        yield return new WaitUntil(() => _understandPressed);

        informationCanvas.SetActive(false);

        Debug.Log("[Validator] Lesson 3 — Complétée ✅");
        OnLesson3Completed?.Invoke();
    }

    // ─────────────────────────────────────────
    // SNAP CUBE
    // ─────────────────────────────────────────
    private void SnapCube()
    {
        cube.position = cubeTarget.position;
        cube.rotation = cubeTarget.rotation;

        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Grabbable grabbable = cube.GetComponent<Grabbable>();
        if (grabbable != null)
            grabbable.enabled = false;

        Debug.Log("[Validator] Cube snappé ✅");
    }

    // ─────────────────────────────────────────
    // WAYPOINT ACTIVATION
    // ─────────────────────────────────────────
    public void ActivatePass(GameObject skip, GameObject pass)
    {
        skip?.SetActive(false);
        pass?.SetActive(true);
    }
    public void SetDoorGrabEnabled(bool enabled)
    {
        if (doorGrabbable != null)
            doorGrabbable.enabled = enabled;
    }

}