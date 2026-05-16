using UnityEngine;

public class UIPopupTextManager : Singleton<UIPopupTextManager>
{
    [field: SerializeField] public UIPopupText PopupPrefab { get; private set; }
    [field: SerializeField] public float DefaultDuration { get; private set; } = 1.0f;
    [field: SerializeField] public float DefaultScaleMult { get; private set; } = 1.2f;
    [field: SerializeField] public float DefaultOffsetAmount { get; private set; } = 50.0f;

    public void SpawnPopup(Transform parent, string value, Color color, bool moveUp = true)
    {
        if (PopupPrefab == null)
        {
            Debug.LogWarning("UIPopupTextManager: PopupPrefab is not assigned!");
            return;
        }

        UIPopupText popup = Instantiate(PopupPrefab, parent);
        popup.transform.localPosition = new Vector3(Random.Range(-20f, 20f), 0, 0);
        
        Vector3 offset = moveUp ? Vector3.up * DefaultOffsetAmount : Vector3.down * DefaultOffsetAmount;
        popup.Play(value, color, offset, DefaultDuration, DefaultScaleMult);
    }
}
