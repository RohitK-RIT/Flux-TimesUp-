using UnityEngine;
using _Project.Scripts.Core.Weapons.Ranged;

namespace _Project.Scripts.Gameplay.Test
{
    public class RangeWeaponTester : MonoBehaviour
    {
        private bool _buttonDown;
        private RangedWeapon _weapon;
        private void Start()
        {
            _weapon = GetComponent<RangedWeapon>();
        }
        
        private void Update()
        {
            if(!_buttonDown && Input.GetMouseButtonDown(0))
            {
                _buttonDown = true;
                _weapon.BeginAttack();
                
            }
            
            if(_buttonDown && Input.GetMouseButtonUp(0))
            {
                _buttonDown = false;
                _weapon.EndAttack();
            }

            if (Input.GetMouseButton(1))
            {
                _weapon.SwitchFireMode();
            }
        }
    }
}