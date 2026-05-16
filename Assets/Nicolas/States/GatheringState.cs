using UnityEngine;
[System.Serializable]
public class GatheringState : ASurvivorState
{

    public Vector3 resourcePosition;
    public Vector3 basePosition;

    public GatheringState(Survivor survivor, SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivor, survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered gathering state");
        if (survivorController.baseTransform != null)
        {
            basePosition = survivorController.baseTransform.position;
        }
        if (survivorController.resourceTransform != null)
        {
            resourcePosition = survivorController.resourceTransform.position;
        }

        survivorController.GoTo(resourcePosition);

    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        // If distance between agent and targetPosition is less than a certain value, start gather.
        if (survivorController.isGathering) return;

        if (survivorController.agent.pathPending) return;

        float distance = Vector3.Distance(survivor.transform.position, resourcePosition);

        if (distance <= survivor.gatherDistanceThreshold)
        {
            StartGather();
        }
    }

    private void StartGather()
    {
        survivorController.isGathering = true;

        // Stop movement
        survivorController.agent.ResetPath();
        survivorController.agent.velocity = Vector3.zero;

        // Start gather animation
        survivorController.animator.SetTrigger("gather");


    }

}
