using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

[Serializable]
public class WeaponData
{
    // The weapon prefab that will be instantiated in the game
    [SerializeField] internal Weapon weaponPrefab;
    
    [SerializeField] internal Sprite icon;
    
    // An array of weapon stats 
    [SerializeField] internal WeaponStats[] weaponStats;
}
