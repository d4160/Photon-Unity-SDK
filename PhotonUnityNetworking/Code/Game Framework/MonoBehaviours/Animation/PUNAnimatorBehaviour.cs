using System.Collections;
using System.Collections.Generic;
using d4160.Core;
using Photon.Pun;
using UnityEngine;

namespace d4160.GameFramework
{
    public class PUNAnimatorBehaviour : AnimatorBehaviour
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

        public override void SetFloat(int paramIndex, float value)
        {
            if (PhotonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (_paramNames.IsValidIndex(paramIndex))
            {
                _animator.SetFloat(ref _paramNames[paramIndex], value);
            }
        }
    }
}