using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIStaminaGauge : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI staminaText;
    public SurvivorController survivorController;


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
        if (slider != null && survivorController != null)
        {
            slider.value = survivorController.GetStaminaPercentage();
            staminaText.text = $"{Mathf.RoundToInt(survivorController.GetStaminaPercentage() * 100)}/100";
        }

    }
}
