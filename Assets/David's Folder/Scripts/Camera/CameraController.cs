using UnityEngine;

namespace Tides.Camera
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField] public CameraSettingsSO SettingsAsset { get; private set; }

        private CameraInputProvider inputProvider;
        private CameraLogic logic;
        private Transform cameraTransform;

        private void Awake()
        {
            if (SettingsAsset == null)
            {
                Debug.LogError("CameraSettingsSO is missing from CameraController!");
                enabled = false;
                return;
            }

            cameraTransform = transform;
            inputProvider = new CameraInputProvider();
            logic = new CameraLogic(SettingsAsset.Settings, cameraTransform.position);
        }

        private void Update()
        {
            inputProvider.Update();
            logic.CalculateNextState(inputProvider, Time.deltaTime);

            cameraTransform.position = logic.CurrentPosition;
            cameraTransform.rotation = logic.CurrentRotation;
        }

        private void OnDrawGizmosSelected()
        {
            if (SettingsAsset == null || SettingsAsset.Settings == null) return;

            Gizmos.color = Color.cyan;
            
            Vector2 min = SettingsAsset.Settings.MinBounds;
            Vector2 max = SettingsAsset.Settings.MaxBounds;

            Vector3 topLeft = new Vector3(min.x, 0, max.y);
            Vector3 topRight = new Vector3(max.x, 0, max.y);
            Vector3 bottomLeft = new Vector3(min.x, 0, min.y);
            Vector3 bottomRight = new Vector3(max.x, 0, min.y);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}
