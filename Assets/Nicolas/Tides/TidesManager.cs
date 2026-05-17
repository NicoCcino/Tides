using UnityEngine;

public class TidesManager : Singleton<TidesManager>
{

    public float tideChangeInterval; // Time in seconds between tide changes
    public float tideDurationLow = 20f;
    public float tideDurationHigh = 5f;

    public float tideDurationLowering = 5f;

    public float tideDurationRising = 20f;

    public float tideTimer = 0f;
    public enum TideState { Rising, High, Lowering, Low }
    public TideState currentTide = TideState.Low;
    [SerializeField]
    public int currentCycleIndex = 0;

    public GameObject waterNavBlocker;

    [SerializeField] private Animator waveAnimator;

    public SetWaveShaderVariables setWaveShaderVariables;

    public TideCyclesSO tideCyclesSO;
    public Transform waveTransform;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Starting at TideState.Low.
        tideChangeInterval = tideDurationLow;

        setWaveShaderVariables.waveHeight = tideCyclesSO.tideCycles[currentCycleIndex].WaveHeight;
        setWaveShaderVariables.restHeight = tideCyclesSO.tideCycles[currentCycleIndex].RestHeight;
    }

    // Update is called once per frame
    void Update()
    {
        tideTimer += Time.deltaTime;

        if (tideTimer >= tideChangeInterval)
        {
            tideTimer = 0f;
            ChangeTide();
        }
    }

    void ChangeTide()
    {
        // Implement tide change logic here

        if (currentTide == TideState.Low)
        {
            currentTide = TideState.Rising;
            // Additional logic for rising tide
            waveAnimator.SetBool("GoingUp", true);
            Debug.Log("waveHeight set to " + tideCyclesSO.tideCycles[currentCycleIndex].WaveHeight);
            setWaveShaderVariables.waveHeight = tideCyclesSO.tideCycles[currentCycleIndex].WaveHeight;

            tideChangeInterval = tideDurationRising;

            CameraShake.Instance.ShakeCamera(tideDurationRising, 3);
        }
        else if (currentTide == TideState.Rising)
        {
            currentTide = TideState.High;
            // Additional logic for high tide
            UpdateWaterNavBlocker(setWaveShaderVariables.waveHeight);

            tideChangeInterval = tideDurationHigh;
        }
        else if (currentTide == TideState.High)
        {
            currentTide = TideState.Lowering;
            // Additional logic for lowering tide
            waveAnimator.SetBool("GoingUp", false);
            Debug.Log("restHeight set to " + tideCyclesSO.tideCycles[currentCycleIndex].RestHeight);
            setWaveShaderVariables.restHeight = tideCyclesSO.tideCycles[currentCycleIndex].RestHeight;
            UpdateWaterNavBlocker(setWaveShaderVariables.restHeight);
            SurvivorsController.Instance.AddAgeToAll(1);

            tideChangeInterval = tideDurationLowering;
            CameraShake.Instance.ShakeCamera(tideDurationLowering, 1);
        }
        else if (currentTide == TideState.Lowering)
        {
            currentTide = TideState.Low;
            // Additional logic for low tide
            currentCycleIndex++;

            tideChangeInterval = tideDurationLow;


        }

        Debug.Log("Tide changed to: " + currentTide);

    }

    void UpdateWaterNavBlocker(float waterheight)
    {
        if (waterNavBlocker != null)
        {
            float waterNavBlockerHeight = waterheight + 0.5f; // Adjust the height as needed
            waterNavBlocker.transform.position = new Vector3(waterNavBlocker.transform.position.x, waterNavBlockerHeight, waterNavBlocker.transform.position.z);
            // waterNavBlocker.SetActive(true);

        }
    }
}
