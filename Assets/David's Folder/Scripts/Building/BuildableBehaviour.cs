using Tides.Resources;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildableBehaviour : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private int buildingCost = 25;

    [SerializeField] private GameObject buildableGameObject;
    [SerializeField] private GameObject previewGameObject;
    [SerializeField] private GameObject buildedGameObject;
    [SerializeField] private float buildingTime;
    [SerializeField] private ShaderFloatUpdater visualProgressEffect;

    [SerializeField, Range(0, 1)] private float buildingProgress = 0.0f;

    private bool assignedBuilding = false;

    public void TickUpdateProgress()
    {
        buildingProgress += (1 / buildingTime);
        UpdateProgress(buildingProgress);
    }

    private void UpdateProgress(float amount)
    {
        visualProgressEffect.UpdateVisual(amount);
        previewGameObject.SetActive(amount <= 0);
        if (amount >= 1.0f)
        {
            SpawnBuilding();
        }
    }
    private void SpawnBuilding()
    {
        buildedGameObject.SetActive(true);
        buildableGameObject.SetActive(false);
    }
    public void DestroyBuilding()
    {
        buildedGameObject.SetActive(false);
        buildableGameObject.SetActive(true);
        assignedBuilding = false;
    }
    public void CancelBuilding()
    {
        assignedBuilding = false;
        ResourcesManager.Instance.AddWood(buildingCost);
        UpdateProgress(0f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (assignedBuilding == true)
        {
            //DEBUG ONLY
            TickUpdateProgress();
            //
            return;
        }
        if ((ResourcesManager.Instance.WoodResource as IResource).CanConsume(buildingCost))
        {
            ResourcesManager.Instance.ConsumeWood(buildingCost);
            assignedBuilding = true;
        }
    }
}
