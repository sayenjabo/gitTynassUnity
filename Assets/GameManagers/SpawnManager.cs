using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Spawn Points")]
    [SerializeField] private Transform spawnPoint;

    [Header("OVR Camera Rig")]
    [SerializeField] private OVRCameraRig cameraRig;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnAtPoint1() => SpawnAt(spawnPoint);

    // SUPPRIMÉ — SpawnAtPoint2() n'est plus utilisé

    private void SpawnAt(Transform point)
    {
        if (point == null)
        {
            Debug.LogError("[SpawnManager] Spawn point non assigné.");
            return;
        }

        cameraRig.transform.position = point.position;
        cameraRig.transform.rotation = point.rotation;

        Debug.Log($"[SpawnManager] Joueur spawné à {point.name}");
    }
}