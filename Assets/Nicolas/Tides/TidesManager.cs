using UnityEngine;
using Tides.Resources;
using System.Collections.Generic;


public class TidesManager : Singleton<TidesManager>
{

    public float tideChangeInterval = 30f; // Time in seconds between tide changes
    private float tideTimer = 0f;
    public enum TideState { Rising, High, Lowering, Low }
    public TideState currentTide = TideState.Low;
    [SerializeField]
    private List<TideCycle> tideCycles = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
            // Additional logic for raising tide
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
            SurvivorsController.Instance.AddAgeToAll(1);

        }
        else if (currentTide == TideState.Lowering)
        {
            currentTide = TideState.Low;
            // Additional logic for low tide
        }

        Debug.Log("Tide changed to: " + currentTide);

    }



}
