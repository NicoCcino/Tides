using UnityEngine;

public interface IJob
{
    public Vector3 JobLocation { get; }
    public bool IsCompleted();
}
