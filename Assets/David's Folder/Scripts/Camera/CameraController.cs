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
    }
}
