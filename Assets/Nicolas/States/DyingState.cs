using UnityEngine;
[System.Serializable]
public class DyingState : ASurvivorState
{
    public DyingState(SurvivorController survivorController, SurvivorStateManager survivorStateManager) : base(survivorController, survivorStateManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Survivor entered dying state");

        if (survivorController.currentJob != null)
        {
            survivorController.currentJob = null;
        }

        survivorController.isDying = true;
        Debug.Log($"Survivor {survivorController.name} has died of old age at {survivorController.age} years.");

        // Change state
        survivorStateManager.ChangeState(ESurvivorState.Default);

        // Play Death animation
        survivorController.animator.SetTrigger("die");
        // Disable survivor's ability to interact with the world
        survivorController.agent.isStopped = true;
        // Remove survivor from SurvivorsController list
        SurvivorsController.Instance.survivorsToRemove.Add(survivorController);
        // If survivor has a job, remove it from the job or mark it as unassigned so that another survivor can take it
        // JobManager.Instance.PendingJobs.Enqueue(survivorController.currentJob);
        // If survivor has a resource in inventory, delete it
        if (survivorController.resourceInInventory != null)
        {
            survivorController.resourceInInventory = null;
        }
        // Delete survivor after some time to allow death animation to play
        survivorController.DestroyThis(5f);

        //Audio
        if (survivorController.AudioSource != null && survivorController.DieClip != null)
        {
            survivorController.AudioSource.clip = survivorController.DieClip;
            survivorController.AudioSource.loop = false;
            survivorController.AudioSource.volume = 1.2f;
            survivorController.AudioSource.Play();
        }
    }

    public override void Exit()
    {
    }

    public override void Update()
    {

    }
}
