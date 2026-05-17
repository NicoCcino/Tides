using Tides.Resources;
using TMPro;
using UnityEngine;

public class UIFoodConsumption : MonoBehaviour
{
    [SerializeField] private TMP_Text textNextConsumptionAmount;

    private void FixedUpdate()
    {
        textNextConsumptionAmount.text = "-" + (SurvivorsController.Instance.Survivors.Count * ResourcesManager.Instance.FoodConsumptionPerSurvivor);
    }
}
