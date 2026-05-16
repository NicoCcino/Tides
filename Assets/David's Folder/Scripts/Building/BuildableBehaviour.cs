using IFCE.ValueTracking.UX;
using Unity.VisualScripting;
using UnityEngine;

public class BuildableBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject buildableGameObject;
    [SerializeField] private GameObject buildedGameObject;



    private void UpdateProgress(float amount)
    {

        if (amount >= 1.0f)
        {
            SpawnBuilding();
        }
    }
    private void SpawnBuilding()
    {
        buildedGameObject.SetActive(true);
        buildableGameObject.SetActive(false);
    }
}
