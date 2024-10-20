using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Character;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private MovementController _movementController;
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update(){
        followTarget.transform.rotation *= Quaternion.AngleAxis(_movementController.LookDirection.x * Time.deltaTime * 25f, Vector3.up);
        followTarget.transform.rotation *= Quaternion.AngleAxis(_movementController.LookDirection.y * Time.deltaTime * 25f, Vector3.right);
        var angles = followTarget.transform.localEulerAngles;
        angles.z = 0;
        var angle = followTarget.transform.localEulerAngles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        followTarget.transform.localEulerAngles = angles;
        followTarget.transform.localEulerAngles = new Vector3(0, angles.y, 0);
    }
}
