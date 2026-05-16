using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Pool;

public class UIPopupTextManager : Singleton<UIPopupTextManager>
{
    [Header("World Space Settings")]
    [field: FormerlySerializedAs("PopupPrefab")]
    [field: SerializeField] public UIPopupText WorldPopupPrefab { get; private set; }
    
    [field: FormerlySerializedAs("DefaultOffsetAmount")]
    [field: SerializeField] public float WorldOffsetAmount { get; private set; } = 1.0f;
    
    [field: SerializeField] public float WorldRandomXRange { get; private set; } = 0.5f;

    [Header("UI Space Settings")]
    [field: SerializeField] public UIPopupText UIPopupPrefab { get; private set; }
    
    [field: SerializeField] public float UIOffsetAmount { get; private set; } = 50.0f;
    
    [field: SerializeField] public float UIRandomXRange { get; private set; } = 20.0f;

    [Header("Shared Settings")]
    [field: SerializeField] public float DefaultDuration { get; private set; } = 1.0f;
    [field: SerializeField] public float DefaultScaleMult { get; private set; } = 1.2f;

    private IObjectPool<UIPopupText> worldPool;
    private IObjectPool<UIPopupText> uiPool;

    protected override void Awake()
    {
        base.Awake();
        InitializePools();
    }

    private void InitializePools()
    {
        worldPool = new ObjectPool<UIPopupText>(
            createFunc: () => CreatePopup(WorldPopupPrefab, worldPool),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        uiPool = new ObjectPool<UIPopupText>(
            createFunc: () => CreatePopup(UIPopupPrefab, uiPool),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );
    }

    private UIPopupText CreatePopup(UIPopupText prefab, IObjectPool<UIPopupText> pool)
    {
        if (prefab == null) return null;
        UIPopupText popup = Instantiate(prefab, transform);
        popup.Init(pool.Release);
        return popup;
    }

    public void SpawnPopup(Transform parent, string value, Color color, bool moveUp = true)
    {
        bool isUI = parent is RectTransform;
        IObjectPool<UIPopupText> pool = isUI ? uiPool : worldPool;
        UIPopupText prefab = isUI ? UIPopupPrefab : WorldPopupPrefab;

        if (prefab == null)
        {
            Debug.LogWarning($"UIPopupTextManager: {(isUI ? "UIPopupPrefab" : "WorldPopupPrefab")} is not assigned!");
            return;
        }

        UIPopupText popup = pool.Get();
        if (popup == null) return;

        popup.transform.SetParent(parent, false);
        
        float randomXRange = isUI ? UIRandomXRange : WorldRandomXRange;
        float offsetAmount = isUI ? UIOffsetAmount : WorldOffsetAmount;

        if (isUI && popup.transform is RectTransform rect)
        {
            rect.anchoredPosition = new Vector2(Random.Range(-randomXRange, randomXRange), 0);
        }
        else
        {
            popup.transform.localPosition = new Vector3(Random.Range(-randomXRange, randomXRange), 0, 0);
        }
        
        Vector3 offset = moveUp ? Vector3.up * offsetAmount : Vector3.down * offsetAmount;
        popup.Play(value, color, offset, DefaultDuration, DefaultScaleMult);
    }
}
