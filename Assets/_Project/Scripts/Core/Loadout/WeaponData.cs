using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

[Serializable]
public class WeaponData
{
    public Weapon weaponPrefab;
    
    public WeaponStats[] weaponStats;
}
