using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class SurvivorStateManager : BaseFSM<ESurvivorState, ASurvivorState>
{
    [SerializeField] public SurvivorController survivorController;
    [SerializeField] public SurvivorStateManager survivorStateManager;

    [Header("States")]
    [SerializeField] private DefaultState defaultState;

    //[Header("Debug")]
    //private bool isInit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        InitStates();
        ChangeState(ESurvivorState.Idling);
    }


    public override void ChangeState(ESurvivorState newState)
    {
        // Exit the current state (if any)
        if ((stateDictionary == null || newState == CurrentState))
        {
            return;
        }
        if (stateDictionary.ContainsKey(CurrentState))
        {
            stateDictionary[CurrentState].Exit();
        }

        // Update the current state
        CurrentState = newState;

        // Enter the new state
        if (stateDictionary.ContainsKey(newState))
        {
            stateDictionary[newState].Enter();
        }
    }

    public override void InitStates()
    {
        if (stateDictionary.Count == 0)
        {
            // Initialize the state dictionary
            stateDictionary = new Dictionary<ESurvivorState, ASurvivorState>
             {
                        { ESurvivorState.Default, new DefaultState(survivorController, survivorStateManager)},
                        { ESurvivorState.Idling, new IdlingState(survivorController, survivorStateManager)},
                        { ESurvivorState.Gathering, new GatheringState(survivorController, survivorStateManager)},
                        { ESurvivorState.Building, new BuildingState(survivorController, survivorStateManager)},
                        { ESurvivorState.GoingToBase, new GoingToBaseState(survivorController, survivorStateManager)}
             };
        }
    }

    [Button("Set Start State")]
    private void SetStartState()
    {
        // ChangeState(startState);
    }
}
