using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneScreen : MonoBehaviourPunCallbacks
{
    public GameObject waitingScreen;

    public GameObject playerElementPrefab;

    public Transform parent;

    List<PlayerListElement> allPlayerElement = new List<PlayerListElement>();

    //DM
    public GameObject DeathMatch;
    public Text DeathMatchText;

    //KOTH
    public GameObject KingOfTheHill;
    public Text KothLabel;
    public Text OneForOne;
    public Text Contest;
    public Text Domination;

    //Hue
    public GameObject Hue;

    //None
    public GameObject None;

    bool gameStart = false;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

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

        if (PhotonNetwork.CurrentRoom.GetDeathmatch())
        {
            DeathMatch.SetActive(true);
            DeathMatchText.text = "Eject the opposing marbls to earn <color=#ff0000ff>" + PhotonNetwork.CurrentRoom.GetElimPointDM() + " pts</color> and win.";
        }
        else
        {
            DeathMatch.SetActive(false);
        }

        Hue.SetActive(PhotonNetwork.CurrentRoom.GetHue());

        if (PhotonNetwork.CurrentRoom.GetHill())
        {
            string nameHillMode = "";
            int points = PhotonNetwork.CurrentRoom.GetHillPoint();

            switch (PhotonNetwork.CurrentRoom.GetHillMode())
            {
                case 0:
                    nameHillMode = "1 for 1";
                    OneForOne.text = "Earn <color=#ff0000ff>" + points + " pts</color> per ball in an area." + "\n" + "Every end turn.";
                    OneForOne.gameObject.SetActive(true);
                    break;

                case 1:
                    nameHillMode = "Contest";
                    Contest.text = "Be the only team on the area to" + "\n" + "gain <color=#ff0000ff>" + points + " pts</color>. Every end turn.";
                    Contest.gameObject.SetActive(true);
                    break;

                case 2:
                    nameHillMode = "Domination";
                    Domination.text = "The most team marbls in the" + "\n" + "area gain " + "<color=#ff0000ff>" + points + " pts</color>. Every end turn.";
                    Domination.gameObject.SetActive(true);
                    break;
            }
            KingOfTheHill.SetActive(true);
            KothLabel.text = "King of the hill - <color=#606060ff>" + nameHillMode + "</color>";
        }
        else
        {
            KingOfTheHill.SetActive(false);
        }

        if(!DeathMatch.activeSelf && !KingOfTheHill.activeSelf && !Hue.activeSelf)
        {
            None.SetActive(true);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        int numberPlayerReady = 0;
        foreach (PlayerListElement _p in allPlayerElement)
        {
            if (target.GetPlayerMapState())
            {
                _p.statutText.text = "Ready";
                _p.statutText.color = Color.green;
                numberPlayerReady++;
            }
        }

        if(PhotonNetwork.IsMasterClient && numberPlayerReady == PhotonNetwork.PlayerList.Length && !gameStart)
        {
            gameStart = true;
            StartCoroutine(WaitToStart());
        }
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2);
        GameModeManager.Instance.StartGameForAll();
    }
}
