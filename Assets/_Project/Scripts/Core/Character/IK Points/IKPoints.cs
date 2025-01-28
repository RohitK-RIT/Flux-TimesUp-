using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKPoints : MonoBehaviour
{
    public GameObject rigRoot; // Reference to the root of the rig (or parent containing the constraints)
    public GameObject prefab;  // Reference to the prefab containing the transforms

    void Start()
    {
        if (rigRoot == null || prefab == null)
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
        
        // Assign transforms to each Two Bone IK Constraint
        foreach (var ikConstraint in ikConstraints)
        {
            // // Example: Dynamically fetch transforms based on naming conventions or hierarchy paths
            // string constraintName = ikConstraint.gameObject.name; // Name of the GameObject with the constraint
            //
            // Debug.LogWarning($"Transforms for constraint {constraintName} could not be found in the prefab!");
            // // Fetch source, target, and hint transforms based on the prefab structure
            // //Transform sourceObject = prefab.transform.Find($"IK Points/{constraintName}_target");
            // Transform targetObject = prefab.transform.Find($"EmptyGameObject/{constraintName}_target");
            // Transform hintObject = prefab.transform.Find($"EmptyGameObject/{constraintName}_hint");
            //
            // if (hintObject == null || targetObject == null)
            // {
            //     Debug.LogWarning($"Transforms for constraint {constraintName} could not be found in the prefab!");
            //     continue;
            // }
            //
            // // Assign the transforms to the constraint
            // ikConstraint.data.target = targetObject;
            // ikConstraint.data.hint = hintObject; // Optional, can be null
            //
            // Debug.Log($"Assigned transforms for {constraintName} successfully.");
        }
    }
}
