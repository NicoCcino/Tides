using Tides.Resources;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class BuildableBehaviour : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private int buildingCost = 25;

    [SerializeField] private GameObject buildableGameObject;
    // [SerializeField] private GameObject previewGameObject;
    [SerializeField] private GameObject buildedGameObject;
    [SerializeField] private float buildingTime;
    [SerializeField] private ShaderFloatUpdater visualProgressEffect;
    [SerializeField] private GameObject informationsGameObject;
    [SerializeField] private GameObject buildingControlGameObject;

    [Header("Debug")]
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
        //previewGameObject.SetActive(amount <= 0);
        if (amount >= 1.0f)
        {
            SpawnBuilding();
        }
    }
    private void SpawnBuilding()
    {
        buildedGameObject.SetActive(true);
        buildableGameObject.SetActive(false);
        buildingControlGameObject.SetActive(false);
    }
    public void DestroyBuilding()
    {
        buildedGameObject.SetActive(false);
        buildableGameObject.SetActive(true);
        buildingControlGameObject.SetActive(true);
        assignedBuilding = false;
        informationsGameObject.SetActive(true);
    }
    public void CancelBuilding()
    {
        assignedBuilding = false;
        informationsGameObject.SetActive(true);
        buildingControlGameObject.SetActive(false);
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
        if (ResourcesManager.Instance.TryConsumeWood(buildingCost))
        {
            assignedBuilding = true;
            informationsGameObject.SetActive(false);
            buildingControlGameObject.SetActive(true);
            UpdateProgress(0.0f);
        }
    }

}
public partial class BuildableBehaviour : IJobable
{
    public int AssignedWorkersCount { get; set; }

    public void AddJob()
    {
        JobManager.Instance.PendingJobs.Enqueue(new BuildJob(this));
    }

    public void CancelJobs()
    {
        throw new System.NotImplementedException();
    }

    public void RemoveJob()
    {
        throw new System.NotImplementedException();
    }
}
