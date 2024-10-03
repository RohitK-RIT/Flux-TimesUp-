using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Melee
{
    public class MeleeWeapon : Weapon<MeleeWeaponStats>
    {
        protected override IEnumerator AttackCoroutine()
        {
            while (true)
            {
                Attack();
                
                yield return new WaitForSeconds(1 / stats.AttackSpeed);
            }
        }

        private void Attack()
        {
            // Do the attack
            // Check if the attack hit something
            // If it did, apply damage
        }
    }
}