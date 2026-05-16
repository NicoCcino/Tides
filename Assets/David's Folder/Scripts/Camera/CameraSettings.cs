using System;
using UnityEngine;

namespace Tides.Camera
{
    [Serializable]
    public class CameraSettings
    {
        [field: SerializeField] public float PanSpeed { get; private set; } = 0.5f;
        [field: SerializeField] public float ZoomSpeed { get; private set; } = 5f;
        [field: SerializeField] public float MinHeight { get; private set; } = 5f;
        [field: SerializeField] public float MaxHeight { get; private set; } = 50f;
        [field: SerializeField] public float DefaultHeight { get; private set; } = 20f;
        [field: SerializeField] public float TiltAngle { get; private set; } = 45f;
        [field: SerializeField] public float Smoothing { get; private set; } = 10f;
    }

}
