using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    public class SceneSystem : BaseSystem<SceneSystem>
    {
        private Coroutine _loadSceneCoroutine;

        public void LoadScene(SceneLoadRequest request)
        {
            if(_loadSceneCoroutine != null)
                StopCoroutine(_loadSceneCoroutine);
            
            _loadSceneCoroutine = StartCoroutine(LoadSceneCoroutine(request));
        }
        
        private static IEnumerator LoadSceneCoroutine(SceneLoadRequest request)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(request.SceneName, request.Mode);
            
            yield return asyncOperation;
        }
    }
}