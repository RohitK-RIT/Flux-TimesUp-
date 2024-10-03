using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// A projectile that is fired from a ranged weapon.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// The speed at which the projectile travels.
        /// </summary>
        [SerializeField] private float speed;

        /// <summary>
        /// The weapon stats of the weapon that fired the projectile.
        /// </summary>
        private string _weaponID;

        /// <summary>
        /// The starting position of the projectile.
        /// </summary>
        private Vector3 _startPosition;

        /// <summary>
        /// Initializes the projectile.
        /// </summary>
        /// <param name="weaponID">ID of the weapon</param>
        public void Init(string weaponID)
        {
            // Store the weapon stats of the weapon that fired the projectile.
            _weaponID = weaponID;
            var rigidbody = GetComponent<Rigidbody>();
            // Add a force to the projectile to make it move.
            rigidbody.AddForce(Vector3.forward * (speed * rigidbody.mass), ForceMode.Impulse);
        }

        /// <summary>
        /// Called when the projectile collides with another object.
        /// </summary>
        /// <param name="other">The object that the projectile has collided with.</param>
        private void OnCollisionEnter(Collision other)
        {
            // If the object we hit is damageable, deal damage to it.
            // if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
            // {
            //     damageable.TakeDamage(_weaponStats.Damage);
            // }

            Debug.Log("Pew!!");
            Destroy(gameObject);
        }
    }
}