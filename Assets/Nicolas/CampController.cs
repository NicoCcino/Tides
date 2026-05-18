using UnityEngine;

public class CampController : MonoBehaviour
{
    [SerializeField] private BuildableBehaviour buildableBehaviour;
    public Transform targetPoint;

    public bool IsAvailable => buildableBehaviour.BuildingProgress >= 1;
}
