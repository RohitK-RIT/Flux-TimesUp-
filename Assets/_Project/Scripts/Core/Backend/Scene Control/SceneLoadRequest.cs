using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    public struct SceneLoadRequest
    {
        public readonly string SceneName;
        public readonly LoadSceneMode Mode;

        public SceneLoadRequest(string sceneName, LoadSceneMode mode)
        {
            SceneName = sceneName;
            Mode = mode;
        }
    }
}