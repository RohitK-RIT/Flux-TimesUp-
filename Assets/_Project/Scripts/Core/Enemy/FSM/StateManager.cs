using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Dictionary<EnemyState, BaseState> States = new Dictionary<EnemyState, BaseState>();
    private BaseState CurrentState;
    private bool isTransitioningState = false;

    void Start()
    {
        CurrentState?.EnterState();
    }

    void Update()
    {
        if (CurrentState == null) return;
        Debug.Log("current state in state manager:"+ CurrentState);
        EnemyState nextStateKey = CurrentState.GetNextState();
        Debug.Log("nextState in state manager:"+ nextStateKey);
        Debug.Log("Current state key in state manager:"+ CurrentState.StateKey);
        if (!isTransitioningState && nextStateKey == CurrentState.StateKey)
        {
            Debug.Log("Check 1:"+CurrentState);
            CurrentState.UpdateState();
        }
        else if (!isTransitioningState)
        {
            Debug.Log("Check 2:"+CurrentState);
            TransitionToState(nextStateKey);
        }
    }

    public void InitializeStates(Dictionary<EnemyState, BaseState> states, EnemyState initialState)
    {
        States = states;
        if (States.ContainsKey(initialState))
        {
            CurrentState = States[initialState];
            CurrentState.EnterState();
        }
        else
        {
            Debug.LogError("Initial state not found in state dictionary.");
        }
    }

    public void TransitionToState(EnemyState stateKey)
    {
        if (!States.ContainsKey(stateKey)) return;

        isTransitioningState = true;
        CurrentState?.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        isTransitioningState = false;
    }
}