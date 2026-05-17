using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SurvivorsController : Singleton<SurvivorsController>
{

    //private JobManager jobManager;

    public List<SurvivorController> Survivors = new List<SurvivorController>();
    public List<SurvivorController> survivorsToRemove = new();

    public GameObject survivorPrefab;


    protected override void Awake()
    {
        base.Awake();
        //jobManager = JobManager.Instance;
    }

    void Start()
    {
        InitSurvivorsList();
    }

    void Update()
    {
        if (JobManager.Instance.PendingJobs.Count > 0)
        {
            if (GetIdleSurvivors() != null)
            {
                IJob nextJob = JobManager.Instance.PendingJobs.Dequeue();
                AssignJobToClosestIdleSurvivor(nextJob);
            }
        }
    }

    void LateUpdate()
    {
        foreach (var s in survivorsToRemove)
            Survivors.Remove(s);

        survivorsToRemove.Clear();
    }

    void InitSurvivorsList()
    {
        // Get all objects with Survivor component
        SurvivorController[] foundSurvivors = FindObjectsByType<SurvivorController>();

        Survivors = new List<SurvivorController>(foundSurvivors);

        Debug.Log($"Found {Survivors.Count} survivors");
    }

    public List<SurvivorController> GetIdleSurvivors()
    {
        List<SurvivorController> idleSurvivors = Survivors.Where(s => s.survivorStateManager.CurrentState == ESurvivorState.Idling).ToList();

        //Debug.Log($"Found {idleSurvivors.Count} idle survivors");

        // if (idleSurvivors.Count == 0)
        // {
        //     //Debug.Log("No idle survivors found");
        // }

        return idleSurvivors;
    }

    SurvivorController AssignJobToClosestIdleSurvivor(IJob job)
    {
        // Get the list of idle survivors from the SurvivorsController
        List<SurvivorController> idleSurvivors = GetIdleSurvivors();

        if (idleSurvivors != null && idleSurvivors.Count > 0)
        {
            // Find the closest idle survivor to the job location
            SurvivorController closestSurvivor = null;
            float closestDistance = Mathf.Infinity;

            foreach (var survivor in idleSurvivors)
            {
                float distance = Vector3.Distance(survivor.transform.position, job.JobProvider.JobLocation);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSurvivor = survivor;
                }
            }

            // Assign the job to the closest survivor
            if (closestSurvivor != null)
            {
                closestSurvivor.currentJob = job;
                Debug.Log($"Assigned job to {closestSurvivor.name}");
                closestSurvivor.StartJob();
                return closestSurvivor;
            }
            else
            {
                Debug.Log("No closest survivor found");
                return null;
            }
        }
        else
        {
            Debug.Log("No idle survivors available to assign the job.");
            return null;
        }
    }

    public void AddAgeToAll(int ageToAdd)
    {
        foreach (var survivor in Survivors)
        {
            if (survivor == null) continue;
            survivor.AddAge(ageToAdd);
        }
    }

    public void SpawnSurvivor(GameObject survivorPrefab, Vector3 spawnPosition)
    {
        GameObject newSurvivor = Instantiate(survivorPrefab, spawnPosition, Quaternion.identity);
        SurvivorController newSurvivorController = newSurvivor.GetComponent<SurvivorController>();
        Survivors.Add(newSurvivorController);
    }

}
