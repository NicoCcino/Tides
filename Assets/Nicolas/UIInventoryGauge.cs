using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryGauge : MonoBehaviour
{
    public Slider slider;
    public SurvivorController survivorController;
    public Image resourceIcon;
    public Sprite foodIcon;
    public Sprite woodIcon;
    public Sprite defaultIcon;
    public TextMeshProUGUI loadText;



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
            switch (survivorController.resourceInInventory)
            {
                case FoodResource foodResource:
                    resourceIcon.sprite = foodIcon;
                    break;
                case WoodResource woodResource:
                    resourceIcon.sprite = woodIcon;
                    break;
                default:
                    resourceIcon.sprite = defaultIcon;
                    break;
            }
            if (survivorController.resourceInInventory == null)
            {
                slider.value = 0;
                loadText.text = $"0/{survivorController.maxLoad}";
                return;
            }
            else
            {
                slider.value = (float)survivorController.resourceInInventory.GetAmount() / survivorController.maxLoad;
                loadText.text = $"{survivorController.resourceInInventory.GetAmount()}/{survivorController.maxLoad}";

            }

        }
    }
}
