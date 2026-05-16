using UnityEngine;
using Tides.Resources;

public class BuildJob : IJob
{
    private BuildableBehaviour buildableBehaviour;
    public Vector3 JobLocation => buildableBehaviour.transform.position;

    public BuildJob(BuildableBehaviour buildableBehaviour)
    {
        this.buildableBehaviour = buildableBehaviour;
    }

    public void StartJob(SurvivorController survivorController)
    {
        survivorController.survivorStateManager.ChangeState(ESurvivorState.Building);
    }

    public bool IsCompleted()
    {
        return buildableBehaviour;
    }
}