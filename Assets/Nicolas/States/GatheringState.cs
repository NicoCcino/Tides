using UnityEngine;
[System.Serializable]
public class GatheringState : ASurvivorState
{
    public GatheringState(Survivor survivor, SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivor, survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered gathering state");
    }

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
}
