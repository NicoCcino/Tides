using UnityEngine;
using Tides.Resources;

public class BuildJob : IJob
{
    private GatherPointBehaviour gatherPointBehaviour;
    public Vector3 JobLocation => gatherPointBehaviour.transform.position;

    public BuildJob()
    {
    }

    public Vector3 GetTargetPosition()
    {
        return Vector3.zero; // TO DO
    }

    public void StartJob(SurvivorController survivorController)
    {
        //survivorController.survivorStateManager.ChangeState(ESurvivorState.Gathering);
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