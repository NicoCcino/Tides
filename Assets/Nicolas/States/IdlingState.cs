using UnityEngine;
[System.Serializable]
public class IdlingState : ASurvivorState
{
    public IdlingState(SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered idle state");
        // survivorStateManager.ChangeState(ESurvivorState.Gathering);
        if (survivorController.currentJob != null)
        {
            survivorController.StartJob();
        }
    }

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
}
