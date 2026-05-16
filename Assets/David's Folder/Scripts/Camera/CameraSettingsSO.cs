using UnityEngine;
namespace Tides.Camera
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Tides/Camera/Settings")]
    public class CameraSettingsSO : ScriptableObject
    {
        [field: SerializeField] public CameraSettings Settings { get; private set; }
    }
}