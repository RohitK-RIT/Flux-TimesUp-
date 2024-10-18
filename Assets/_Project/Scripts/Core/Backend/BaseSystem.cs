using UnityEngine;

namespace _Project.Scripts.Core.Backend
{
    /// <summary>
    /// Base class for all systems in the game.
    /// </summary>
    public abstract class BaseSystem<T> : MonoBehaviour where T : BaseSystem<T>
    {
        /// <summary>
        /// The instance of the system.
        /// </summary>
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            // If there is already an instance of the system, destroy the new one.
            if (Instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            
            // If not, set the instance to this system and make it persistent between scenes.
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}