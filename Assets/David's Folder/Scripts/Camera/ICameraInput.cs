using UnityEngine;

namespace Tides.Camera
{
    public interface ICameraInput
    {
        Vector2 PanDelta { get; }
        float ZoomDelta { get; }
        bool IsPanning { get; }
        bool IsZooming { get; }
    }
}
