using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneManager : MonoBehaviour
{
    bool DMactif = false;

    public void Start()
    {
        DMactif = PhotonNetwork.CurrentRoom.GetDeathmatch();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            if (DMactif)
            {
                DeathMatchManager.Instance.OnMarblDie(other.gameObject);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonView ballPV = other.gameObject.GetPhotonView();
                    ballPV.TransferOwnership(PhotonNetwork.LocalPlayer);
                    PhotonNetwork.Destroy(ballPV);
                }

                GameModeManager.Instance.DetectEndGame();
            }
        }
    }
}
