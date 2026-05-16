using UnityEngine;
[System.Serializable]
public class GoingToBaseState : ASurvivorState
{
    public Vector3 target = Vector3.zero;
    public GoingToBaseState(Survivor survivor, SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivor, survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered GoingToBaseState state");
        survivorController.GoTo(target);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
}
