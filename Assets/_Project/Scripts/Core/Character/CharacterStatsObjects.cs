using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    [CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Stats/CharacterStats")]
    public class CharacterStatsObjects : ScriptableObject
    {
        public int maxHealth;
        public int movementSpeed;
        public int reloadSpeed;
        public int aimAccuracy;
        public int cooldown;
    }
}
