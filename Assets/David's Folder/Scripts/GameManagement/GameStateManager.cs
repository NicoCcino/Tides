using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using WiDiD.SceneManagement;
using UnityEngine;

public class GameStateManager : BaseFSM<EGameState, AGameState>
{
    public static GameStateManager Instance;
    [SerializeField] private SceneSet coreSet;
    [Header("States")]
    [SerializeField] private MainMenuState mainMenuState;
    [SerializeField] private PlayingState playingState;
    [SerializeField] private GameOverState gameoverState;
    [SerializeField] private VictoryState victoryState;

    [Header("Debug")]
    [SerializeField] private EGameState startState;

    private void Start()
    {
        Instance = this;
        InitStates();
        SceneManager.Instance.LoadSceneSet(coreSet, true, true, OnCoreSetLoaded);
    }

    public override void ChangeState(EGameState newState)
    {
        // Exit the current state (if any)
        if (stateDictionary == null || newState == CurrentState)
        {
            return;
        }

        AGameState currentStateObj = stateDictionary.ContainsKey(CurrentState) ? stateDictionary[CurrentState] : null;
        AGameState newStateObj = stateDictionary.ContainsKey(newState) ? stateDictionary[newState] : null;

        if (currentStateObj != null)
        {
            currentStateObj.Exit();
        }

        if (newStateObj != null)
        {
            if (newStateObj.SceneSet != null && (currentStateObj == null || newStateObj.SceneSet != currentStateObj.SceneSet))
            {
                bool isEnteringAdditive = IsAdditiveState(newState);
                bool isExitingAdditive = IsAdditiveState(CurrentState);
                bool showLoadingScreen = !isEnteringAdditive && !isExitingAdditive;

                // Load the new scene set.
                SceneManager.Instance.LoadSceneSet(newStateObj.SceneSet, true, showLoadingScreen);

                // Only unload if we're not moving into an additive state (like GameOver/Victory)
                if (!isEnteringAdditive && currentStateObj != null && currentStateObj.SceneSet != null)
                {
                    SceneManager.Instance.UnloadSceneSet(currentStateObj.SceneSet, true, showLoadingScreen);
                }
            }

            CurrentState = newState;
            newStateObj.Enter();
        }
    }

    private bool IsAdditiveState(EGameState state)
    {
        return state == EGameState.GameOver || state == EGameState.Victory;
    }

    public override void InitStates()
    {
        if (stateDictionary.Count == 0)
        {
            // Initialize the state dictionary
            stateDictionary = new Dictionary<EGameState, AGameState>
             {
                        { EGameState.Default, mainMenuState },
                        { EGameState.MainMenu, mainMenuState },
                        { EGameState.Playing, playingState },
                        {EGameState.GameOver,gameoverState},
                        {EGameState.Victory, victoryState}
             }
        ;
        }
    }
    public void SetCoreSceneSet(SceneSet sceneSet)
    {
        coreSet = sceneSet;
    }
    private void OnCoreSetLoaded()
    {
        ChangeState(startState);
    }
}
