using UnityEngine;
using System.Collections.Generic;

public class CampsController : Singleton<CampsController>
{

    public List<CampController> Camps = new List<CampController>();

    void Start()
    {
        InitCampsList();
    }

    void Update()
    {

    }

    void InitCampsList()
    {
        // Get all objects with Camp component
        CampController[] foundCamps = FindObjectsByType<CampController>();

        Camps = new List<CampController>(foundCamps);

        Debug.Log($"Found {Camps.Count} camps");
    }

    public CampController GetClosestCamp(SurvivorController survivor)
    {
        CampController closestCamp = null;
        float closestDistance = Mathf.Infinity;
        Vector3 survivorPosition = survivor.transform.position;

        foreach (CampController camp in Camps)
        {
            if (!camp.IsAvailable) continue;

            float distance = Vector3.Distance(survivorPosition, camp.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCamp = camp;
            }
        }

        if (closestCamp != null)
        {
            Debug.Log($"Closest camp to survivor {survivor.name} is {closestCamp.name} at distance {closestDistance}");
        }
        return closestCamp;
    }
}
