using _Project.Scripts.Core.Character;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget; // Player or the object to follow
        [SerializeField] private MovementController movementController; // Access the movement script of your player
        [SerializeField] private float distance = 2.0f;  // Distance between the camera and player
        [SerializeField] private float height = 1.5f;    // Height offset
        [SerializeField] private float rotationSpeed = 25f;  // Speed for rotating the camera
        [SerializeField] private float minPitch = 40f;   // Minimum pitch (downward rotation)
        [SerializeField] private float maxPitch = 340f;  // Maximum pitch (upward rotation)
    
        private Vector3 _offset; // Offset vector to maintain distance between the camera and the target
    
        void Start()
        {
            // Calculate initial offset based on the current setup
            _offset = new Vector3(0, height, -distance);
        }

        // Update is called once per frame
        void LateUpdate()  // LateUpdate is better for camera updates to ensure all movement logic has been processed first
        {
            // Rotate the follow target based on the player's input (yaw and pitch)
            RotateCamera();

            // Set the position of the camera behind the player based on the offset
            Vector3 desiredPosition = followTarget.position + followTarget.rotation * _offset;
            transform.position = desiredPosition;

            // Look at the follow target (player)
            transform.LookAt(followTarget);
        }

        private void RotateCamera()
        {
            // Handle horizontal rotation (yaw)
            followTarget.rotation *= Quaternion.AngleAxis(movementController.LookDirection.x * Time.deltaTime * rotationSpeed, Vector3.up);

            // Handle vertical rotation (pitch)
            followTarget.rotation *= Quaternion.AngleAxis(movementController.LookDirection.y * Time.deltaTime * rotationSpeed, Vector3.right);

            // Clamp the vertical rotation (pitch) to avoid flipping the camera
            Vector3 angles = followTarget.localEulerAngles;
            angles.z = 0;  // Reset roll

            // Handle pitch clamping for smoother transitions
            float pitch = angles.x;

            if (pitch > 180) pitch -= 360; // Convert 360-degree system to -180 to 180 for clamping

            pitch = Mathf.Clamp(pitch, -minPitch, -maxPitch);

            angles.x = pitch;
            followTarget.localEulerAngles = angles;
        }
    }
}
