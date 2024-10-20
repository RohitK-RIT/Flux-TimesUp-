using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core
{
    //General class to manage the game
    public class GameManager : MonoBehaviour
    {
        public GameObject pauseMenuUI;  // Drag your pause menu UI panel here
        private bool _isPaused;          // Variable to check if the game is paused
        private SceneSystem _sceneSystem; // Reference to the SceneSystem

        // Update is called once per frame
        void Update()
        {
            // Check if the player presses the pause key (Escape in this case)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        // Call this function to resume the game
        public void Resume()
        {
            pauseMenuUI.SetActive(false);  // Hide pause menu
            Time.timeScale = 1f;           // Unfreeze the game
            _isPaused = false;              // Update pause state
        }
        // Call this function to pause the game
        public void Pause()
        {
            Cursor.visible = true;          // Show the cursor
            pauseMenuUI.SetActive(true);   // Show pause menu
            Time.timeScale = 0f;           // Freeze the game
            _isPaused = true;               // Update pause state
        }
        // Function to restart the game and go back to the main menu
        public void RestartToMainMenu()
        {
            // Unfreeze the game in case it's paused
            Time.timeScale = 1f;

            // Replace "MainMenuScene" with the name of your actual main menu scene
            _sceneSystem.LoadScene(new SceneLoadRequest("UI", LoadSceneMode.Single));
        }
        //Function to exit the game when the quit button is clicked
        public void QuitGame()
        {
            // Check if we are running in the editor
            #if UNITY_EDITOR
            // If in the editor, stop playing the scene
                UnityEditor.EditorApplication.isPlaying = false;
            #else
            // If in a built version, quit the application
                Application.Quit();
            #endif
                Application.Quit();
        }
    }
}

