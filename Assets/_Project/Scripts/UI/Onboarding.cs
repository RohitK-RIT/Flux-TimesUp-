using System.Collections;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class Onboarding : MonoBehaviour
    {   
        // Time in seconds before disabling the GameObject
        public float disableTime = 5f;

        private void Start()
        {
            // Start the coroutine to disable the GameObject after a delay
            StartCoroutine(DisableAfterDelay());
        }

        private IEnumerator DisableAfterDelay()
        {
            // Wait for the specified time
            yield return new WaitForSeconds(disableTime);

            // Disable the GameObject
            gameObject.SetActive(false);

            // Optional: Log a message to confirm the object is disabled
            Debug.Log(gameObject.name + " has been disabled.");
        }
    }
}
