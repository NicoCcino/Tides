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
            float screenScale = 1080f / Screen.height;

            // Panning
            if (input.IsPanning)
            {
                float panSpeed = input.IsTouch ? settings.MobilePanSpeed : settings.PanSpeed;
                // Removed deltaTime because PanDelta is already a per-frame delta.
                // Added screenScale for resolution consistency.
                float speedMultiplier = currentHeight * panSpeed * 0.01f * screenScale;
                Vector3 moveDelta = new Vector3(-input.PanDelta.x, 0, -input.PanDelta.y) * speedMultiplier;
                targetPosition += moveDelta;

                ApplyBounds();
            }

            // Edge Scrolling
            if (input.EdgeScrollDelta != Vector2.zero)
            {
                // Edge scroll is speed-based, so it requires deltaTime.
                float speedMultiplier = currentHeight * settings.EdgeScrollSpeed * 0.1f * deltaTime;
                Vector3 moveDelta = new Vector3(input.EdgeScrollDelta.x, 0, input.EdgeScrollDelta.y) * speedMultiplier;
                targetPosition += moveDelta;

                ApplyBounds();
            }

            // Zooming
            if (input.IsZooming)
            {
                float zoomSpeed = input.IsTouch ? settings.MobileZoomSpeed : settings.ZoomSpeed;
                // Removed deltaTime because ZoomDelta is already a per-frame delta.
                currentHeight -= input.ZoomDelta * zoomSpeed * 0.001f * screenScale;
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