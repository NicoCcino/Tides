using UnityEngine;
[System.Serializable]
public class BuildingState : ASurvivorState
{
    public BuildingState(SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered building state");
    }

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
}
