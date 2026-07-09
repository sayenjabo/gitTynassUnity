using UnityEngine;

public class CompanySceneManager : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private OVRCameraRig cameraRig;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (spawnPoint == null || cameraRig == null)
        {
            Debug.LogError("[CompanySceneManager] SpawnPoint ou CameraRig non assigné.");
            return;
        }

        cameraRig.transform.position = spawnPoint.position;
        cameraRig.transform.rotation = spawnPoint.rotation;

        Debug.Log("[CompanySceneManager] Joueur spawné devant le login canvas.");
    }
}