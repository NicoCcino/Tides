using UnityEngine;
[System.Serializable]
public class VictoryState : AGameState
{
    public AudioClip victoryClip;
    public AudioSource audioSource;
    public override void Enter()
    {
        audioSource.PlayOneShot(victoryClip);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}
