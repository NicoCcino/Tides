using UnityEngine;
using Tides.Resources;

[System.Serializable]
public class GoingToBaseState : ASurvivorState
{
    public Vector3 basePosition = Vector3.zero;
    float baseDistanceThreshold = 2f;
    bool isStoring = false;
    float storeTimer = 0f;
    float storeDuration = 3f;
    public GoingToBaseState(SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered GoingToBaseState state");
        basePosition = CampsController.Instance.GetClosestCamp(survivorController).transform.position;

        survivorController.GoTo(basePosition);

        isStoring = false;
        storeTimer = 0f;

    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        CheckDistance();
        StoreUpdate();
    }

    void StoreUpdate()
    {
        if (isStoring)
        {
            storeTimer += Time.deltaTime;
            if (storeTimer >= storeDuration)
            {
                CompleteStoreResources();
            }
        }
    }

    private void CheckDistance()
    {
        if (!isStoring)
        {
            // If distance between agent and base is less than a certain value, put down resource
            if (survivorController.agent.pathPending) return;

            float distance = Vector3.Distance(survivorController.transform.position, basePosition);

            if (distance <= baseDistanceThreshold)
            {
                Debug.Log("Survivor reached based with resources");


                // Base collects all inventory.
                StartStoreResources();

            }
        }
    }

    private void StartStoreResources()
    {
        isStoring = true;

        // Play store resources anim
        survivorController.animator.SetTrigger("store");

    }

    private void CompleteStoreResources()
    {

        // Transfer resource from survivor inventory to global resource manager
        if (survivorController.resourceInInventory == null) return;
        if (survivorController.resourceInInventory is WoodResource)
        {
            Tides.Resources.ResourcesManager.Instance.AddWood(
                survivorController.resourceInInventory.GetAmount()
                );
            survivorController.resourceInInventory = null;
        }
        else if (survivorController.resourceInInventory is FoodResource)
        {
            ResourcesManager.Instance.AddFood(
                survivorController.resourceInInventory.GetAmount()
                );
            survivorController.resourceInInventory = null;
        }

        storeTimer = 0f;
        // Check if deposit still has resources. If not, remove current gathering job.
        if (survivorController.gatherPointBehaviour == null || survivorController.gatherPointBehaviour.Resource.GetAmount() <= 0)
        {
            Debug.Log("No resources left to gather at this point, removing currentJob");
            survivorController.currentJob = null;
            survivorController.gatherPointBehaviour = null;
        }
        // Back to idle
        survivorStateManager.ChangeState(ESurvivorState.Idling);

    }
}
