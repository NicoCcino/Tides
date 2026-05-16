using UnityEngine;
using UnityEngine.AI;

public class Survivor : MonoBehaviour
{
    public float age;
    public float speed;

    private SurvivorController survivorController;
    private NavMeshAgent agent;

    [Header("Gather")]

    //public float gatherDistanceThreshold = 0.5f;
    //public float gatherDuration = 5.97f;


    [Header("Inventory")]
    public int maxLoad = 5;
    public IResource resourceInInventory = null;

    public void Awake()
    {
        survivorController = GetComponent<SurvivorController>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {

    }

    public void SetAge(float newAge)
    {
        age = newAge;
        // TO DO : Speed based on age

        agent.speed = speed;
    }

}
