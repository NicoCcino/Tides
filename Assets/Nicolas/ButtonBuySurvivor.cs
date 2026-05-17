using UnityEngine;
using Tides.Resources;

public class ButtonBuySurvivor : MonoBehaviour
{
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

        Vector3 spawnPosition = this.transform.position + new Vector3(0, 0, -1); // Spawn next to the button

        SurvivorsController.Instance.SpawnSurvivor(survivorPrefab, spawnPosition);
    }
}
