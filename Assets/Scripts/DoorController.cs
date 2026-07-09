using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private float grabDistance = 0.3f;
    [SerializeField] private float rotationSpeed = 150f;

    private HingeJoint _hingeJoint;
    private Rigidbody _rigidbody;
    private bool _isGrabbed = false;
    private Transform _grabbingHand;
    private float _previousHandAngle;

    private void Awake()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Détecter grip droit
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
            TryGrab(rightHandTransform);

        // Détecter grip gauche
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
            TryGrab(leftHandTransform);

        // Relâcher
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) ||
            OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
            Release();

        // Tourner la porte si grabbée
        if (_isGrabbed)
            RotateDoor();
    }

    private void TryGrab(Transform hand)
    {
        if (hand == null) return;

        float distance = Vector3.Distance(hand.position, transform.position);
        if (distance <= grabDistance)
        {
            _isGrabbed = true;
            _grabbingHand = hand;
            _previousHandAngle = GetHandAngle(hand);
            Debug.Log("[Door] Porte grabbée ✅");
        }
    }

    private void Release()
    {
        _isGrabbed = false;
        _grabbingHand = null;
        _rigidbody.angularVelocity = Vector3.zero;
        Debug.Log("[Door] Porte relâchée");
    }

    private void RotateDoor()
    {
        if (_grabbingHand == null) return;

        float currentAngle = GetHandAngle(_grabbingHand);
        float delta = Mathf.DeltaAngle(_previousHandAngle, currentAngle);
        _previousHandAngle = currentAngle;

        _rigidbody.angularVelocity = transform.forward * delta * rotationSpeed * Time.deltaTime;
    }

    private float GetHandAngle(Transform hand)
    {
        Vector3 pivotPosition = transform.position +
                                transform.TransformVector(_hingeJoint.anchor);

        Vector3 toHand = hand.position - pivotPosition;
        Vector3 projected = Vector3.ProjectOnPlane(toHand, transform.forward);

        return Vector3.SignedAngle(Vector3.up, projected, transform.forward);
    }
}