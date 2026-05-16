using UnityEngine;
using Tides.Resources;

public class BuildJob : IJob
{
    BuildableBehaviour buildableBehaviour;
    public IJobProvider JobProvider { get => buildableBehaviour; }

    public BuildJob(BuildableBehaviour buildableBehaviour)
    {
        this.buildableBehaviour = buildableBehaviour;
    }

    public bool IsCompleted()
    {
        return buildableBehaviour.BuildingProgress >= 1;
    }
}