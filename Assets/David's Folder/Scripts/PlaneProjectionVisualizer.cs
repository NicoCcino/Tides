using UnityEngine;

namespace David.Utils
{
    /// <summary>
    /// A simple visualizer to test the PlaneProjectionHelper in the scene.
    /// Attach this to a GameObject that represents the "moving point".
    /// </summary>
    public class PlaneProjectionVisualizer : MonoBehaviour
    {
        [field: SerializeField] public Vector3 MovementDirection { get; private set; } = Vector3.forward;
        [field: SerializeField] public Transform TargetToTest { get; private set; }
        [field: SerializeField] public float GizmoSize { get; private set; } = 5f;

        private void OnDrawGizmos()
        {
            if (MovementDirection.sqrMagnitude < 0.001f) return;

            Vector3 pos = transform.position;
            Vector3 horizontalDir = new Vector3(MovementDirection.x, 0, MovementDirection.z).normalized;
            
            // Draw the direction vector
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(pos, horizontalDir * 2f);

            // Draw the "Plane" (represented as a cross or a large square)
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Vector3 right = Vector3.Cross(Vector3.up, horizontalDir);
            
            // Draw a rectangle to represent the vertical plane
            Vector3 p1 = pos + (right * GizmoSize) + (Vector3.up * GizmoSize);
            Vector3 p2 = pos - (right * GizmoSize) + (Vector3.up * GizmoSize);
            Vector3 p3 = pos - (right * GizmoSize) - (Vector3.up * GizmoSize);
            Vector3 p4 = pos + (right * GizmoSize) - (Vector3.up * GizmoSize);

            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);

            if (TargetToTest != null)
            {
                Vector3 targetPos = TargetToTest.position;
                bool isInFront = PlaneProjectionHelper.IsPointInFrontOfPlane(pos, horizontalDir, targetPos);
                float distance = PlaneProjectionHelper.GetSignedDistanceToPlane(pos, horizontalDir, targetPos);
                Vector3 projected = PlaneProjectionHelper.ProjectPointOntoPlane(pos, horizontalDir, targetPos);

                // Draw line to target
                Gizmos.color = isInFront ? Color.green : Color.red;
                Gizmos.DrawLine(pos, targetPos);
                Gizmos.DrawSphere(targetPos, 0.2f);

                // Draw projection
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(projected, 0.15f);
                Gizmos.DrawLine(targetPos, projected);

                #if UNITY_EDITOR
                UnityEditor.Handles.Label(targetPos + Vector3.up * 0.5f, 
                    $"In Front: {isInFront}\nDistance: {distance:F2}");
                #endif
            }
        }
    }
}
