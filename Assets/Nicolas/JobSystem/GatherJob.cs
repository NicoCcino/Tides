using UnityEngine;
using Tides.Resources;

public class GatherJob : IJob
{
    public GatherPointBehaviour gatherPointBehaviour;
    public Vector3 JobLocation => gatherPointBehaviour.transform.position;

    public GatherJob(GatherPointBehaviour gatherPointBehaviour)
    {
        this.gatherPointBehaviour = gatherPointBehaviour;
    }

    public Vector3 GetTargetPosition()
    {
        return gatherPointBehaviour.transform.position;
    }

    public void StartJob(SurvivorController survivorController)
    {
        // If there is still resource to gather, start gathering, otherwise do nothing (job should be removed from queue)
        if (gatherPointBehaviour.Resource.GetAmount() > 0)
        {
            survivorController.survivorStateManager.ChangeState(ESurvivorState.Gathering);
        }
        else
        {
            Debug.Log("No resources left to gather at this point");
            survivorController.currentJob = null;
            survivorController.survivorStateManager.ChangeState(ESurvivorState.Idling);
        }
    }

    public bool IsCompleted()
    {
        return true;
    }

    //public EJobType GetJobType()
    //{
    //    return gatherPointBehaviour.Gather;
    //}
}