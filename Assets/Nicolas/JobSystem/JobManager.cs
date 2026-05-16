using System.Collections.Generic;
using Tides.Resources;
using UnityEngine;

public class JobManager : Singleton<JobManager>
{
    private IJob testJob;
    public GatherPointBehaviour testGatherPointBehaviour;
    public Queue<IJob> PendingJobs { get; private set; }

    void Start()
    {
        PendingJobs = new Queue<IJob>();
        testJob = new GatherJob(testGatherPointBehaviour);
        PendingJobs.Enqueue(testJob);
    }

}
