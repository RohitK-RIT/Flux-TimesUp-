using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    public class SceneSystem : BaseSystem<SceneSystem>
    {
        [SerializeField] private string loadingSceneName;

        private Coroutine _loadSceneCoroutine;

        public void LoadScene(SceneLoadRequest request)
        {
            if (_loadSceneCoroutine != null)
                StopCoroutine(_loadSceneCoroutine);

            _loadSceneCoroutine = StartCoroutine(LoadSceneCoroutine(request));
        }

        private IEnumerator LoadSceneCoroutine(SceneLoadRequest request)
        {
            var sceneOp = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
            yield return sceneOp;


            sceneOp = SceneManager.LoadSceneAsync(request.SceneName, request.Mode);

            if (sceneOp == null)
            {
                Debug.LogError($"Failed to load scene {request.SceneName}");
                yield break;
            }

            yield return sceneOp;
        }
    }
}