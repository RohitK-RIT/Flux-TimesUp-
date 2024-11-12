using System;
using System.Linq;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    //General class to manage the game
    public class LevelSceneController : MonoBehaviour
    {
        public static LevelSceneController Instance { get; private set; }
        public int NumberOfEnemies => enemies.Count(controller => controller.gameObject.activeSelf);
        
        [SerializeField] private GameObject pauseMenuPage, winPage, loosePage; // Drag your game scene UI panel here

        [Space(25f), Header("Players in Scene")] [SerializeField]
        private PlayerController player; // Drag your player here

        [SerializeField] private EnemyController[] enemies; // Array to store all enemies in the scene

        private bool _isPaused; // Variable to check if the game is paused

        private void Awake()
        {
            if(Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            // Remove all null elements from the array.
            enemies = Array.FindAll(enemies, enemy => enemy != null);
        }

        private void Update()
        {
            // Check if the player presses the pause key (Escape in this case)
            if (
#if UNITY_EDITOR
                Keyboard.current[Key.M].wasPressedThisFrame
#else
                Keyboard.current[Key.Escape].wasPressedThisFrame
#endif
            )
            {
                if (_isPaused)
                    Resume();
                else
                    Pause();
            }

            if (player.CurrentHealth <= 0) // Check if the player is dead
                GameOver(false);
            else if (Array.TrueForAll(enemies, enemy => enemy.CurrentHealth <= 0)) // Check if all enemies are dead.
                GameOver(true);
        }

        private void OnDestroy()
        {
            // This is to prevent the game from freezing after returning from the menu scene.
            Time.timeScale = 1f; // Unfreeze the game;
        }

        private void GameOver(bool win)
        {
            Cursor.visible = true; // Show the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            if (win)
            {
                winPage.SetActive(true);
            }
            else
            {
                loosePage.SetActive(true);
            }

            Time.timeScale = 0f; // Freeze the game
        }

        // Call this function to resume the game
        public void Resume()
        {
            Cursor.visible = false; // Show the cursor
            Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor
            pauseMenuPage.SetActive(false); // Hide pause menu
            Time.timeScale = 1f; // Unfreeze the game
            _isPaused = false; // Update pause state
        }

        // Call this function to pause the game
        public void Pause()
        {
            Cursor.visible = true; // Show the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            pauseMenuPage.SetActive(true); // Show pause menu
            Time.timeScale = 0f; // Freeze the game
            _isPaused = true; // Update pause state
        }

        // Function to restart the game and go back to the main menu
        public void BackToMainMenu()
        {
            // Unfreeze the game in case it's paused
            Time.timeScale = 1f;

            // Replace "MainMenuScene" with the name of your actual main menu scene
            SceneSystem.Instance.LoadScene(new SceneLoadRequest("UI", LoadSceneMode.Single));
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
        }
    }
}