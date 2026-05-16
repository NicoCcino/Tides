using UnityEngine;

public interface IJob
{
    public IJobProvider JobProvider { get; }
    public bool IsCompleted();
}
