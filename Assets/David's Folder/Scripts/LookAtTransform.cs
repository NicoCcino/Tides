using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    [SerializeField]
    private LookAtTarget lookAtTarget = LookAtTarget.MainCamera;

    [SerializeField]
    private Transform customTarget;

    [SerializeField]
    private bool inverseDirection = false;

    [SerializeField]
    private Vector3 upDirection = Vector3.up;

    [SerializeField]
    private bool useLocalLookAt = false;

    [SerializeField]
    private float smoothTime = 0.2f;

    [field: SerializeField] public bool LockX { get; private set; }
    [field: SerializeField] public bool LockY { get; private set; }
    [field: SerializeField] public bool LockZ { get; private set; }

    private Vector3 lockedWorldEuler;

    private void Start()
    {
        lockedWorldEuler = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        Transform target = GetLookTarget();
        if (target == null) return;

        Vector3 targetPosition = target.position;
        if (inverseDirection)
        {
            targetPosition = transform.position - (targetPosition - transform.position);
        }

        Vector3 direction = targetPosition - transform.position;
        if (direction == Vector3.zero) return;

        Quaternion lookRotation = Quaternion.LookRotation(direction, upDirection);
        
        Vector3 euler = lookRotation.eulerAngles;

        if (LockX) euler.x = lockedWorldEuler.x;
        if (LockY) euler.y = lockedWorldEuler.y;
        if (LockZ) euler.z = lockedWorldEuler.z;

        Quaternion targetWorldRotation = Quaternion.Euler(euler);

        if (smoothTime > 0)
        {
            targetWorldRotation = Quaternion.Lerp(transform.rotation, targetWorldRotation, Time.deltaTime / smoothTime);
        }

        if (useLocalLookAt)
        {
            if (transform.parent != null)
            {
                transform.localRotation = Quaternion.Inverse(transform.parent.rotation) * targetWorldRotation;
            }
            else
            {
                transform.localRotation = targetWorldRotation;
            }
        }
        else
        {
            transform.rotation = targetWorldRotation;
        }
    }

    private Transform GetLookTarget()
    {
        return lookAtTarget switch
        {
            LookAtTarget.MainCamera => Camera.main?.transform,
            LookAtTarget.CustomTarget => customTarget,
            _ => null
        };
    }

    public enum LookAtTarget
    {
        MainCamera,
        CustomTarget
    }
}
