using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientPanel : MonoBehaviourPunCallbacks
{
    public Text generalInfo;
    public Text ballInfo;
    public Text dmInfo;
    public Text kothInfo;
    public Text hueInfo;

    public GameObject deathMatch;
    public GameObject koth;
    public GameObject hue;

    public List<MapElement> allMaps = new List<MapElement>();


    // Start is called before the first frame update
    void Start()
    {
        RefreshGamemode();
        line.position = currentChoice.position;
    }

    private void OnEnable()
    {
        RefreshGamemode();
        RefreshMaps();
    }

    void RefreshMaps()
    {
        foreach(MapElement _map in allMaps)
        {
            if(_map.nameMap == PhotonNetwork.CurrentRoom.GetMap())
            {
                _map.SetMap();
                return;
            }
        }
    }


    void RefreshGamemode()
    {
        //General
        generalInfo.text = "Turn time : " + PhotonNetwork.CurrentRoom.GetTurnLimit() + "s" + "\n";

        if(PhotonNetwork.CurrentRoom.GetRoundProp() == 21)
        {
            generalInfo.text += "Round number : ∞";
        }
        else
        {
            generalInfo.text += "Round number : " + PhotonNetwork.CurrentRoom.GetRoundProp();
        }


        //Ball settings
        ballInfo.text = "Ball number by team : " + PhotonNetwork.CurrentRoom.GetNbrbBallProp() + "\n" +
            "Spawn mode : ";

        switch (PhotonNetwork.CurrentRoom.GetSpawnMode())
        {
            case 0:
                ballInfo.text += "team";
                break;

            case 1:
                ballInfo.text += "random";
                break;
        }

        ballInfo.text += "\n" + "Launch power : " + PhotonNetwork.CurrentRoom.GetLaunchPower() + "\n" +
             "Impact power : " + PhotonNetwork.CurrentRoom.GetImpactPower();

        //Deathmatch
        if (PhotonNetwork.CurrentRoom.GetDeathmatch())
        {
            deathMatch.SetActive(true);

            dmInfo.text = "Win points : " + PhotonNetwork.CurrentRoom.GetWinPointDM() + "\n" +
             "Kill points : " + PhotonNetwork.CurrentRoom.GetElimPointDM() + "\n" +
             "Killstreak multiplayer : ";

            switch (PhotonNetwork.CurrentRoom.GetKillstreakDMProp())
            {
                case true:
                    dmInfo.text += " Yes";
                    break;

                case false:
                    dmInfo.text += " No";
                    break;
            }

            dmInfo.text += "\n" + "Suicide points : " + PhotonNetwork.CurrentRoom.GetSuicidePointDM();
        }
        else
        {
            deathMatch.SetActive(false);
        }

        //Hill
        if (PhotonNetwork.CurrentRoom.GetHill())
        {
            koth.SetActive(true);

            kothInfo.text = "Number of area : " + PhotonNetwork.CurrentRoom.GetNbrHill() + "\n" +
             "Gamemode : ";

            switch (PhotonNetwork.CurrentRoom.GetHillMode())
            {
                case 0:
                    kothInfo.text += " One for one";
                    break;

                case 1:
                    kothInfo.text += " Contest";
                    break;

                case 2:
                    kothInfo.text += " Domination";
                    break;
            }

            kothInfo.text += "\n" + "Points in area : " + PhotonNetwork.CurrentRoom.GetHillPoint();
        }
        else
        {
            koth.SetActive(false);
        }

        //Hue
        if (PhotonNetwork.CurrentRoom.GetHue())
        {
            hue.SetActive(true);

            hueInfo.text = "Number of neutral balls : " + PhotonNetwork.CurrentRoom.GetHueNutralBall();
        }
        else
        {
            hue.SetActive(false);
        }
    }


    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        RefreshGamemode();
        RefreshMaps();
    }

    //--------------------------Gestion label--------------------
    [Header("Top")]
    public RectTransform currentChoice;
    public Text currentText;
    List<Text> oldTexts = new List<Text>();
    List<Text> removeTexts = new List<Text>();

    public RectTransform line;

    [Header("panel")]
    public GameObject PlayersList;
    public GameObject GameMode;
    public GameObject Map;

    // Update is called once per frame
    void Update()
    {
        line.position = Vector2.MoveTowards(line.position, currentChoice.position, 3000 * Time.deltaTime);

        removeTexts.Clear();
        foreach (Text oldText in oldTexts)
        {
            oldText.color = Color.Lerp(oldText.color, Color.white, Mathf.PingPong(Time.time, 3));

            if (oldText.color == Color.white)
            {
                removeTexts.Add(oldText);
            }
        }

        foreach (Text text in removeTexts)
        {
            oldTexts.Remove(text);
        }

        currentText.color = Color.Lerp(currentText.color, new Color(1, 0.1986281f, 0), Mathf.PingPong(Time.time, 3));
    }

    public void SetChoice(RectTransform element)
    {
        Text newtext = element.GetComponent<Text>();

        if (currentChoice != newtext)
        {
            oldTexts.Add(currentText);
            currentChoice = element;
            currentText = newtext;

            switch (newtext.text)
            {
                case "Players":
                    PlayersList.SetActive(true);
                    GameMode.SetActive(false);
                    Map.SetActive(false);
                    break;

                case "Gamemode":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(true);
                    Map.SetActive(false);
                    break;

                case "Map":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(false);
                    Map.SetActive(true);
                    break;
            }
        }
    }
}
