using UnityEngine;
using UnityEngine.EventSystems;
public class HoverShaderSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Material baseMaterial;
    [SerializeField] private Renderer attachedRenderer;
    [SerializeField] private Material hoverMaterial;

    private void Awake()
    {
        baseMaterial = attachedRenderer.sharedMaterial;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        attachedRenderer.material = hoverMaterial;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        attachedRenderer.material = baseMaterial;
    }
}
