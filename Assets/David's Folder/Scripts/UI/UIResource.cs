using TMPro;
using UnityEngine;
using Tides.Resources;
using DG.Tweening;

public class UIResource : MonoBehaviour
{
    [SerializeField] private TMP_Text textAmount;
    [SerializeField] private ResourceType resourceType;
    private IResource trackedResource;

    private Tween currentTween;

    private int lastAmount;

    private void OnEnable()
    {
        switch (resourceType)
        {
            case ResourceType.FOOD:
                trackedResource = ResourcesManager.Instance.FoodResource;
                break;
            case ResourceType.WOOD:
                trackedResource = ResourcesManager.Instance.WoodResource;
                break;
        }

        if (trackedResource != null)
        {
            trackedResource.OnFailedConsumed += HandleFailedConsumed;
            trackedResource.OnAmountChanged += UpdateAmountText;
            lastAmount = trackedResource.GetAmount();
            UpdateAmountText(lastAmount);
        }
    }

    private void OnDisable()
    {
        if (trackedResource != null)
        {
            trackedResource.OnFailedConsumed -= HandleFailedConsumed;
            trackedResource.OnAmountChanged -= UpdateAmountText;
        }
    }

    private void UpdateAmountText(int amount)
    {
        int delta = amount - lastAmount;
        if (delta != 0)
        {
            string sign = delta > 0 ? "+" : "";
            Color color = delta > 0 ? Color.green : Color.red;
            UIPopupTextManager.Instance.SpawnPopup(transform, $"{sign}{delta}", color, delta > 0);
        }

        lastAmount = amount;
        textAmount.text = amount.ToString();
    }

    private void HandleFailedConsumed()
    {
        currentTween?.Kill(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(textAmount.transform.DOPunchScale(Vector3.one * 1, 0.3f));
        sequence.Join(textAmount.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo));

        currentTween = sequence;
    }
}
