using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class SurvivorStateManager : BaseFSM<ESurvivorState, ASurvivorState>
{
    [SerializeField] public SurvivorController survivorController;
    [SerializeField] public Survivor survivor;
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
                        { ESurvivorState.Default, new DefaultState(survivor, survivorController, survivorStateManager)},
                        { ESurvivorState.Idling, new IdlingState(survivor, survivorController, survivorStateManager)},
                        { ESurvivorState.Gathering, new GatheringState(survivor, survivorController, survivorStateManager)},
                        { ESurvivorState.Building, new BuildingState(survivor, survivorController, survivorStateManager)},
                        { ESurvivorState.GoingToBase, new GoingToBaseState(survivor, survivorController, survivorStateManager)}
             };
        }
    }
    [Button("Set Start State")]
    private void SetStartState()
    {
        // ChangeState(startState);
    }
}
