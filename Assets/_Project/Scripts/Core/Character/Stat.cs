using System;
using System.Collections.Generic;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// A generic class that represents a stat with a base value and modifiers.
    /// Can be used for any numeric type (int, float, etc.).
    /// </summary>
    /// <typeparam name="T">The type of the stat (e.g., int, float).</typeparam>
    public class Stat<T> where T : struct, IComparable<T>
    {
        // The base value of the stat.
        private T baseValue;

        // The maximum possible value for this stat.
        public T MaxValue;
        // Events to notify other parts of the system when changes occur.
        public event Action<T> OnValueChanged;
        public event Action OnDeath;

        // A list to hold modifiers that temporarily modify the stat value.
        private List<T> modifiers = new List<T>();

        /// <summary>
        /// Constructor to initialize the base and max values of the stat.
        /// </summary>
        /// <param name="baseValue">The starting base value of the stat.</param>
        /// <param name="maxValue">The maximum possible value of the stat.</param>
        public Stat(T baseValue, T maxValue)
        {
            this.baseValue = baseValue;
            this.MaxValue = maxValue;
        }

        /// <summary>
        /// Returns the current value of the stat, including all modifiers.
        /// The final value is clamped between 0 and MaxValue.
        /// </summary>
        /// <returns>The current stat value.</returns>
        public T GetValue()
        {
            // Start with the base value.
            T finalValue = baseValue;
            
            // Add all modifiers to the base value.
            foreach (var modifier in modifiers)
            {
                finalValue = Add(finalValue, modifier);
            }

            // Clamp the value to ensure it doesn't go below 0 or exceed MaxValue.
            return Clamp(finalValue, default(T), MaxValue);
        }

        /// <summary>
        /// Adds a modifier to the stat.
        /// Modifiers can be positive (buffs) or negative (debuffs).
        /// </summary>
        /// <param name="modifier">The value to add as a modifier.</param>
        public void AddModifier(T modifier)
        {
            // Ensure the modifier is not zero before adding it.
            if (!modifier.Equals(default(T))) 
                modifiers.Add(modifier);
            NotifyValueChanged();
        }

        /// <summary>
        /// Removes a modifier from the stat.
        /// </summary>
        /// <param name="modifier">The value of the modifier to remove.</param>
        public void RemoveModifier(T modifier)
        {
            // Ensure the modifier is not zero before attempting to remove it.
            if (!modifier.Equals(default(T))) 
                modifiers.Remove(modifier);
            NotifyValueChanged();
        }
        /// <summary>
        /// Function to take damage by reducing the stat's value.
        /// </summary>
        /// <param name="damage">The amount of damage to apply.</param>
        public void TakeDamage(T damage)
        {
            baseValue = Subtract(baseValue, damage);
            if (GetValue().CompareTo(default(T)) <= 0)
            {
                // If health reaches zero, trigger death event.
                baseValue = default(T); // Ensure base value is not negative.
                OnDeath?.Invoke();
            }
            NotifyValueChanged();
        }

        /// <summary>
        /// Function to heal by increasing the stat's value.
        /// </summary>
        /// <param name="healAmount">The amount of healing to apply.</param>
        public void Heal(T healAmount)
        {
            baseValue = Add(baseValue, healAmount);
            NotifyValueChanged();
        }

        /// <summary>
        /// Helper function to notify subscribers when the stat value changes.
        /// </summary>
        private void NotifyValueChanged()
        {
            OnValueChanged?.Invoke(GetValue());
        }

        /// <summary>
        /// Adds or Subtract two values together. Uses dynamic to support multiple numeric types.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value (modifier).</param>
        /// <returns>The result of adding a and b.</returns>
        private T Add(T a, T b)
        {
            dynamic da = a, db = b;
            return da + db;
        }
        private T Subtract(T a, T b)
        {
            dynamic da = a, db = b;
            return da - db;
        }

        /// <summary>
        /// Clamps a value between a minimum and a maximum.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum possible value (usually 0).</param>
        /// <param name="max">The maximum possible value (usually MaxValue).</param>
        /// <returns>The clamped value.</returns>
        private T Clamp(T value, T min, T max)
        {
            dynamic val = value, minValue = min, maxValue = max;

            // If the value is less than the minimum, return the minimum.
            if (val.CompareTo(minValue) < 0) return min;

            // If the value is greater than the maximum, return the maximum.
            if (val.CompareTo(maxValue) > 0) return max;

            // Otherwise, return the original value.
            return value;
        }
    }
}
