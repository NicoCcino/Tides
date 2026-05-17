using UnityEngine;
using TMPro;

public class UINextTides : MonoBehaviour
{

    public TidesManager tidesManager;
    public TMPro.TextMeshProUGUI[] tidesNumberTexts;
    public TMPro.TextMeshProUGUI[] tidesRemainingTimeTexts;
    public TMPro.TextMeshProUGUI[] tidesHeightTexts;

    [SerializeField]
    Color lightBlue = new Color(0.55f, 0.8f, 0.95f);

    [SerializeField]
    Color darkBlue = new Color(0.02f, 0.15f, 0.35f);

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

            tidesNumberTexts[i].text = $"Tide {tidesManager.GetHighTidesSurvived() + i + 1}";

            float timeRemainingBeforeHighTide = tidesManager.GetTimeRemainingBeforeHighTide() + (tidesManager.cycleDuration * i);
            tidesRemainingTimeTexts[i].text = $"in {Mathf.Max(0, timeRemainingBeforeHighTide):F0} seconds";

            tidesHeightTexts[i].text = $"Height: {tidesManager.tideCyclesSO.tideCycles[tideIndex].tideCoefficient}";

            Color newColor = Color.Lerp(lightBlue, darkBlue, tidesManager.tideCyclesSO.tideCycles[tideIndex].tideCoefficient / 4f);

            tidesHeightTexts[i].color = newColor;
        }
    }
}
