using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// A class representing a stat with a base value and a list of modifiers.
    /// It calculates the final value of the stat by adding modifiers to the base value.
    /// </summary>
    [Serializable]
    public class Stat
    {
        // The base value of the stat, serialized so it can be adjusted in Unity Inspector.
        [SerializeField] private int baseValue;
        // A list to store modifiers that affect the stat value (e.g., buffs, debuffs).
        private List<int> modifiers = new List<int>();
        
        /// <summary>
        /// Constructor to initialize the base value of the stat.
        /// </summary>
        /// <param name="baseValue">The base value of the stat.</param>
        public Stat(int baseValue)
        {
            this.baseValue = baseValue;
        }
        /// <summary>
        /// Calculates the current value of the stat by adding all modifiers to the base value.
        /// </summary>
        /// <returns>The final value of the stat.</returns>
        public int GetValue()
        {
            int finalValue = baseValue;
            modifiers.ForEach(x => finalValue += x);
            return finalValue;
        }
        /// <summary>
        /// Adds a modifier to the stat, affecting its final value.
        /// </summary>
        /// <param name="modifier">The modifier value to add (can be positive or negative).</param>
        public void AddModifier(int modifier)
        {
            // Only add non-zero modifiers to avoid unnecessary calculations.
            if (modifier != 0)
            {
                modifiers.Add(modifier);
            }
        }
        /// <summary>
        /// Removes a modifier from the stat.
        /// </summary>
        /// <param name="modifier">The modifier value to remove.</param>
        public void RemoveModifier(int modifier)
        {
            // Only remove non-zero modifiers to avoid unnecessary calculations.
            if (modifier != 0)
            {
                modifiers.Remove(modifier);
            }
        }        
    }
}
