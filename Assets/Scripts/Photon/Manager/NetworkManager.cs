using UnityEngine;
using System.Collections;
using Photon;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Use this for initialization
    void Start()
    {

    }

    public override void OnJoinedRoom()
    {
        print("éeghveuezhj");

    }

    void Update()
    {
       
    }
}