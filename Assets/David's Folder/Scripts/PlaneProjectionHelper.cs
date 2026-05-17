using UnityEngine;

namespace David.Utils
{
    /// <summary>
    /// Utility class to determine the relationship between world positions and a vertical plane 
    /// defined by a moving point and its horizontal direction.
    /// </summary>
    public static class PlaneProjectionHelper
    {
        /// <summary>
        /// Determines if a point is in front of or behind a vertical plane.
        /// The plane passes through 'planePoint' and its normal is 'horizontalDirection' (strictly horizontal).
        /// </summary>
        /// <param name="planePoint">The origin of the plane in world space.</param>
        /// <param name="horizontalDirection">The forward direction of the plane.</param>
        /// <param name="pointToTest">The world position to check.</param>
        /// <returns>True if the point is in front of the plane, false if it is behind (or exactly on it).</returns>
        public static bool IsPointInFrontOfPlane(Vector3 planePoint, Vector3 horizontalDirection, Vector3 pointToTest)
        {
            Vector3 normal = GetHorizontalNormal(horizontalDirection);
            Vector3 directionToPoint = pointToTest - planePoint;
            
            // Positive dot product means the point is in the half-space defined by the normal.
            return Vector3.Dot(directionToPoint, normal) > 0;
        }

        /// <summary>
        /// Returns the signed distance from the plane to the point.
        /// Positive = In Front, Negative = Behind, Zero = On Plane.
        /// </summary>
        public static float GetSignedDistanceToPlane(Vector3 planePoint, Vector3 horizontalDirection, Vector3 pointToTest)
        {
            Vector3 normal = GetHorizontalNormal(horizontalDirection);
            Vector3 directionToPoint = pointToTest - planePoint;
            
            return Vector3.Dot(directionToPoint, normal);
        }

        /// <summary>
        /// Projects a world position onto the vertical plane.
        /// </summary>
        public static Vector3 ProjectPointOntoPlane(Vector3 planePoint, Vector3 horizontalDirection, Vector3 pointToTest)
        {
            Vector3 normal = GetHorizontalNormal(horizontalDirection);
            float distance = GetSignedDistanceToPlane(planePoint, horizontalDirection, pointToTest);
            
            return pointToTest - (normal * distance);
        }

        /// <summary>
        /// Normalizes the direction and removes the Y component to ensure the plane is vertical.
        /// </summary>
        private static Vector3 GetHorizontalNormal(Vector3 direction)
        {
            Vector3 horizontal = new Vector3(direction.x, 0, direction.z);
            
            if (horizontal.sqrMagnitude < 0.0001f)
            {
                // Default to world forward if the provided direction is vertical.
                return Vector3.forward;
            }

            return horizontal.normalized;
        }
    }
}
