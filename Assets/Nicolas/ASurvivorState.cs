using UnityEngine;
public abstract class ASurvivorState : BaseState
{

    protected SurvivorController survivorController;
    protected Survivor survivor;
    protected SurvivorStateManager survivorStateManager;


    protected ASurvivorState(Survivor survivor, SurvivorController survivorController, SurvivorStateManager survivorStateManager)
    {
        this.survivor = survivor;
        this.survivorController = survivorController;
        this.survivorStateManager = survivorStateManager;

    }

    public override abstract void Enter();
    public override abstract void Exit();
    public override abstract void Update();

}
