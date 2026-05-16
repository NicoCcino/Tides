using UnityEngine;
public abstract class ASurvivorState : BaseState
{

    protected SurvivorController survivorController;
    protected SurvivorStateManager survivorStateManager;


    protected ASurvivorState(SurvivorController survivorController, SurvivorStateManager survivorStateManager)
    {
        this.survivorController = survivorController;
        this.survivorStateManager = survivorStateManager;

    }

    public override abstract void Enter();
    public override abstract void Exit();
    public override abstract void Update();

}
