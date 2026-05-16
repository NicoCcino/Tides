using UnityEngine;
using Tides.Resources;

[System.Serializable]
public class GoingToBaseState : ASurvivorState
{
    public Vector3 target = Vector3.zero;
    public Vector3 basePosition = Vector3.zero;
    float baseDistanceThreshold = 0.5f;
    bool isStoring = false;
    float storeTimer = 0f;
    float storeDuration = 4.5f;
    public GoingToBaseState(Survivor survivor, SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivor, survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered GoingToBaseState state");
        survivorController.GoTo(target);

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
                storeTimer = 0f;
                survivorStateManager.ChangeState(ESurvivorState.Idling);
            }
        }
    }

    private void CheckDistance()
    {
        if (!isStoring)
        {
            // If distance between agent and base is less than a certain value, put down resource
            if (survivorController.agent.pathPending) return;

            float distance = Vector3.Distance(survivor.transform.position, basePosition);

            if (distance <= baseDistanceThreshold)
            {
                Debug.Log("Survivor reached based with resources");


                // Base collects all inventory.
                StoreResources();

            }
        }



    }

    private void StoreResources()
    {
        isStoring = true;
        if (survivor.resourceInInventory == null) return;

        if (survivor.resourceInInventory is WoodResource)
        {
            Tides.Resources.ResourcesManager.Instance.AddWood(
                survivor.resourceInInventory.GetAmount()
                );
            survivor.resourceInInventory = null;
        }
        else if (survivor.resourceInInventory is FoodResource)
        {
            ResourcesManager.Instance.AddFood(
                survivor.resourceInInventory.GetAmount()
                );
            survivor.resourceInInventory = null;
        }

        // Play store resources anim
        survivorController.animator.SetTrigger("store");

    }
}
