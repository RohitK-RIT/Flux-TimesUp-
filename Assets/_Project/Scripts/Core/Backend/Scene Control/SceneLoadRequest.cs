using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    /// <summary>
    /// Struct for loading a scene.
    /// </summary>
    public struct SceneLoadRequest
    {
        /// <summary>
        /// Name of the scene to be loaded.
        /// </summary>
        public readonly string SceneName;
        /// <summary>
        /// Mode in which the scene should be loaded.
        /// </summary>
        public readonly LoadSceneMode Mode;

        /// <summary>
        /// Constructor for the struct.
        /// </summary>
        /// <param name="sceneName">Name of the scene to load</param>
        /// <param name="mode">Load scene mode</param>
        public SceneLoadRequest(string sceneName, LoadSceneMode mode)
        {
            SceneName = sceneName;
            Mode = mode;
        }
    }
}