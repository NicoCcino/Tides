using UnityEngine;
[System.Serializable]
public class GameOverState : AGameState
{
    public AudioClip audioClip;
    public AudioSource source;

    public override void Enter()
    {
        source.PlayOneShot(audioClip);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}
