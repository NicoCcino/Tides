using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Tides.Camera
{
    public class CameraInputProvider : ICameraInput
    {
        private readonly CameraSettings settings;
        private bool isDragging;

        public Vector2 PanDelta { get; private set; }
        public float ZoomDelta { get; private set; }
        public Vector2 EdgeScrollDelta { get; private set; }

        // Now checks for exactly 1 active touch to count as touch-panning
        public bool IsPanning => isDragging || Touch.activeTouches.Count == 1;
        public bool IsZooming { get; private set; }
        public bool IsTouch { get; private set; }

        public CameraInputProvider(CameraSettings settings)
        {
            this.settings = settings;
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
            
            if (!IsTouch && settings.UseEdgeScrolling)
            {
                HandleEdgeScrolling();
            }
            else
            {
                EdgeScrollDelta = Vector2.zero;
            }
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
                IsTouch = false;
            }
            else
            {
                PanDelta = Vector2.zero;
            }

            // Zoom
            float scroll = mouse.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.001f)
            {
                ZoomDelta = scroll;
                IsZooming = true;
                IsTouch = false;
            }
            else if (!IsTouch)
            {
                ZoomDelta = 0;
                IsZooming = false;
            }
        }

        private void HandleTouchInput()
        {
            // If there are no active touches on the screen, exit early 
            if (Touch.activeTouches.Count == 0) return;

            IsTouch = true;

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

        private void HandleEdgeScrolling()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            Vector2 mousePos = mouse.position.ReadValue();
            Vector3 delta = Vector2.zero;

            if (mousePos.x < settings.EdgeScrollSize) delta.x = -1;
            else if (mousePos.x > Screen.width - settings.EdgeScrollSize) delta.x = 1;

            if (mousePos.y < settings.EdgeScrollSize) delta.y = -1;
            else if (mousePos.y > Screen.height - settings.EdgeScrollSize) delta.y = 1;

            EdgeScrollDelta = delta;
        }
    }
}