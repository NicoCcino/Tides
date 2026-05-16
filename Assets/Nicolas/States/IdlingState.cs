using UnityEngine;
[System.Serializable]
public class IdlingState : ASurvivorState
{
    public IdlingState(Survivor survivor, SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivor, survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered idle state");
        survivorStateManager.ChangeState(ESurvivorState.GoingToBase);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
}
