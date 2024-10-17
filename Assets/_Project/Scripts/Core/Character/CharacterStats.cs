using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    [CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Stats/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        [SerializeField] private int maxHealth = 100;
        public Stat<float> movementSpeed = new Stat<float>(5f, 10f);
        public Stat<int> reloadSpeed = new Stat<int>(2, 5);
        public Stat<float> aimAccuracy = new Stat<float>(0.9f, 1f);
        public Stat<int> cooldown = new Stat<int>(1, 3);
    }
}
