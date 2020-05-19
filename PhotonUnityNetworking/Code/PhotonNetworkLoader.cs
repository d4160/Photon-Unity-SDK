using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Photon.Pun
{
    public static partial class PhotonNetwork
    {
        /// <summary>This method wraps loading a level asynchronously and pausing network messages during the process.</summary>
        /// <remarks>
        /// While loading levels in a networked game, it makes sense to not dispatch messages received by other players.
        /// LoadLevel takes care of that by setting PhotonNetwork.IsMessageQueueRunning = false until the scene loaded.
        ///
        /// To sync the loaded level in a room, set PhotonNetwork.AutomaticallySyncScene to true.
        /// The Master Client of a room will then sync the loaded level with every other player in the room.
        /// Note that this works only for a single active scene and that reloading the scene is not supported.
        /// The Master Client will actually reload a scene but other clients won't.
        ///
        /// You should make sure you don't fire RPCs before you load another scene (which doesn't contain
        /// the same GameObjects and PhotonViews).
        ///
        /// LoadLevel uses SceneManager.LoadSceneAsync().
        ///
        /// Check the progress of the LevelLoading using PhotonNetwork.LevelLoadingProgress.
        ///
        /// Calling LoadLevel before the previous scene finished loading is not recommended.
        /// If AutomaticallySyncScene is enabled, PUN cancels the previous load (and prevent that from
        /// becoming the active scene). If AutomaticallySyncScene is off, the previous scene loading can finish.
        /// In both cases, a new scene is loaded locally.
        /// </remarks>
        /// <param name='levelNumber'>
        /// Build-index number of the level to load. When using level numbers, make sure they are identical on all clients.
        /// </param>
        public static void LoadLevel(int levelNumber, LoadSceneMode loadSceneMode)
        {
            if (PhotonNetwork.AutomaticallySyncScene)
            {
                SetLevelInPropsIfSynced(levelNumber);
            }

            PhotonNetwork.IsMessageQueueRunning = false;
            loadingLevelAndPausedNetwork = true;
            _AsyncLevelLoadingOperation = SceneManager.LoadSceneAsync(levelNumber, loadSceneMode);
        }

        /// <summary>This method wraps loading a level asynchronously and pausing network messages during the process.</summary>
        /// <remarks>
        /// While loading levels in a networked game, it makes sense to not dispatch messages received by other players.
        /// LoadLevel takes care of that by setting PhotonNetwork.IsMessageQueueRunning = false until the scene loaded.
        ///
        /// To sync the loaded level in a room, set PhotonNetwork.AutomaticallySyncScene to true.
        /// The Master Client of a room will then sync the loaded level with every other player in the room.
        /// Note that this works only for a single active scene and that reloading the scene is not supported.
        /// The Master Client will actually reload a scene but other clients won't.
        ///
        /// You should make sure you don't fire RPCs before you load another scene (which doesn't contain
        /// the same GameObjects and PhotonViews).
        ///
        /// LoadLevel uses SceneManager.LoadSceneAsync().
        ///
        /// Check the progress of the LevelLoading using PhotonNetwork.LevelLoadingProgress.
        ///
        /// Calling LoadLevel before the previous scene finished loading is not recommended.
        /// If AutomaticallySyncScene is enabled, PUN cancels the previous load (and prevent that from
        /// becoming the active scene). If AutomaticallySyncScene is off, the previous scene loading can finish.
        /// In both cases, a new scene is loaded locally.
        /// </remarks>
        /// <param name='levelName'>
        /// Name of the level to load. Make sure it's available to all clients in the same room.
        /// </param>
        public static void LoadLevel(string levelName, LoadSceneMode loadSceneMode)
        {
            if (PhotonNetwork.AutomaticallySyncScene)
            {
                SetLevelInPropsIfSynced(levelName);
            }

            PhotonNetwork.IsMessageQueueRunning = false;
            loadingLevelAndPausedNetwork = true;
            _AsyncLevelLoadingOperation = SceneManager.LoadSceneAsync(levelName, loadSceneMode);
        }

        public static AsyncOperation LoadLevelAsync(int levelNumber, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (PhotonNetwork.AutomaticallySyncScene)
            {
                SetLevelInPropsIfSynced(levelNumber);
            }

            PhotonNetwork.IsMessageQueueRunning = false;
            loadingLevelAndPausedNetwork = true;
            _AsyncLevelLoadingOperation = SceneManager.LoadSceneAsync(levelNumber, loadSceneMode);

            return _AsyncLevelLoadingOperation;
        }

        public static AsyncOperation LoadLevelAsync(string levelName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            if (PhotonNetwork.AutomaticallySyncScene)
            {
                SetLevelInPropsIfSynced(levelName);
            }

            PhotonNetwork.IsMessageQueueRunning = false;
            loadingLevelAndPausedNetwork = true;
            _AsyncLevelLoadingOperation = SceneManager.LoadSceneAsync(levelName, loadSceneMode);

            return _AsyncLevelLoadingOperation;
        }

    }
}