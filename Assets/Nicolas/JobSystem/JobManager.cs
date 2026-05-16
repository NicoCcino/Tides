using System.Collections.Generic;
using UnityEngine;

public class JobManager : Singleton<JobManager>
{
    public Queue<IJob> PendingJobs { get; private set; }

}
