using UnityEngine;
using UnityEngine.AI;

public class SurvivorController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Survivor survivor;
    private Animator animator;

    private static readonly int SpeedHash = Animator.StringToHash("speed");


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        survivor = GetComponent<Survivor>();
        animator = GetComponent<Animator>();


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.speed = survivor.speed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnim();
    }

    public void GoTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    private void UpdateAnim()
    {
        float currentSpeed = agent.velocity.magnitude;
        float normalizedSpeed = currentSpeed / agent.speed;

        animator.SetFloat(SpeedHash, currentSpeed / agent.speed);
    }
}
