using UnityEngine;

namespace _Project.Scripts.UI
{
    public abstract class UIPage : MonoBehaviour
    {
        // Flag to check if the page is active
        private bool _isActive;

        // Method to show the page
        public abstract void Show();

        public abstract void Hide();

        // Optional: Initialization logic for the page
        public virtual void Initialize()
        {
            // Initialization logic here, if required
        }

        // Optional: Update logic for the page
        public virtual void UpdatePage()
        {
            if (_isActive)
            {
                // Update logic here, if required
            }
        }

        // Method to check if the page is active
        public bool IsActive()
        {
            return _isActive;
        }
    }
}
