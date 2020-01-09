namespace d4160.SceneManagement
{
    using System;
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class PUNSceneManagementSingleton : SceneManagementSingleton
    {
        protected override void NetworkingLoadSceneInternal(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            PhotonNetwork.LoadLevel(sceneName, mode);
        }

        protected override void NetworkingLoadSceneInternal(int sceneBuildIdx, LoadSceneMode mode = LoadSceneMode.Single)
        {
            PhotonNetwork.LoadLevel(sceneBuildIdx, mode);
        }

        protected override AsyncOperation NetworkingLoadSceneAsyncInternal(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return PhotonNetwork.LoadLevelAsync(sceneName, mode);
        }

        protected override AsyncOperation NetworkingLoadSceneAsyncInternal(int sceneBuildIdx, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return PhotonNetwork.LoadLevelAsync(sceneBuildIdx, mode);
        }
    }
}