using System.Collections.Generic;
using Tides.Resources;
using UnityEngine;

public class JobManager : Singleton<JobManager>
{
    public Queue<IJob> PendingJobs { get; private set; }

    void Start()
    {
        PendingJobs = new Queue<IJob>();
    }

}
