using UnityEngine;
using UnityEngine.InputSystem;

namespace Tides.Camera
{
    public class SimpleCameraController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float panSpeed = 1.0f;
        [SerializeField] private float zoomSpeed = 5.0f;
        [SerializeField] private float smoothing = 10.0f;

        [Header("Height & Tilt")]
        [SerializeField] private float minHeight = 5.0f;
        [SerializeField] private float maxHeight = 50.0f;
        [SerializeField] private float tiltAngle = 45.0f;

        private Vector3 targetPosition;
        private float currentHeight;

        private void Start()
        {
            targetPosition = transform.position;
            currentHeight = targetPosition.y;
            
            // Initial tilt
            transform.rotation = Quaternion.Euler(tiltAngle, 0, 0);
        }

        private void Update()
        {
            HandleInput();
            ApplyMovement();
        }

        private void HandleInput()
        {
            // Mouse Panning
            var mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.isPressed)
            {
                Vector2 delta = mouse.delta.ReadValue();
                if (delta.sqrMagnitude > 0.01f)
                {
                    float multiplier = currentHeight * 0.05f * panSpeed;
                    targetPosition.x -= delta.x * multiplier * Time.deltaTime * 10f;
                    targetPosition.z -= delta.y * multiplier * Time.deltaTime * 10f;
                }
            }

            // Mouse Zoom
            if (mouse != null)
            {
                float scroll = mouse.scroll.ReadValue().y;
                if (Mathf.Abs(scroll) > 0.01f)
                {
                    currentHeight -= scroll * zoomSpeed * Time.deltaTime * 0.1f;
                }
            }

            // Touch Support
            var touch = Touchscreen.current;
            if (touch != null && touch.touches.Count > 0)
            {
                if (touch.touches.Count == 1)
                {
                    Vector2 delta = touch.touches[0].delta.ReadValue();
                    float multiplier = currentHeight * 0.05f * panSpeed;
                    targetPosition.x -= delta.x * multiplier * Time.deltaTime * 10f;
                    targetPosition.z -= delta.y * multiplier * Time.deltaTime * 10f;
                }
                else if (touch.touches.Count >= 2)
                {
                    var t0 = touch.touches[0];
                    var t1 = touch.touches[1];
                    float currentDist = Vector2.Distance(t0.position.ReadValue(), t1.position.ReadValue());
                    float prevDist = Vector2.Distance(t0.position.ReadValue() - t0.delta.ReadValue(), t1.position.ReadValue() - t1.delta.ReadValue());
                    float deltaDist = currentDist - prevDist;
                    currentHeight -= deltaDist * zoomSpeed * Time.deltaTime * 0.1f;
                }
            }

            currentHeight = Mathf.Clamp(currentHeight, minHeight, maxHeight);
            targetPosition.y = currentHeight;
        }

        private void ApplyMovement()
        {
            float lerpFactor = 1f - Mathf.Exp(-smoothing * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpFactor);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(tiltAngle, 0, 0), lerpFactor);
        }
    }
}
