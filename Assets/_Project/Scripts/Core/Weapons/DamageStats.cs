using System;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    [Serializable]
    public struct DamageStats
    {
        [Header("Modifiers")] [SerializeField] private float headModifier;
        [SerializeField] private float bodyModifier;
        [SerializeField] private float legModifier;

        public float HeadModifier => headModifier;
        public float BodyModifier => bodyModifier;
        public float LegModifier => legModifier;
    }
}