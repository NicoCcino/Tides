using Tides.Resources;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class TidesManager : Singleton<TidesManager>
{

    public float tideChangeInterval; // Time in seconds between tide changes
    public float tideDurationLow = 20f;
    public float tideDurationHigh = 5f;

    public float tideDurationLowering = 5f;

    public float tideDurationRising = 20f;

    public float cycleDuration; // Total duration of a tide cycle (rising + high + lowering + low)

    public float tideTimer = 0f;
    public enum TideState { Rising, High, Lowering, Low }
    public TideState currentTide = TideState.Low;
    [SerializeField]
    public int currentCycleIndex = 0;

    public GameObject waterNavBlocker;
    public NavMeshSurface navMeshSurface;

    [SerializeField] private Animator waveAnimator;

    public SetWaveShaderVariables setWaveShaderVariables;

    public TideCyclesSO tideCyclesSO;
    public Transform waveTransform;

    public Transform waveRandomOrient;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cycleDuration = tideDurationLow + tideDurationRising + tideDurationHigh + tideDurationLowering;

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
            // Additional logic when starting rising tide

            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            waveRandomOrient.rotation = randomRotation;

            waveAnimator.SetBool("GoingUp", true);
            Debug.Log("waveHeight set to " + tideCyclesSO.tideCycles[currentCycleIndex].WaveHeight);
            setWaveShaderVariables.waveHeight = tideCyclesSO.tideCycles[currentCycleIndex].WaveHeight;
            tideChangeInterval = tideDurationRising;

            CameraShake.Instance.ShakeCamera(tideDurationRising, 3);
        }
        else if (currentTide == TideState.Rising)
        {
            currentTide = TideState.High;
            // Additional logic when starting high tide
            UpdateWaterNavBlocker(setWaveShaderVariables.waveHeight);

            tideChangeInterval = tideDurationHigh;

            foreach (SurvivorController survivor in SurvivorsController.Instance.Survivors)
            {
                if (!ResourcesManager.Instance.TryConsumeFood(ResourcesManager.Instance.FoodConsumptionPerSurvivor))
                {
                    survivor.Die();
                }
                else
                {
                    survivor.survivorStateManager.ChangeState(ESurvivorState.Eating);
                }
            }
        }
        else if (currentTide == TideState.High)
        {
            currentTide = TideState.Lowering;
            // Additional logic when starting lowering tide
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
            // Additional logic when starting low tide

            // cycle index is increased when the tide is low.
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

    private float GetTimeRemainingInCurrentTide()
    {
        float timeRemaining = tideChangeInterval - tideTimer;
        //Debug.Log("Time remaining in current tide: " + timeRemaining + " seconds");
        return timeRemaining;
    }

    public float GetTimeRemainingBeforeRisingTide()
    {
        float timeRemaining = 0f;

        switch (currentTide)
        {
            case TideState.Rising:
                timeRemaining = tideDurationRising + tideDurationHigh + tideDurationLowering + tideDurationLow - tideTimer;
                break;
            case TideState.High:
                timeRemaining = tideDurationHigh + tideDurationLowering + tideDurationLow - tideTimer;
                break;
            case TideState.Lowering:
                timeRemaining = tideDurationLowering + tideDurationLow - tideTimer;
                break;
            case TideState.Low:
                timeRemaining = tideDurationLow - tideTimer;
                break;
        }

        //Debug.Log("Time remaining before rising tide: " + timeRemaining + " seconds");
        return timeRemaining;
    }

    public int GetHighTidesStartedCount()
    {
        int highTideCycleIndex = currentCycleIndex;

        if (currentTide == TideState.Rising)
        {
            highTideCycleIndex = currentCycleIndex + 1;
        }
        else if (currentTide == TideState.High)
        {
            highTideCycleIndex = currentCycleIndex + 1;
        }
        else if (currentTide == TideState.Lowering)
        {
            highTideCycleIndex = (currentCycleIndex + 1);
        }
        else if (currentTide == TideState.Low)
        {
            highTideCycleIndex = currentCycleIndex;
        }

        return highTideCycleIndex;
    }
}
