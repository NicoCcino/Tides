using UnityEngine;
using UnityEngine.AI;

public class Survivor : MonoBehaviour
{
    public float age;
    public float speed;
    public float gatherDistanceThreshold = 0.5f;
    private SurvivorController survivorController;
    private NavMeshAgent agent;

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
