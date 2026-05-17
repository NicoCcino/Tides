using UnityEngine;
[System.Serializable]
public class BuildingState : ASurvivorState
{
    bool isBuilding = false;
    float buildDistanceThreshold = 3.0f;
    public BuildJob BuildJob { get; set; }
    public BuildingState(SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        survivorController.GoTo(BuildJob.JobProvider.JobLocation);
        Debug.Log("Survivor entered building state");
    }

    public override void Exit()
    {
        isBuilding = false;
        survivorController.animator.SetTrigger("stopBuild");
    }

    public override void Update()
    {
        if (!isBuilding)
        {
            CheckDistance();
            return;
        }
        if (BuildJob.IsCompleted())
        {
            survivorController.StopCurrentJob();
            return;
        }
        (BuildJob.JobProvider as BuildableBehaviour).TickUpdateProgress();
    }

    private void CheckDistance()
    {
        // If distance between agent and targetPosition is less than a certain value, start gather.
        if (isBuilding) return;

        if (survivorController.agent.pathPending) return;

        float distance = Vector3.Distance(survivorController.transform.position, BuildJob.JobProvider.JobLocation);

        if (distance <= buildDistanceThreshold)
        {
            StartBuilding();
        }
    }
    private void StartBuilding()
    {
        isBuilding = true;

        survivorController.agent.ResetPath();
        survivorController.agent.velocity = Vector3.zero;

        // Start gather animation
        survivorController.animator.SetTrigger("build");
    }

}
