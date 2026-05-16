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

    private Quaternion targetRotation;

    private void Update()
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
        Vector3 currentEuler = useLocalLookAt ? transform.localEulerAngles : transform.eulerAngles;

        if (LockX) euler.x = currentEuler.x;
        if (LockY) euler.y = currentEuler.y;
        if (LockZ) euler.z = currentEuler.z;

        targetRotation = Quaternion.Euler(euler);

        if (useLocalLookAt)
        {
            transform.localRotation = targetRotation;
        }
        else
        {
            if (smoothTime > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / smoothTime);
            }
            else
            {
                transform.rotation = targetRotation;
            }
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
