using UnityEngine;
[System.Serializable]
public class DefaultState : ASurvivorState
{
    public DefaultState(SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivorController, survivorStateManager)
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
