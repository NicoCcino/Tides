using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverMaterialAdder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Renderer[] targetRenderers;
    [SerializeField] private Material hoverMaterial;

    private Material instantiatedTempMaterial;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AddTemporaryMaterial();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RemoveTemporaryMaterial();
    }

    public void AddTemporaryMaterial()
    {

        if (targetRenderers == null || hoverMaterial == null) return;
        // Create an instance of the material so we don't modify the project asset
        instantiatedTempMaterial = new Material(hoverMaterial);

        foreach (Renderer rend in targetRenderers)
        {
            // Fetch current materials into a list
            List<Material> materials = new List<Material>();
            rend.GetMaterials(materials);

            // Add our new material and apply the list back to the renderer
            materials.Add(instantiatedTempMaterial);
            rend.SetMaterials(materials);
        }
    }

    public void RemoveTemporaryMaterial()
    {
        if (targetRenderers == null || instantiatedTempMaterial == null) return;

        foreach (Renderer rend in targetRenderers)
        {
            List<Material> materials = new List<Material>();
            rend.GetMaterials(materials);

            // Remove the specific instance we added
            if (materials.Remove(materials.Where(m => m.shader == hoverMaterial.shader).First()))
            {
                rend.SetMaterials(materials);
            }
        }
        Destroy(instantiatedTempMaterial);
        instantiatedTempMaterial = null;
    }
}
