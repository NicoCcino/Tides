using System;
using Tides.Camera;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCutscene : Singleton<IntroCutscene>
{
    private PlayableDirector playableDirector;
    public bool hasBeenPlayed;
    public bool stopped = false;
    public CameraController cameraController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();

        playableDirector = GetComponent<PlayableDirector>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TryPlay()
    {
        if (playableDirector != null && !hasBeenPlayed)
        {
            playableDirector.Play();
            cameraController.enabled = false;
            hasBeenPlayed = true;
            playableDirector.stopped += OnStopped;
        }
    }

    private void OnStopped(PlayableDirector director)
    {
        cameraController.enabled = true;
        stopped = true;
    }
}
