using UnityEngine;
[System.Serializable]
public class EatingState : ASurvivorState
{


    private bool animationStarted;
    private float animLength;
    Animator animator;

    public EatingState(SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        survivorController.agent.ResetPath();
        Debug.Log("Survivor entered eating state");

        animator = survivorController.animator;

        animator.SetBool("eating", true);

        animationStarted = false;

        // Audio triggered through anim event.

    }

    public override void Exit()
    {
        Debug.Log("Survivor exiting eating state");
        animator.SetBool("eating", false);
    }

    public override void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!animationStarted)
        {
            if (stateInfo.IsName("Eating"))
            {
                animationStarted = true;
            }

            return;
        }

        // Quand l'anim est terminée
        if (stateInfo.IsName("Eating") && stateInfo.normalizedTime >= 1f)
        {
            survivorStateManager.ChangeState(ESurvivorState.Idling);
        }
    }
}
