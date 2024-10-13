using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    public class TitleSceneController : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName;

        private void Start()
        {
            Invoke(nameof(LoadMainMenuScene), 2.5f);
        }

        private void LoadMainMenuScene()
        {
            SceneSystem.Instance.LoadScene(new SceneLoadRequest(mainMenuSceneName, LoadSceneMode.Single));
        }
    }
}