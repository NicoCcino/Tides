using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class UIPopupText : MonoBehaviour
{
    [field: SerializeField] public TMP_Text Text { get; private set; }
    private Action<UIPopupText> returnToPool;

    public void Init(Action<UIPopupText> onRelease)
    {
        returnToPool = onRelease;
    }

    public void Play(string value, Color color, Vector3 offset, float duration, float scaleMult)
    {
        Text.text = value;
        Text.color = color;
        Text.alpha = 1; // Ensure it's visible if reused
        transform.localScale = Vector3.one; // Reset scale

        Sequence sequence = DOTween.Sequence();
        
        // Move relative to current position
        if (transform is RectTransform rectTransform)
        {
            sequence.Append(rectTransform.DOAnchorPos(rectTransform.anchoredPosition + (Vector2)offset, duration).SetEase(Ease.OutQuad));
        }
        else
        {
            sequence.Append(transform.DOLocalMove(transform.localPosition + offset, duration).SetEase(Ease.OutQuad));
        }
        
        // Scale
        sequence.Join(transform.DOScale(transform.localScale * scaleMult, duration).SetEase(Ease.OutQuad));
        
        // Fade
        sequence.Join(Text.DOFade(0, duration).SetEase(Ease.InQuad));
        
        sequence.OnComplete(() => returnToPool?.Invoke(this));
    }
}
