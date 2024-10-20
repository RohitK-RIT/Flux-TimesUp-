using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class MainMenu : UIPage
    {
        [SerializeField] private string gameSceneName;
        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
        
        public void OnStartButtonClicked()
        {
            SceneSystem.Instance.LoadScene(new SceneLoadRequest(gameSceneName, LoadSceneMode.Single));
        }
    }
}
    
