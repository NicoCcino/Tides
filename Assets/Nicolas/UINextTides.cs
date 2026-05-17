using UnityEngine;
using TMPro;

public class UINextTides : MonoBehaviour
{

    public TidesManager tidesManager;
    public TMPro.TextMeshProUGUI[] tidesNumberTexts;
    public TMPro.TextMeshProUGUI[] tidesHeightTexts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateNextTides();
    }

    void UpdateNextTides()
    {
        if (tidesManager == null) return;
        float currentTideIndex = tidesManager.currentCycleIndex;
        // Implement logic to update the UI with the next tide information
        for (int i = 0; i < tidesNumberTexts.Length; i++)
        {
            int tideIndex = (int)(currentTideIndex + i);

            tidesNumberTexts[i].text = $"Tide {tideIndex}";
            tidesHeightTexts[i].text = $"Height: {tidesManager.tideCyclesSO.tideCycles[tideIndex].tideCoefficient}";
        }
    }
}
