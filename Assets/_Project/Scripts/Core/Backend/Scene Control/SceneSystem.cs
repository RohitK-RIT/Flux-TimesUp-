﻿using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend.Scene_Control
{
    /// <summary>
    /// System for loading scenes.
    /// </summary>
    public class SceneSystem : BaseSystem<SceneSystem>
    {
        protected override bool IsPersistent => true;
        internal NetworkSceneManagerDefault NetworkSceneManager { get; private set; }

        /// <summary>
        /// Name of the loading scene.
        /// </summary>
        [SerializeField] private string loadingSceneName = "Loading Scene";

        /// <summary>
        /// Coroutine for loading a scene.
        /// </summary>
        private Coroutine _loadSceneCoroutine;

        protected override void Awake()
        {
            base.Awake();

            // Create a new gameobject for the network scene manager.
            var sceneManagerGameobject = new GameObject("Network Scene Manager");
            sceneManagerGameobject.transform.SetParent(transform);
            NetworkSceneManager = sceneManagerGameobject.AddComponent<NetworkSceneManagerDefault>();
        }

        /// <summary>
        /// Load a scene.
        /// </summary>
        /// <param name="request">struct describing scene detail to be loaded</param>
        public void LoadScene(SceneLoadRequest request)
        {
            // If there is a scene loading coroutine running, stop it.
            if (_loadSceneCoroutine != null)
                StopCoroutine(_loadSceneCoroutine);

            // Start a new coroutine to load the scene.
            _loadSceneCoroutine = StartCoroutine(LoadSceneCoroutine(request));
        }

        /// <summary>
        /// Coroutine for loading a scene.
        /// </summary>
        /// <param name="request">struct describing scene detail to be loaded</param>
        private IEnumerator LoadSceneCoroutine(SceneLoadRequest request)
        {
            // Load the loading scene first.
            var sceneOp = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
            yield return sceneOp;


            // Load the requested scene. If the scene is not found, log an error and return.
            sceneOp = SceneManager.LoadSceneAsync(request.SceneName, request.Mode);

            if (sceneOp == null)
            {
                Debug.LogError($"Failed to load scene {request.SceneName}");
                yield break;
            }

            // Else, wait until the requested scene is loaded.
            yield return sceneOp;
        }

        /// <summary>
        /// Coroutine for loading a scene.
        /// </summary>
        /// <param name="request">struct describing scene detail to be loaded</param>
        public async void LoadNetworkScene(SceneLoadRequest request)
        {
            if (!NetworkSystem.Instance.Runner)
            {
                Debug.LogError("Trying to load a scene without a runner");
                return;
            }

            // If the current scene is not the authority, return.
            if (!NetworkSystem.Instance.Runner.IsSceneAuthority)
                return;

            // Load the loading scene first.
            var sceneOp = NetworkSystem.Instance.Runner.LoadScene(loadingSceneName, LoadSceneMode.Additive);
            await sceneOp;

            // Load the requested scene. If the scene is not found, log an error and return.
            sceneOp = NetworkSystem.Instance.Runner.LoadScene(request.SceneName, request.Mode);

            if (!sceneOp.IsValid)
            {
                Debug.LogError($"Failed to load scene {request.SceneName}");
                return;
            }

            // Else, wait until the requested scene is loaded.
            await sceneOp;
        }
    }
}