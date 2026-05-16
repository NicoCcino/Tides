using UnityEngine;
using System.Collections;

[System.Serializable]
public class GatheringState : ASurvivorState
{

    public Vector3 resourcePosition;
    public Vector3 basePosition;
    public string resourceType; // TODO: REPLACE WITH RIGHT DATA
    private float gatherTimer;
    private bool isGathering;
    private float gatherDuration = 5.97f;
    public float gatherDistanceThreshold = 0.5f;


    public GatheringState(Survivor survivor, SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivor, survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered gathering state");

        isGathering = false;
        gatherTimer = 0f;

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
        survivorController.animator.SetTrigger("stopGather");
    }

    public override void Update()
    {
        if (isGathering)
        {
            gatherTimer += Time.deltaTime;
            if (gatherTimer >= gatherDuration)
            {
                // Create resource in inventory
                if (survivor.resourceInInventory == null)
                {
                    survivor.resourceInInventory = new WoodResource(0); // TO DO : Replace with right resource
                    Debug.Log("setting resourceInInventory");
                }
                // Add resource
                survivor.resourceInInventory.Add(1);
                gatherTimer -= gatherDuration;

                if (survivor.resourceInInventory.GetAmount() >= survivor.maxLoad)
                {
                    // Go back home with full load
                    survivorStateManager.ChangeState(ESurvivorState.GoingToBase);
                }
            }

        }
        CheckDistance();

    }

    private void CheckDistance()
    {
        // If distance between agent and targetPosition is less than a certain value, start gather.
        if (isGathering) return;

        if (survivorController.agent.pathPending) return;

        float distance = Vector3.Distance(survivor.transform.position, resourcePosition);

        if (distance <= gatherDistanceThreshold)
        {
            StartGather();
        }
    }

    private void StartGather()
    {
        isGathering = true;

        // Stop movement
        survivorController.agent.ResetPath();
        survivorController.agent.velocity = Vector3.zero;

        // Start gather animation
        survivorController.animator.SetTrigger("gather");
    }

}
