using _Project.Scripts.Core.Character.Weapon_Controller;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace _Project.Scripts.Core.Character.IK_Points
{
    public class IKPoints : MonoBehaviour
    {
        public GameObject rigRoot; // Reference to the root of the rig (or parent containing the constraints)
        //public GameObject prefab;  // Reference to the prefab containing the transforms
        private WeaponController weaponController;

        void Awake()
        {
            weaponController = GetComponent<WeaponController>(); // Fetch the singleton instance of WeaponController
        }
        void Update()
        {
            //Debug.LogError("current weapon ="+weaponController.CurrentWeapon.gameObject.name);
        }

        void Start()
        {
            UpdateIKPoints();
        }
        internal void UpdateIKPoints()
        {
            
            if (rigRoot == null || weaponController == null)
            {
                Debug.LogError("Rig root or prefab is not assigned!");
                return;
            }
            
            // Fetch all Two Bone IK Constraints under the rig root
            TwoBoneIKConstraint[] ikConstraints = rigRoot.GetComponentsInChildren<TwoBoneIKConstraint>();
            
            if (ikConstraints.Length == 0)
            {
                Debug.LogError("No Two Bone IK Constraints found under the rig root!");
                return;
            }
            
            Debug.LogError("current weapon ="+weaponController.CurrentWeapon.gameObject.name);
            // Assign transforms to each Two Bone IK Constraint
            foreach (var ikConstraint in ikConstraints)
            {
                // Example: Dynamically fetch transforms based on naming conventions or hierarchy paths
                string constraintName = ikConstraint.gameObject.name; // Name of the GameObject with the constraint
            
                Debug.LogWarning($"Transforms for constraint {constraintName} could not be found in the prefab!");
                // Fetch source, target, and hint transforms based on the prefab structure
                //Transform sourceObject = prefab.transform.Find($"IK Points/{constraintName}_target");
                Transform targetObject = weaponController.CurrentWeapon.transform.Find($"IK Points/{constraintName}_target");
                Transform hintObject = weaponController.CurrentWeapon.transform.Find($"IK Points/{constraintName}_hint");
                
                Debug.LogWarning($"Transforms for target object {targetObject} could not be found in the prefab!");
                
                if (hintObject == null || targetObject == null)
                {
                    Debug.LogWarning($"Transforms for constraint {constraintName} could not be found in the prefab!");
                    continue;
                }
                
                // Assign the transforms to the constraint
                ikConstraint.data.target = targetObject;
                ikConstraint.data.hint = hintObject; // Optional, can be null
                
                Debug.Log($"Assigned transforms for {constraintName} successfully.");
            }

            RwfreshRig();
        }

        internal void RwfreshRig()
        {
            // Refresh the rig to apply changes
            RigBuilder rigBuilder = rigRoot.GetComponentInParent<RigBuilder>();
            if (rigBuilder != null)
            {
                rigBuilder.Build();
            }
            
            Debug.Log("All Two Bone IK Constraints assigned successfully!");
        }
    }
}
