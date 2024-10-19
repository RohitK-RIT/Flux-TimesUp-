using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// Class to store character's stats and handle damage, healing, and death.
    /// ScriptableObject is used to create an asset that can be reused across multiple objects.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Stats/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        [SerializeField] internal int maxHealth = 100;
        public float movementSpeed;
        public float reloadSpeed;
        public float aimAccuracy;
        public int cooldown;
        
        private void Awake()
        {
            // Initialize current health to max health when the game starts
        }

        
        
        
    }
}
