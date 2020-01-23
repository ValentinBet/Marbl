using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyChat : MonoBehaviour
{

    private PhotonView PV;

    private static LobbyChat _instance;
    public static LobbyChat Instance { get { return _instance; } }

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void SendMessageString(string value)
    {
        PV.RPC("RpcLobbyChat", RpcTarget.AllViaServer, value);
    }

    [PunRPC]
    void RpcLobbyChat(string _text)
    {
        ChatManager.Instance.OnChatMessage(_text);
    }

    public void DestroyThis()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

}
