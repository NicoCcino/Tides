using UnityEngine;
using UnityEngine.AI;

public class SurvivorController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Survivor survivor;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        survivor = GetComponent<Survivor>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.speed = survivor.speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);

    }
}
