using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneScreen : MonoBehaviourPunCallbacks
{
    public GameObject waitingScreen;

    public GameObject playerElementPrefab;

    public Transform parent;

    List<PlayerListElement> allPlayerElement = new List<PlayerListElement>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Player _p in PhotonNetwork.PlayerList)
        {
            GameObject newPlayerObj = Instantiate(playerElementPrefab, parent);
            PlayerListElement newPlayerListElement = newPlayerObj.GetComponent<PlayerListElement>();

            newPlayerListElement.myPlayer = _p;
            newPlayerListElement.nameText.text = _p.NickName;
            allPlayerElement.Add(newPlayerListElement);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        foreach (PlayerListElement _p in allPlayerElement)
        {
            if (target.GetPlayerTurnState())
            {
                _p.statutText.text = "Ready";
                _p.statutText.color = Color.green;
            }
        }

        if (CheckAllHaveLoadMap())
        {
            waitingScreen.SetActive(false);
        }
    }

    bool CheckAllHaveLoadMap()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!p.GetPlayerMapState())
            {
                return false;
            }
        }
        return true;
    }
}
