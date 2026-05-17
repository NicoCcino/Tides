using UnityEngine;
using Tides.Resources;
using System.Collections.Generic;
using UnityEngine.Playables;



public class TidesManager : Singleton<TidesManager>
{

    public float tideChangeInterval = 30f; // Time in seconds between tide changes
    private float tideTimer = 0f;
    public enum TideState { Rising, High, Lowering, Low }
    public TideState currentTide = TideState.Low;
    [SerializeField]
    public int currentCycleIndex = 0;

    [SerializeField] private Animator waveAnimator;

    public SetWaveShaderVariables setWaveShaderVariables;

    public TideCyclesSO tideCyclesSO;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        }
        else if (currentTide == TideState.Rising)
        {
            currentTide = TideState.High;
            // Additional logic for high tide
        }
        else if (currentTide == TideState.High)
        {
            currentTide = TideState.Lowering;
            // Additional logic for lowering tide
            waveAnimator.SetBool("GoingUp", false);
            Debug.Log("restHeight set to " + tideCyclesSO.tideCycles[currentCycleIndex].RestHeight);
            setWaveShaderVariables.restHeight = tideCyclesSO.tideCycles[currentCycleIndex].RestHeight;
            SurvivorsController.Instance.AddAgeToAll(1);

        }
        else if (currentTide == TideState.Lowering)
        {
            currentTide = TideState.Low;
            // Additional logic for low tide
            currentCycleIndex++;
        }

        Debug.Log("Tide changed to: " + currentTide);

    }



}
