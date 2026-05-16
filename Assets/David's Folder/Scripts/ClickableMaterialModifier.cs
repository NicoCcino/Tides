
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableMaterialModifier : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Renderer[] targetRenderers;


    [SerializeField] private string colorPropertyName;
    [SerializeField] private Color clickedColor;

    private MaterialPropertyBlock propBlock;
    private void Start()
    {
        propBlock = new MaterialPropertyBlock();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Renderer rend in targetRenderers)
        {
            // Get the current properties
            rend.GetPropertyBlock(propBlock);

            // Set the temporary color
            propBlock.SetColor(colorPropertyName, clickedColor);

            // Apply the block back to the renderer
            rend.SetPropertyBlock(propBlock);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (Renderer rend in targetRenderers)
        {
            rend.SetPropertyBlock(null);
        }
    }
}
