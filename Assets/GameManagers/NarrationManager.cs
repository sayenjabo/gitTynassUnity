using System.Collections;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance { get; private set; }

    [SerializeField] private AudioSource spaceVoiceSource;

    private Coroutine _reminderCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Joue un clip et attend la fin
    public IEnumerator Play(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("[NarrationManager] Clip non assigné.");
            yield break;
        }

        spaceVoiceSource.clip = clip;
        spaceVoiceSource.Play();

        Debug.Log($"[NarrationManager] Playing: {clip.name}");
        yield return new WaitWhile(() => spaceVoiceSource.isPlaying);
        Debug.Log("[NarrationManager] Narration terminée.");
    }

    // Démarre le rappel périodique
    public void StartReminder(AudioClip reminderClip, float interval = 15f)
    {
        StopReminder();
        if (reminderClip == null)
        {
            Debug.LogWarning("[NarrationManager] Reminder clip non assigné.");
            return;
        }
        _reminderCoroutine = StartCoroutine(ReminderLoop(reminderClip, interval));
    }

    // Arrête le rappel périodique
    public void StopReminder()
    {
        if (_reminderCoroutine != null)
        {
            StopCoroutine(_reminderCoroutine);
            _reminderCoroutine = null;
        }
    }

    public void Stop()
    {
        spaceVoiceSource.Stop();
    }

    public bool IsPlaying => spaceVoiceSource.isPlaying;

    private IEnumerator ReminderLoop(AudioClip clip, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // Attendre que la narration courante soit terminée
            yield return new WaitWhile(() => spaceVoiceSource.isPlaying);

            spaceVoiceSource.clip = clip;
            spaceVoiceSource.Play();
            Debug.Log($"[NarrationManager] Rappel: {clip.name}");
        }
    }
}