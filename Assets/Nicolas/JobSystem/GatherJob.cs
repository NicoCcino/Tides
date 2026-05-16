using UnityEngine;
using Tides.Resources;

public class GatherJob : IJob
{
    public GatherPointBehaviour gatherPointBehaviour;
    public IJobProvider JobProvider { get => gatherPointBehaviour; }

    public GatherJob(GatherPointBehaviour gatherPointBehaviour)
    {
        this.gatherPointBehaviour = gatherPointBehaviour;
    }

    public bool IsCompleted()
    {
        return true;
    }
}