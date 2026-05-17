using UnityEngine;

public class RingABell : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public void RingBell()
    {
        SurvivorController[] survivorsControllers = SurvivorsController.Instance.Survivors.ToArray();

        for (int i = 0; i < survivorsControllers.Length; i++)
        {
            if (survivorsControllers[i] == null) continue;
            survivorsControllers[i].survivorStateManager.ChangeState(ESurvivorState.GoingToBase);
        }
        audioSource.Play();
    }
}
