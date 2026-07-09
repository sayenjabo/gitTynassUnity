using Oculus.Interaction.Locomotion;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TrainingSceneManager : MonoBehaviour
{
    [Header("Narration — Environnement")]
    [SerializeField] private AudioClip welcomeClip;
    [SerializeField] private AudioClip lesson1IntroClip;
    [SerializeField] private AudioClip lesson2IntroClip;
    [SerializeField] private AudioClip lesson3IntroClip;
    [SerializeField] private AudioClip completionClip;

    [Header("Narration — Rappels")]
    [SerializeField] private AudioClip lesson1ReminderClip;
    [SerializeField] private AudioClip lesson2ReminderClip;
    [SerializeField] private AudioClip lesson3ReminderClip;

    [Header("UI")]
    [SerializeField] private GameObject choiceCanvas;

    [Header("Videos")]
    [SerializeField] private VideoPlayer lesson1Video;
    [SerializeField] private VideoPlayer lesson2Video;
    [SerializeField] private VideoPlayer lesson3Video;

    [Header("Video Names")]
    [SerializeField] private string lesson1FileName = "lesson1.mp4";
    [SerializeField] private string lesson2FileName = "lesson2.mp4";
    [SerializeField] private string lesson3FileName = "lesson3.mp4";

    [Header("Locomotion")]
    [SerializeField] private FirstPersonLocomotor locomotor;

    private bool _playerSkipped;

    // ─────────────────────────────────────────
    private void Start()
    {
        lesson1Video.url = System.IO.Path.Combine(Application.streamingAssetsPath, lesson1FileName);
        lesson2Video.url = System.IO.Path.Combine(Application.streamingAssetsPath, lesson2FileName);
        lesson3Video.url = System.IO.Path.Combine(Application.streamingAssetsPath, lesson3FileName);

        SpawnManager.Instance.SpawnAtPoint1();
        choiceCanvas.SetActive(false);

        StartCoroutine(RunScenario());
    }

    private IEnumerator RunScenario()
    {
        yield return StartCoroutine(Step_SpawnAndWelcome());
        yield return StartCoroutine(Step_ABChoice());

        if (!_playerSkipped)
        {
            yield return StartCoroutine(Step_Lesson1());
            yield return StartCoroutine(Step_Lesson2());
            yield return StartCoroutine(Step_Lesson3());
        }

        yield return StartCoroutine(Step_Complete());
    }

    // ─────────────────────────────────────────
    // STEP 1 — Spawn + Welcome
    // ─────────────────────────────────────────
    private IEnumerator Step_SpawnAndWelcome()
    {
        Debug.Log("[Scenario] Step 1 — Spawn + Welcome");
        locomotor.DisableMovement();
        yield return StartCoroutine(NarrationManager.Instance.Play(welcomeClip));
    }

    // ─────────────────────────────────────────
    // STEP 2 — A/B Choice
    // ─────────────────────────────────────────
    private IEnumerator Step_ABChoice()
    {
        Debug.Log("[Scenario] Step 2 — Waiting for A/B choice");

        choiceCanvas.SetActive(true);
        _playerSkipped = false;

        yield return null;
        yield return null;

        yield return new WaitUntil(() =>
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                _playerSkipped = false;
                return true;
            }
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                _playerSkipped = true;
                return true;
            }
            return false;
        });

        choiceCanvas.SetActive(false);

        if (_playerSkipped)
        {
            SceneManager.LoadScene("Companys");
            yield break;
        }

        // NE PAS activer locomotion ici
        Debug.Log("[Scenario] Joueur commence les leçons");
    }


    // ─────────────────────────────────────────
    // STEP 3 — Lessons 
    // ─────────────────────────────────────────
    private IEnumerator Step_Lesson1()
    {
        Debug.Log("[Scenario] Step 3 — Lesson 1 : Mouvement");

        // Voix intro
        yield return StartCoroutine(NarrationManager.Instance.Play(lesson1IntroClip));

        // Activer locomotion après le clip — velocity reset automatique
        locomotor.EnableMovement();

        LessonTaskValidator.Instance.waypointLesson1Skip?.SetActive(true); ;
        LessonTaskValidator.Instance.ResetLesson1();

        yield return StartCoroutine(PlayVideoOrSkip(lesson1Video, () =>
            LessonTaskValidator.Instance.IsLesson1Skipped));

        if (!LessonTaskValidator.Instance.IsLesson1Skipped)
            LessonTaskValidator.Instance.ActivatePass(LessonTaskValidator.Instance.WaypointLesson1Skip,LessonTaskValidator.Instance.WaypointLesson1Pass);

        NarrationManager.Instance.StartReminder(lesson1ReminderClip, 15f);
        yield return StartCoroutine(LessonTaskValidator.Instance.WaitForLesson1());
        NarrationManager.Instance.StopReminder();
    }

    private IEnumerator Step_Lesson2()
    {
        Debug.Log("[Scenario] Step 4 — Lesson 2 : Grip");

        LessonTaskValidator.Instance.SetDoorGrabEnabled(false);

        // Voix environnement intro
        yield return StartCoroutine(NarrationManager.Instance.Play(lesson2IntroClip));

        LessonTaskValidator.Instance.ResetLesson2();

        // Vidéo
        yield return StartCoroutine(PlayVideoOrSkip(lesson2Video, () =>
            LessonTaskValidator.Instance.IsLesson2Skipped));

        LessonTaskValidator.Instance.SetDoorGrabEnabled(true);

        // Démarrer rappel pendant la tâche
        NarrationManager.Instance.StartReminder(lesson2ReminderClip, 15f);

        yield return StartCoroutine(LessonTaskValidator.Instance.WaitForLesson2());

        // Arrêter rappel
        NarrationManager.Instance.StopReminder();
    }

    private IEnumerator Step_Lesson3()
    {
        Debug.Log("[Scenario] Step 5 — Lesson 3 : Interaction");

        // Voix environnement intro
        yield return StartCoroutine(NarrationManager.Instance.Play(lesson3IntroClip));

        LessonTaskValidator.Instance.ResetLesson3();

        // Vidéo
        yield return StartCoroutine(PlayVideo(lesson3Video));

        // Démarrer rappel pendant la tâche
        NarrationManager.Instance.StartReminder(lesson3ReminderClip, 15f);

        yield return StartCoroutine(LessonTaskValidator.Instance.WaitForLesson3());

        // Arrêter rappel
        NarrationManager.Instance.StopReminder();
    }

    // ─────────────────────────────────────────
    // STEP 4 — Complete
    // ─────────────────────────────────────────
    private IEnumerator Step_Complete()
    {
        Debug.Log("[Scenario] Step 6 — Complete");
        locomotor.DisableMovement();
        yield return StartCoroutine(NarrationManager.Instance.Play(completionClip));
        SceneManager.LoadScene("Companys");
    }

    // ─────────────────────────────────────────
    // VIDEO HELPER — avec skip
    // ─────────────────────────────────────────
    private IEnumerator PlayVideoOrSkip(VideoPlayer player, System.Func<bool> isSkipped)
    {
        if (player == null)
        {
            Debug.LogError("[Scenario] VideoPlayer non assigné.");
            yield break;
        }

        player.Stop();
        player.Prepare();

        float timeout = 5f;
        float elapsed = 0f;

        while (!player.isPrepared && elapsed < timeout && !isSkipped())
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (isSkipped())
        {
            Debug.Log($"[Scenario] Vidéo skippée pendant préparation : {player.name}");
            yield break;
        }

        if (!player.isPrepared)
        {
            Debug.LogError($"[Scenario] VideoPlayer timeout : {player.name}");
            yield break;
        }

        bool finished = false;
        void OnEnd(VideoPlayer vp) => finished = true;

        player.loopPointReached += OnEnd;
        player.Play();

        yield return new WaitUntil(() => finished || isSkipped());

        player.loopPointReached -= OnEnd;
        player.Stop();

        Debug.Log($"[Scenario] Vidéo arrêtée : {player.name}");
    }

    // ─────────────────────────────────────────
    // VIDEO HELPER — sans skip
    // ─────────────────────────────────────────
    private IEnumerator PlayVideo(VideoPlayer player)
    {
        if (player == null)
        {
            Debug.LogError("[Scenario] VideoPlayer non assigné.");
            yield break;
        }

        player.Stop();
        player.Prepare();

        float timeout = 5f;
        float elapsed = 0f;

        while (!player.isPrepared && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!player.isPrepared)
        {
            Debug.LogError($"[Scenario] VideoPlayer timeout : {player.name}");
            yield break;
        }

        bool finished = false;
        void OnEnd(VideoPlayer vp) => finished = true;

        player.loopPointReached += OnEnd;
        player.Play();

        yield return new WaitUntil(() => finished);

        player.loopPointReached -= OnEnd;
        Debug.Log($"[Scenario] Vidéo terminée : {player.name}");
    }

}

