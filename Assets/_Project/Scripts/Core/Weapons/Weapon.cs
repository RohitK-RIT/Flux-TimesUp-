using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Weapons
{
    public abstract class Weapon<T> : MonoBehaviour where T: WeaponStats
    {
        [FormerlySerializedAs("weaponStats")] [SerializeField] protected T stats;

        protected Coroutine AttackingCoroutine;
        
        public virtual float GetDamage() => stats.Damage;

        public void AttackBeing()
        {
            // End the previous attack if it's still running
            AttackEnd();
            // Start the new attack
            AttackingCoroutine = StartCoroutine(AttackCoroutine());
        }

        public void AttackEnd()
        {
            // End the previous attack if it's still running
            if(AttackingCoroutine != null)
                StopCoroutine(AttackingCoroutine);

            AttackingCoroutine = null;
        }
        
        protected abstract IEnumerator AttackCoroutine();
    }
}
