using UnityEngine;
using UnityEngine.AI;

public class SurvivorController : MonoBehaviour
{
    public NavMeshAgent agent;
    private Survivor survivor;
    public Animator animator;
    private SurvivorStateManager survivorStateManager;
    public bool isGathering;

    [Header("Animation")]
    private static readonly int SpeedHash = Animator.StringToHash("speed");
    private static readonly int GatherHash = Animator.StringToHash("gather");

    [Header("Debug")]
    public Transform baseTransform;
    public Transform resourceTransform;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        survivor = GetComponent<Survivor>();
        animator = GetComponent<Animator>();
        survivorStateManager = GetComponent<SurvivorStateManager>();
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
        // Send speed to anim controller
        float currentSpeed = agent.velocity.magnitude;
        float normalizedSpeed = currentSpeed / agent.speed;
        animator.SetFloat(SpeedHash, currentSpeed / agent.speed);
    }

    public void OnGatherAnimationFinished()
    {
        isGathering = false;
        animator.SetTrigger("stopGather");
        survivorStateManager.ChangeState(ESurvivorState.GoingToBase);
    }
}
