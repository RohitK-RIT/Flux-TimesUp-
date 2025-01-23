using _Project.Scripts.Core.Weapons;

namespace _Project.Scripts.Core.Backend.Interfaces
{
    public interface IDamageable
    {
        public void TakeDamage(Weapon weapon, float damage);
    }
}