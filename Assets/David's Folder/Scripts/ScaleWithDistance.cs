using UnityEngine;

public class ScaleWithDistance : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The object to measure distance to. If left empty, it defaults to the Main Camera.")]
    public Transform targetTransform;

    [Header("Distance Settings")]
    [Tooltip("The distance at which the object will be at its minScale.")]
    public float minDistance = 5f;
    [Tooltip("The distance at which the object will be at its maxScale.")]
    public float maxDistance = 50f;

    [Header("Scale Settings")]
    [Tooltip("The minimum absolute world scale of the object.")]
    public float minScale = 1f;
    [Tooltip("The maximum absolute world scale of the object.")]
    public float maxScale = 5f;

    void Start()
    {
        // If no target was assigned in the Inspector, default to the Main Camera
        if (targetTransform == null)
        {
            if (Camera.main != null)
            {
                targetTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("ScaleWithDistance: No target assigned and no camera tagged as 'MainCamera' was found!");
            }
        }
    }

    void Update()
    {
        if (targetTransform == null) return;

        // 1. Calculate distance
        float distance = Vector3.Distance(transform.position, targetTransform.position);

        // 2. Calculate interpolation factor (t)
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);

        // 3. Find the desired absolute world scale
        float targetScale = Mathf.Lerp(minScale, maxScale, t);

        // 4. Apply scale, counteracting the parent's scale if one exists
        if (transform.parent != null)
        {
            Vector3 parentScale = transform.parent.lossyScale;

            // Prevent division by zero errors in case a parent scale is accidentally set to 0
            float scaleX = parentScale.x != 0 ? targetScale / parentScale.x : targetScale;
            float scaleY = parentScale.y != 0 ? targetScale / parentScale.y : targetScale;
            float scaleZ = parentScale.z != 0 ? targetScale / parentScale.z : targetScale;

            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
        else
        {
            // If there is no parent, localScale is the same as world scale
            transform.localScale = Vector3.one * targetScale;
        }
    }
}