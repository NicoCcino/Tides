using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SurvivorsController : Singleton<SurvivorsController>
{

    public JobManager jobManager;

    public List<SurvivorController> survivors = new List<SurvivorController>();
    public List<SurvivorController> survivorsToRemove = new();


    protected override void Awake()
    {
        base.Awake();
        jobManager = JobManager.Instance;
    }

    void Start()
    {
        InitSurvivorsList();
    }

    void Update()
    {
        if (jobManager.PendingJobs.Count > 0)
        {
            if (GetIdleSurvivors() != null)
            {
                IJob nextJob = jobManager.PendingJobs.Dequeue();
                AssignJobToClosestIdleSurvivor(nextJob);
            }
        }
    }

    void LateUpdate()
    {
        foreach (var s in survivorsToRemove)
            survivors.Remove(s);

        survivorsToRemove.Clear();
    }

    void InitSurvivorsList()
    {
        // Get all objects with Survivor component
        SurvivorController[] foundSurvivors = FindObjectsByType<SurvivorController>();

        survivors = new List<SurvivorController>(foundSurvivors);

        Debug.Log($"Found {survivors.Count} survivors");
    }

    public List<SurvivorController> GetIdleSurvivors()
    {
        List<SurvivorController> idleSurvivors = survivors.Where(s => s.survivorStateManager.CurrentState == ESurvivorState.Idling).ToList();

        Debug.Log($"Found {idleSurvivors.Count} idle survivors");

        if (idleSurvivors.Count == 0)
        {
            Debug.Log("No idle survivors found");
            return null;
        }
        else
        {
            return idleSurvivors;
        }
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
                float distance = Vector3.Distance(survivor.transform.position, job.JobLocation);

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
        foreach (var survivor in survivors)
        {
            if (survivor == null) continue;
            survivor.AddAge(ageToAdd);
        }
    }
}
