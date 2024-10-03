using System;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    [Serializable]
    [CreateAssetMenu(fileName = "RWS_WeaponName", menuName = "Weapons/Weapon Stats/Ranged", order = 0)]
    public class RangedWeaponStats : WeaponStats
    {
        public FireModes[] FireModes => fireModes;
        public float ReloadTime => reloadTime;
        public float Spread => spread;
        public float Recoil => recoil;
        public int MagazineSize => magazineSize;
        public int BurstAmount => burstAmount;
        public float BurstDuration => burstDuration;

        [Header("Ranged Weapon Stats")] [SerializeField]
        private FireModes[] fireModes = { Ranged.FireModes.Auto };

        [SerializeField] private float reloadTime;
        [SerializeField] private float spread;
        [SerializeField] private float recoil;
        [SerializeField] private int magazineSize;
        [SerializeField] private int burstAmount;
        [SerializeField] private float burstDuration;
    }
}