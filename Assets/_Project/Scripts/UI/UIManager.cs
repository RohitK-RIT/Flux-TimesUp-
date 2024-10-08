using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        // Dictionary to store all UI pages
        private Dictionary<string, UIPage> _pages;
   
        // Reference to the current page
        private UIPage _currentPage;
   
        private void Start()
        {
            // Initialize the dictionary
            _pages = new Dictionary<string, UIPage>();
   
            // Find all UI pages in the scene
            UIPage[] allPages = FindObjectsOfType<UIPage>();
            foreach (UIPage page in allPages)
            {
                page.Initialize();
                _pages.Add(page.name, page);  // Add the page to the dictionary
                page.Hide();  // Hide the page
            }
        }
        // Method to switch between pages
        public void SwitchPage(string pageName)
        {
            if (_currentPage != null)
            {
                _currentPage.Hide();
            }
            if (_pages.ContainsKey(pageName))
            {
                _currentPage = _pages[pageName];
                _currentPage.Show();
            }
            else
            {
                Debug.LogWarning($"UI Page {pageName} not found!");
            }
        }
   
        // Update method to update the current page
        private void Update()
        {
            if (_currentPage != null)
            {
                _currentPage.UpdatePage();
            }
        }
    }
}

