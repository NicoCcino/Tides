using UnityEngine;
[System.Serializable]
public class DefaultState : ASurvivorState
{
    public DefaultState(Survivor survivor, SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivor, survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        survivorStateManager.ChangeState(ESurvivorState.Idling);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
}
