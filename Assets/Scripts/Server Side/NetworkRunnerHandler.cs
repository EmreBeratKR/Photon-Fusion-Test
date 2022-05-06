using System;
using System.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Server_Side
{
    public class NetworkRunnerHandler : MonoBehaviour
    {
        [SerializeField] private NetworkRunner networkRunnerPrefab;

        private NetworkRunner networkRunner;

        
        private void Init()
        {
            if (networkRunner != null) return;
            
            networkRunner = Instantiate(networkRunnerPrefab);
            networkRunner.name = "Network Runner V1.0";

            var networkSceneProvider = networkRunner.GetComponent<INetworkSceneObjectProvider>();

            var clientTask = StartNetworkRunner
            (
                networkRunner,
                GameMode.AutoHostOrClient,
                NetAddress.Any(),
                0,
                OnInitialized,
                networkSceneProvider
            );
        }


        private Task StartNetworkRunner
        (
            NetworkRunner runner,
            GameMode gameMode,
            NetAddress address,
            SceneRef scene,
            Action<NetworkRunner> initialized,
            INetworkSceneObjectProvider networkSceneProvider
        )
        {
            runner.ProvideInput = true;

            return runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                Address = address,
                Scene = scene,
                SessionName = "Room 101",
                Initialized = initialized,
                SceneObjectProvider = networkSceneProvider
            });
        }

        private void OnInitialized(NetworkRunner runner)
        {
            Debug.Log("Runner has initialized!");
        }


        private void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 200f, 50f), "Start Server"))
            {
                Init();
            }
        }
    }
}
