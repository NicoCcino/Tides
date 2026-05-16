using Tides.Resources;
using UnityEngine;
using UnityEngine.AI;

public class SurvivorController : MonoBehaviour
{
    public float age; // Age in full cycles of tide
    public float maxAge = 10f; // Age at which survivor dies
    public float maxSpeed = 3.5f; // Speed at age 0, will decrease with age until it reaches minSpeed at maxAge
    public float minSpeed = 1.5f; // Speed at max age
    public SurvivorStateManager survivorStateManager;
    public NavMeshAgent agent;
    public Animator animator;
    public bool isDying = false;

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
        UpdateSpeedBasedOnAge();
    }

    void Update()
    {
        UpdateAnim();
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
        animator.SetFloat(SpeedHash, currentSpeed / maxSpeed);
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

    public void AddAge(int ageToAdd)
    {
        age += ageToAdd;
        if (age > maxAge)
        {
            Die();
        }
        else
        {
            UpdateSpeedBasedOnAge();
        }
        Debug.Log($"Survivor {name} is now {age} years old.");
    }

    void UpdateSpeedBasedOnAge()
    {
        float speed = Mathf.Lerp(maxSpeed, minSpeed, age / maxAge);
        agent.speed = speed;
    }

    void Die()
    {
        isDying = true;
        Debug.Log($"Survivor {name} has died of old age at {age} years.");
        // TODO: play Death animation
        animator.SetTrigger("die");
        // Disable survivor's ability to interact with the world
        agent.isStopped = true;
        // Remove survivor from SurvivorsController list
        SurvivorsController.Instance.survivorsToRemove.Add(this);
        // If survivor has a job, remove it from the job or mark it as unassigned so that another survivor can take it
        JobManager.Instance.PendingJobs.Enqueue(currentJob);
        currentJob = null;
        // If survivor has a resource in inventory, delete it
        if (resourceInInventory != null)
        {
            resourceInInventory = null;
        }
        // Delete survivor after some time to allow death animation to play
        Destroy(gameObject, 5f);
    }

}
