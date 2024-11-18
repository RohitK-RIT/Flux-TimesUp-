namespace _Project.Scripts.Core.Enemy.FSM
{
    public enum EnemyState 
    {
        Patrol,
        // Represents the state where the enemy searches for the player within detection range
        Detect,
        
        // Represents the state where the enemy follows the player within a certain distance
        Chase,
        
        // Represents the state where the enemy attacks the player once in close range
        Attack
    }
}