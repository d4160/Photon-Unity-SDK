using System.Collections;
using System.Collections.Generic;
using d4160.Networking;
using Photon.Pun;
using UnityEngine;

namespace d4160.GameFramework
{
    public class PUNEntityBehaviour : NetworkingEntityBehaviour
    {
        protected PhotonView _photonView;

        public PhotonView PhotonView
        {
            get
            {
                if (this._photonView == null)
                {
                    this._photonView = PhotonView.Get(this);
                }
                return this._photonView;
            }
        }

        public override bool IsLocal => PhotonView.IsMine;
    }
}