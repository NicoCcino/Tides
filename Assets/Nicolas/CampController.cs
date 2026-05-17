using UnityEngine;

public class CampController : MonoBehaviour
{
    [SerializeField] private BuildableBehaviour buildableBehaviour;

    public bool IsAvailable => buildableBehaviour.BuildingProgress >= 1;
}
