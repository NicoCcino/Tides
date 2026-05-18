using UnityEngine;
using Tides.Resources;
using Unity.VisualScripting;

public class ButtonBuySurvivor : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TryBuySurvivor()
    {

        if (ResourcesManager.Instance.TryConsumeFood(10))
        {
            SpawnSurvivor();
        }
        else
        {
            Debug.Log("Not enough food to buy survivor!");
        }
    }

    private void SpawnSurvivor()
    {
        // Assuming you have a prefab for the survivor and a spawn point in your scene
        GameObject survivorPrefab = SurvivorsController.Instance.survivorPrefab; // Reference to survivor prefab
        Vector2 random = Random.insideUnitCircle * 0.3f;
        Vector3 spawnPosition = spawnPoint.position + new Vector3(random.x, 0, random.y); // Spawn next to the button

        SurvivorsController.Instance.SpawnSurvivor(survivorPrefab, spawnPosition);
    }
}
