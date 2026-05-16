using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Tides.Camera
{
    public class CameraInputProvider : ICameraInput
    {
        private bool isDragging;

        public Vector2 PanDelta { get; private set; }
        public float ZoomDelta { get; private set; }

        // Now checks for exactly 1 active touch to count as touch-panning
        public bool IsPanning => isDragging || Touch.activeTouches.Count == 1;
        public bool IsZooming { get; private set; }

        public CameraInputProvider()
        {
            // EnhancedTouch must be enabled before it can track active touches
            if (!EnhancedTouchSupport.enabled)
            {
                EnhancedTouchSupport.Enable();
            }
        }

        public void Update()
        {
            HandleMouseInput();
            HandleTouchInput();
        }

        private void HandleMouseInput()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            // Pan
            isDragging = mouse.leftButton.isPressed;
            if (isDragging)
            {
                PanDelta = mouse.delta.ReadValue();
            }
            else
            {
                PanDelta = Vector2.zero;
            }

            // Zoom
            ZoomDelta = mouse.scroll.ReadValue().y;
            IsZooming = Mathf.Abs(ZoomDelta) > 0.001f;
        }

        private void HandleTouchInput()
        {
            // If there are no active touches on the screen, exit early 
            // so we don't accidentally overwrite the mouse inputs.
            if (Touch.activeTouches.Count == 0) return;

            if (Touch.activeTouches.Count == 1)
            {
                var touch = Touch.activeTouches[0];
                PanDelta = touch.delta; // EnhancedTouch provides direct delta access
                IsZooming = false;
                ZoomDelta = 0;
            }
            else if (Touch.activeTouches.Count >= 2)
            {
                var touch0 = Touch.activeTouches[0];
                var touch1 = Touch.activeTouches[1];

                Vector2 pos0 = touch0.screenPosition;
                Vector2 pos1 = touch1.screenPosition;

                Vector2 prevPos0 = pos0 - touch0.delta;
                Vector2 prevPos1 = pos1 - touch1.delta;

                float currentDist = Vector2.Distance(pos0, pos1);
                float prevDist = Vector2.Distance(prevPos0, prevPos1);

                ZoomDelta = currentDist - prevDist;
                IsZooming = true;
                PanDelta = Vector2.zero;
            }
        }
    }
}