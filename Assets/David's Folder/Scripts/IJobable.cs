using UnityEngine;

public interface IJobable
{
    public int AssignedWorkersCount { get; set; }
    public void AddJob();
    public void RemoveJob();
    public void CancelJobs();
}
