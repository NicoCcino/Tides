using System.Collections.Generic;
using System.Linq;
using David.Utils;
using Tides.Resources;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class BuildableBehaviour : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private int buildingCost = 25;
    [SerializeField] private Transform worldUiTransform;
    [SerializeField] private GameObject buildableGameObject;
    // [SerializeField] private GameObject previewGameObject;
    [SerializeField] private GameObject buildedGameObject;
    [SerializeField] private float buildingTime;
    [SerializeField] private ShaderFloatUpdater visualProgressEffect;
    [SerializeField] private GameObject informationsGameObject;
    [SerializeField] private GameObject buildingControlGameObject;

    [Header("Debug")]
    [SerializeField, Range(0, 1)] public float BuildingProgress = 0.0f;

    private bool assignedBuilding = false;
    public AudioSource AudioSource;
    public AudioClip BuildEndClip;
    public AudioClip DestroyClip;

    public void TickUpdateProgress()
    {
        Debug.Log("TickUpdateProgress called. Current progress: " + BuildingProgress);
        BuildingProgress += (1 / buildingTime) * Time.deltaTime;
        UpdateProgress(BuildingProgress);
    }

    private void UpdateProgress(float amount)
    {
        visualProgressEffect.UpdateVisual(amount);
        //previewGameObject.SetActive(amount <= 0);
        BuildingProgress = amount;
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
        assignedBuilding = false;
        GetComponent<Collider>().enabled = false;
        if (AudioSource != null && BuildEndClip != null)
        {
            AudioSource.clip = BuildEndClip;
            AudioSource.Play();
        }
    }
    public void DestroyBuilding()
    {
        if (BuildingProgress < 1.0f && assignedBuilding)
        {
            ResourcesManager.Instance.AddWood(buildingCost);
        }
        CancelJobs();
        buildedGameObject.SetActive(false);
        buildableGameObject.SetActive(true);
        buildingControlGameObject.SetActive(false);
        assignedBuilding = false;
        informationsGameObject.SetActive(true);
        BuildingProgress = 0;
        UpdateProgress(0.0f);
        if (AudioSource != null && DestroyClip != null)
        {
            AudioSource.clip = DestroyClip;
            AudioSource.Play();
        }
        GetComponent<Collider>().enabled = true;
    }
    public void CancelBuilding()
    {
        assignedBuilding = false;
        informationsGameObject.SetActive(true);
        buildingControlGameObject.SetActive(false);
        ResourcesManager.Instance.AddWood(buildingCost);
        UIPopupTextManager.Instance.SpawnPopup(worldUiTransform, "+" + buildingCost.ToString(), Color.green, true);
        //CancelJobs();
        UpdateProgress(0f);
        GetComponent<Collider>().enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (assignedBuilding == true || BuildingProgress >= 1.0f)
        {
            return;
        }
        if (ResourcesManager.Instance.TryConsumeWood(buildingCost))
        {
            assignedBuilding = true;
            informationsGameObject.SetActive(false);
            buildingControlGameObject.SetActive(true);
            UpdateProgress(0.0f);
            UIPopupTextManager.Instance.SpawnPopup(worldUiTransform, "-" + buildingCost.ToString(), Color.red, false);
            GetComponent<Collider>().enabled = false;
        }
    }
    private void FixedUpdate()
    {
        if (BuildingProgress <= 0.0f && !assignedBuilding) return;
        if (TidesManager.Instance.currentTide == TidesManager.TideState.Rising)
        {
            Transform waveTransform = TidesManager.Instance.waveTransform;
            if (!PlaneProjectionHelper.IsPointInFrontOfPlane(waveTransform.position, waveTransform.forward, transform.position) && transform.position.y <= TidesManager.Instance.tideCyclesSO.tideCycles[TidesManager.Instance.currentCycleIndex].WaveHeight)
            {
                Debug.Log("DESTROY BUILDING");
                DestroyBuilding();
            }
        }
    }

}
public partial class BuildableBehaviour : IJobProvider
{
    public int AssignedWorkersCount { get; set; }

    public Vector3 JobLocation => transform.position;

    public void AddJob()
    {
        JobManager.Instance.PendingJobs.Enqueue(new BuildJob(this));
        AssignedWorkersCount++;
    }

    public void CancelJobs()
    {
        CancelBuilding();
        List<SurvivorController> survivorControllers = SurvivorsController.Instance.Survivors.Where(s => s.currentJob != null && s.currentJob.GetType() == typeof(BuildJob) && (Object)s.currentJob.JobProvider == this).ToList();
        foreach (SurvivorController survivor in survivorControllers)
        {
            survivor.StopCurrentJob();
        }
        AssignedWorkersCount = 0;
    }

    public void RemoveJob()
    {
        SurvivorController survivorController = SurvivorsController.Instance.Survivors.FirstOrDefault(s => s.currentJob != null && s.currentJob.GetType() == typeof(BuildJob) && (Object)s.currentJob.JobProvider == this);

        if (survivorController == null) return;

        survivorController.StopCurrentJob();
        AssignedWorkersCount--;
    }
}
