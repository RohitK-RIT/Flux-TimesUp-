using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    /// <summary>
    /// Controller for the title scene.
    /// </summary>
    public class TitleSceneController : MonoBehaviour
    {
        /// <summary>
        /// Name of the main menu scene.
        /// </summary>
        [SerializeField] private string mainMenuSceneName;

        private void Start()
        {
            // Load the main menu scene after 2.5 seconds.
            Invoke(nameof(LoadMainMenuScene), 2.5f);
        }

        /// <summary>
        /// Load the main menu scene.
        /// </summary>
        private void LoadMainMenuScene()
        {
            SceneSystem.Instance.LoadScene(new SceneLoadRequest(mainMenuSceneName, LoadSceneMode.Single));
        }
    }
}