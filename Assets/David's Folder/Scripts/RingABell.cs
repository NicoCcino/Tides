using System.Linq;
using UnityEngine;

public class RingABell : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public void RingBell()
    {
        SurvivorController[] survivorsControllers = SurvivorsController.Instance.Survivors.ToArray();

        IJobProvider[] allActiveProviders = survivorsControllers
            .Where(s => s.currentJob != null)
            .Select(s => s.currentJob.JobProvider)
            .Distinct()
            .ToArray();


        for (int i = 0; i < survivorsControllers.Length; i++)
        {
            if (survivorsControllers[i] == null) continue;
            survivorsControllers[i].StopCurrentJob();
            survivorsControllers[i].survivorStateManager.ChangeState(ESurvivorState.GoingToBase);
        }
        foreach (var provider in allActiveProviders)
        {
            provider.AssignedWorkersCount = 0;
        }

        audioSource.Play();
    }
}
