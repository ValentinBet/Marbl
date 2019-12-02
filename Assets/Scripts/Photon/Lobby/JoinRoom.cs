using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour
{
    public Text roomName;

    public void JoinThisRoom()
    {
        Lobby.lobby.ConnectToThisRoom(roomName.text);
    }
}
