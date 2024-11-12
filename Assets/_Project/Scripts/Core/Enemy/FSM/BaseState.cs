using UnityEngine;

public abstract class BaseState
{
    public EnemyState StateKey { get; private set; }

    public BaseState(EnemyState key)
    {
        StateKey = key;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EnemyState GetNextState();
}