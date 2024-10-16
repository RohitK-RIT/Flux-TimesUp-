using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    [CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Stats/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        public Stat<int> MaxHealth = new Stat<int>(100, 200);
        public Stat<float> MovementSpeed = new Stat<float>(5f, 10f);
        public Stat<int> ReloadSpeed = new Stat<int>(2, 5);
        public Stat<float> AimAccuracy = new Stat<float>(0.9f, 1f);
        public Stat<int> Cooldown = new Stat<int>(1, 3);
    }
}
