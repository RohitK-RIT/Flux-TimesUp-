using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    public class Stat : MonoBehaviour
    {
        private int baseValue;
        public int maxValue;
        private List<int> modifiers = new List<int>();

        public Stat(int baseValue)
        {
            this.baseValue = baseValue;
        }

        public int GetValue()
        {
            int finalValue = baseValue;
            foreach (var modifier in modifiers)
            {
                finalValue += modifier;
                finalValue = Mathf.Clamp(baseValue, 0, maxValue);

            }
            return finalValue;
        }

        public void AddModifier(int modifier)
        {
            if (modifier != 0) modifiers.Add(modifier);
        }

        public void RemoveModifier(int modifier)
        {
            if (modifier != 0) modifiers.Remove(modifier);
        }
    }
}
