using UnityEngine;
using System.Collections;
using Tides.Resources;
using System.Security.AccessControl;

[System.Serializable]
public class GatheringState : ASurvivorState
{
    public GatherPointBehaviour gatherPointBehaviour;
    // public Vector3 resourcePosition;
    public ResourceType resourceType;
    public Vector3 basePosition;
    private float gatherTimer;
    private bool isGathering;
    private float gatherDuration = 5.97f;
    public float gatherDistanceThreshold = 0.5f;


    public GatheringState(SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {

        isGathering = false;
        gatherTimer = 0f;
        gatherPointBehaviour = survivorController.gatherPointBehaviour;
        resourceType = gatherPointBehaviour.ResourceType;

        Debug.Log("Survivor entered gathering state with target " + gatherPointBehaviour.transform.position);


        // if (survivorController.baseTransform != null)
        // {
        //     basePosition = survivorController.baseTransform.position;
        // }
        //if (survivorController.resourceTransform != null)
        //{
        // resourcePosition = survivorController.resourceTransform.position;
        //}

        survivorController.GoTo(gatherPointBehaviour.transform.position);

    }

    public override void Exit()
    {
        survivorController.animator.SetTrigger("stopGather");
        survivorController.gatherPointBehaviour = null;
        isGathering = false;
    }

    public override void Update()
    {
        if (isGathering)
        {
            gatherTimer += Time.deltaTime;
            if (gatherTimer >= gatherDuration)
            {
                // Create resource in inventory
                if (survivorController.resourceInInventory == null)
                {
                    switch (resourceType)
                    {
                        case ResourceType.FOOD:
                            survivorController.resourceInInventory = new FoodResource(0);
                            break;
                        case ResourceType.WOOD:
                            survivorController.resourceInInventory = new WoodResource(0);
                            break;
                        default:
                            survivorController.resourceInInventory = new FoodResource(0);
                            break;
                    }
                    Debug.Log("Setting up resourceInInventory");
                }

                gatherTimer -= gatherDuration;

                // Consume resource in gather point
                if (gatherPointBehaviour.Resource.TryConsume(1))
                {
                    Debug.Log("Resource consumed from gather point");
                    // Add resource
                    survivorController.resourceInInventory.Add(1);
                }
                else
                {
                    Debug.Log("No resource left to gather from gather point, returning to base");
                    // Go back home if deposit is empty
                    survivorStateManager.ChangeState(ESurvivorState.GoingToBase);
                }

                if (survivorController.resourceInInventory.GetAmount() >= survivorController.maxLoad)
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

        float distance = Vector3.Distance(survivorController.transform.position, gatherPointBehaviour.transform.position);

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
