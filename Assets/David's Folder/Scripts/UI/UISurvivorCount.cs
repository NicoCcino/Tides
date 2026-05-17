using TMPro;
using UnityEngine;

public class UISurvivorCount : MonoBehaviour
{
    [SerializeField] private TMP_Text textSurvivorAmount;
    private int lastCount;
    private void FixedUpdate()
    {
        int survivorsCount = SurvivorsController.Instance.Survivors.Count;
        textSurvivorAmount.text = survivorsCount.ToString();
        if (lastCount != survivorsCount)
        {
            int diff = lastCount - survivorsCount;
            if (diff > 0)
            {
                UIPopupTextManager.Instance.SpawnPopup(textSurvivorAmount.transform, diff.ToString(), Color.red);
            }
            else
            {
                UIPopupTextManager.Instance.SpawnPopup(textSurvivorAmount.transform, "+" + Mathf.Abs(diff).ToString(), Color.green);
            }
            lastCount = survivorsCount;
        }
    }
}
