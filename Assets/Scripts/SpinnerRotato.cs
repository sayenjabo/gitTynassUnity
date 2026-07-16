using UnityEngine;

public class SpinnerRotato : MonoBehaviour
{
    [SerializeField] private float speed = 180f;

    void Update()
    {
        transform.Rotate(0f, 0f, -speed * Time.deltaTime);
    }
}