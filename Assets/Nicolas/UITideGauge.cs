using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITideGauge : MonoBehaviour
{
    public TidesManager tidesManager;
    public Slider slider;
    public TextMeshProUGUI tideStateText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateGauge();
    }

    void UpdateGauge()
    {
        if (tidesManager == null) return;

        // Implement logic to update the UI gauge based on the current tide state
        switch (tidesManager.currentTide)
        {
            case TidesManager.TideState.Rising:
                // Update gauge for rising tide
                tideStateText.text = "Rising Tide";
                break;
            case TidesManager.TideState.High:
                // Update gauge for high tide
                tideStateText.text = "High Tide";
                break;
            case TidesManager.TideState.Lowering:
                // Update gauge for lowering tide
                tideStateText.text = "Falling Tide";
                break;
            case TidesManager.TideState.Low:
                // Update gauge for low tide
                tideStateText.text = "Low Tide";
                break;
        }

        // Example: Update slider value based on tide state
        slider.value = tidesManager.tideTimer / tidesManager.tideChangeInterval;
    }
}
