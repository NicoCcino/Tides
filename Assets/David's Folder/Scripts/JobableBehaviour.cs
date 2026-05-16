using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JobableBehaviour : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] public GameObject JobableControlsGameObject;
    public IJobable Jobable { get; protected set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        JobableControlsGameObject.SetActive(true);
    }

    private void Awake()
    {
        Jobable = GetComponentInParent<IJobable>();
    }
}
