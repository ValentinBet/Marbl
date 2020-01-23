using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyChat : MonoBehaviour
{
    private PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
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
