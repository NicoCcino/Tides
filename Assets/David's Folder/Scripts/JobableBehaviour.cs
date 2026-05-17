using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JobableBehaviour : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool ControlsUIDisplayal = true;
    [SerializeField] public GameObject JobableControlsGameObject;
    public IJobProvider Jobable { get; protected set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!ControlsUIDisplayal) return;
        JobableControlsGameObject.SetActive(true);
    }

    private void Awake()
    {
        Jobable = GetComponentInParent<IJobProvider>();
    }
}
