using UnityEngine;

public interface IJobProvider
{
    public Vector3 JobLocation { get; }
    public int AssignedWorkersCount { get; set; }
    public void AddJob();
    public void RemoveJob();
    public void CancelJobs();
}
