using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Core.Backend.Scene_Control;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core.Backend
{
    public class NetworkSystem : BaseSystem<NetworkSystem>, INetworkRunnerCallbacks
    {
        protected override bool IsPersistent => true;
        public List<SessionInfo> SessionList { get; private set; } = new();
        public SessionInfo CurrentSessionInfo => Runner.SessionInfo;
        public bool IsSceneAuthority => Runner.IsSceneAuthority;

        internal NetworkRunner Runner { get; private set; }

        private NetworkRunner CreateRunner()
        {
            var runnerGameObject = new GameObject("Network Runner");
            runnerGameObject.transform.SetParent(transform);
            var runner = runnerGameObject.AddComponent<NetworkRunner>();
            runner.ProvideInput = true;

            return runner;
        }

        public async Task<(bool, string)> CreateSession()
        {
            Runner = CreateRunner();
            Runner.ProvideInput = true;

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            // Start or join (depends on gamemode) a session with a specific name
            var result = await Runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Host,
                PlayerCount = 3,
                Scene = scene,
                SceneManager = SceneSystem.Instance.NetworkSceneManager
            });

            // If the session was created successfully, return true and the session name
            if (result.Ok)
                return (true, Runner.SessionInfo.Name);

            // If the session was not created successfully, shutdown the runner and return false with the error message
            await Runner.Shutdown();
            Destroy(Runner.gameObject);
            return (false, result.ErrorMessage);
        }

        public async Task<(bool, string)> JoinSession(string sessionName)
        {
            Runner = CreateRunner();
            Runner.ProvideInput = true;

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            // Join a session with a specific name
            var result = await Runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Client,
                SessionName = sessionName,
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            // If the session was joined successfully, return true and the session name
            if (result.Ok)
                return (true, Runner.SessionInfo.Name);

            // If the session was not joined successfully, shutdown the runner and return false with the error message
            await Runner.Shutdown();
            Destroy(Runner.gameObject);
            return (false, result.ErrorMessage);
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

        public void OnInput(NetworkRunner runner, NetworkInput input) { }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

        public void OnConnectedToServer(NetworkRunner runner) { }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            if (runner == Runner)
                SessionList = sessionList;
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

        public void OnSceneLoadDone(NetworkRunner runner) { }

        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}