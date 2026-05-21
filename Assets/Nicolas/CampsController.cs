using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

        int nextWaveHeight = TidesManager.Instance.tideCyclesSO.tideCycles[TidesManager.Instance.currentCycleIndex].WaveHeight;
        CampController[] safeCamps = Camps.Where(c => c.transform.position.y > nextWaveHeight + 0.1f && c.IsAvailable).ToArray();
        if (safeCamps.Length == 0)
        {
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
        }
        else
        {
            foreach (CampController camp in safeCamps)
            {
                if (!camp.IsAvailable) continue;
                float distance = Vector3.Distance(survivorPosition, camp.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCamp = camp;
                }
            }
        }

        if (closestCamp != null)
        {
            Debug.Log($"Closest camp to survivor {survivor.name} is {closestCamp.name} at distance {closestDistance}");
        }
        return closestCamp;
    }
}
