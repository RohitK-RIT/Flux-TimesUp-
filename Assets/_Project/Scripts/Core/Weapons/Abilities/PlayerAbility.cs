using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public sealed class PlayerAbility : Weapon
    {
        protected override IEnumerator OnAttack()
        {
            // start attack
            yield return null;
        }

        public override void EndAttack()
        {
            base.EndAttack();
            // start cooldown
        }
    }
}