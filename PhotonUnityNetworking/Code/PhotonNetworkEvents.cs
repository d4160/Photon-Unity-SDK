using System;

namespace Photon.Pun
{

    using UnityEngine;

    public static partial class PhotonNetwork
    {
        public static event Action<int> OnSyncLevelLoad;
    }
}