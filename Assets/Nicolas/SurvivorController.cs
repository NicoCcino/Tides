using Tides.Resources;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorController : MonoBehaviour
{
    public float age;
    public float speed;
    public SurvivorStateManager survivorStateManager;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Job")]
    public IJob currentJob;
    public GatherPointBehaviour gatherPointBehaviour;


    [Header("Inventory")]
    public int maxLoad = 5;
    public IResource resourceInInventory = null;

    [Header("Animation")]
    private static readonly int SpeedHash = Animator.StringToHash("speed");
    private static readonly int GatherHash = Animator.StringToHash("gather");


    [Header("Debug")]
    public Transform baseTransform;
    public Transform resourceTransform;


    public void Awake()
    {
        survivorStateManager = GetComponent<SurvivorStateManager>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        agent.speed = speed;

    }

    void Update()
    {
        UpdateAnim();
    }

    public void SetAge(float newAge)
    {
        age = newAge;
        // TO DO : Speed based on age

        agent.speed = speed;
    }



    public void GoTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
        Debug.Log($"Survivor {name} is going to {targetPosition}. Agent's destination is {agent.destination}");
    }

    private void UpdateAnim()
    {
        // Send speed to anim controller
        float currentSpeed = agent.velocity.magnitude;
        float normalizedSpeed = currentSpeed / agent.speed;
        animator.SetFloat(SpeedHash, currentSpeed / agent.speed);
    }

    public void StartJob()
    {
        if (currentJob is GatherJob gatherJob)
        {
            gatherPointBehaviour = gatherJob.gatherPointBehaviour;

            survivorStateManager.ChangeState(ESurvivorState.Gathering);
        }
        if (currentJob is BuildJob buildJob)
        {
            survivorStateManager.ChangeState(ESurvivorState.Building);
        }
    }
    public void StopCurrentJob()
    {
        gatherPointBehaviour = null;
        survivorStateManager.ChangeState(ESurvivorState.Idling);
    }
}
