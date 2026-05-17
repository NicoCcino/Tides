using UnityEngine;

namespace Tides.Camera
{
    public class CameraLogic
    {
        private readonly CameraSettings settings;
        private Vector3 targetPosition;
        private float currentHeight;

        public Vector3 CurrentPosition { get; private set; }
        public Quaternion CurrentRotation { get; private set; }

        public CameraLogic(CameraSettings settings, Vector3 initialPosition)
        {
            this.settings = settings;
            targetPosition = initialPosition;
            currentHeight = initialPosition.y;

            ApplyBounds();

            CurrentPosition = targetPosition;
            CurrentRotation = Quaternion.Euler(this.settings.TiltAngle, 0, 0);
        }

        public void CalculateNextState(ICameraInput input, float deltaTime)
        {
            // Panning
            if (input.IsPanning)
            {
                // Restored the 0.5f scaling and deltaTime from your original SimpleCameraController
                float speedMultiplier = currentHeight * settings.PanSpeed * 0.5f * deltaTime;
                Vector3 moveDelta = new Vector3(-input.PanDelta.x, 0, -input.PanDelta.y) * speedMultiplier;
                targetPosition += moveDelta;

                ApplyBounds();
            }

            // Zooming
            if (input.IsZooming)
            {
                // Restored the 0.1f scaling and deltaTime from your original SimpleCameraController
                currentHeight -= input.ZoomDelta * settings.ZoomSpeed * 0.1f * deltaTime;
                currentHeight = Mathf.Clamp(currentHeight, settings.MinHeight, settings.MaxHeight);
                targetPosition.y = currentHeight;
            }

            // Smoothing
            float lerpFactor = 1f - Mathf.Exp(-settings.Smoothing * deltaTime);
            CurrentPosition = Vector3.Lerp(CurrentPosition, targetPosition, lerpFactor);
            CurrentRotation = Quaternion.Slerp(CurrentRotation, Quaternion.Euler(settings.TiltAngle, 0, 0), lerpFactor);
        }

        private void ApplyBounds()
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, settings.MinBounds.x, settings.MaxBounds.x);
            targetPosition.z = Mathf.Clamp(targetPosition.z, settings.MinBounds.y, settings.MaxBounds.y);
        }
    }
}