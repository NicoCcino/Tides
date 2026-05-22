using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{

    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0, 24)] private float timeOfDay; // 0 to 24]

    [Tooltip("Set to 0 if you want the tides duration to get value from the TidesManager")]
    [SerializeField] private float tideCycleDuration; // Duration of a full tide cycle
    [SerializeField] private float tidesPerDay = 2f; // Number of tide cycles per day


    [Tooltip("Multiplier to speed up or slow down time. Default speed is 1 second in game = 1 hour in real time")]

    [SerializeField] private float timeMultiplier = 1f; // Multiplier to speed up or slow down time

    private void OnValidate()
    {
        if (directionalLight != null)
            return;
        if (UnityEngine.RenderSettings.sun != null)
        {
            directionalLight = UnityEngine.RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == UnityEngine.LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }

    }

    private void Start()
    {
        if (tideCycleDuration == 0f)
        {

            if (TidesManager.Instance != null)
            {
                tideCycleDuration = TidesManager.Instance.cycleDuration;
            }
        }
    }

    private void UpdateLighting(float timePercent)
    {
        UnityEngine.RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        UnityEngine.RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);
        if (directionalLight != null)
        {
            directionalLight.color = preset.directionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (preset == null)
            return;
        if (Application.isPlaying)
        {
            if (IntroCutscene.Instance.stopped == false) return;
            timeOfDay += (Time.deltaTime / tideCycleDuration / tidesPerDay) * 24f * timeMultiplier; // Increment time of day based on tides duration and multiplier
            timeOfDay %= 24f; // Wrap around to stay within 0-24
        }
        UpdateLighting(timeOfDay / 24f); // Update lighting based on time of day percentage

    }
}
