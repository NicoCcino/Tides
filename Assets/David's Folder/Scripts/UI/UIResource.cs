using TMPro;
using UnityEngine;
using Tides.Resources;
public class UIResource : MonoBehaviour
{
    [SerializeField] private TMP_Text textAmount;
    [SerializeField] private ResourceType resourceType;
    private IResource trackedResource;
    private void OnEnable()
    {
        switch (resourceType)
        {
            case ResourceType.FOOD:
                trackedResource = BaseManager.Instance.FoodResource;
                break;
            case ResourceType.WOOD:
                trackedResource = BaseManager.Instance.WoodResource;
                break;
        }

    }
    private void Update()
    {
        textAmount.text = trackedResource.GetAmount().ToString();
    }
}
