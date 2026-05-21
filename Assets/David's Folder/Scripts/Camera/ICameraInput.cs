using UnityEngine;

namespace Tides.Camera
{
    public interface ICameraInput
    {
        Vector2 PanDelta { get; }
        float ZoomDelta { get; }
        Vector2 EdgeScrollDelta { get; }
        bool IsPanning { get; }
        bool IsZooming { get; }
        bool IsTouch { get; }
    }
}
