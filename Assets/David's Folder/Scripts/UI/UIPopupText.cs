using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIPopupText : MonoBehaviour
{
    [field: SerializeField] public TMP_Text Text { get; private set; }

    public void Play(string value, Color color, Vector3 offset, float duration, float scaleMult)
    {
        Text.text = value;
        Text.color = color;

        Sequence sequence = DOTween.Sequence();
        
        // Move relative to current position
        sequence.Append(transform.DOLocalMove(transform.localPosition + offset, duration).SetEase(Ease.OutQuad));
        
        // Scale
        sequence.Join(transform.DOScale(transform.localScale * scaleMult, duration).SetEase(Ease.OutQuad));
        
        // Fade
        sequence.Join(Text.DOFade(0, duration).SetEase(Ease.InQuad));
        
        sequence.OnComplete(() => Destroy(gameObject));
    }
}
