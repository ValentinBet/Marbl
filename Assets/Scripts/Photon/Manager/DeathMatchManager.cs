using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class DeathMatchManager : MonoBehaviour
{
    public GameObject deathZone;

    public List<Team> listTeamsDeath = new List<Team>();

    public bool localPlayerTurn = false;
    public Team localPlayerTeam;

    List<GameObject> allBalls = new List<GameObject>();

    bool DMactif = false;
    int winPoint = 0;
    int killPoint = 0;
    int sucidePoint = 0;
    bool killstreak = false;
    int killThisRound = 0;
    int sucideThisRound = 0;

    private static DeathMatchManager _instance;
    public static DeathMatchManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ActiveThisMode(bool value)
    {
        if (value)
        {
            
        }
        else
        {
            enabled = false;
        }
    }

    public void GivePoint(List<Team> _presentTeam)
    {
        if (PhotonNetwork.CurrentRoom.GetDeathmatch())
        {
            int winPoint = PhotonNetwork.CurrentRoom.GetWinPointDM();
            int point = 0;

            foreach (Team _team in listTeamsDeath)
            {
                PhotonNetwork.CurrentRoom.AddTeamScore(_team, point);
                point += Mathf.FloorToInt(winPoint / PhotonNetwork.PlayerList.Length);
            }

            foreach (Team _element in _presentTeam)
            {
                if (!listTeamsDeath.Contains(_element))
                {
                    PhotonNetwork.CurrentRoom.AddTeamScore(_element, winPoint);
                }
            }
        }
    }

    public void NewTrun()
    {
        killThisRound = 0;
        sucideThisRound = 0;
    }

    public void OnMarblDie(GameObject marbl)
    {
        if (GameModeManager.Instance.gameFinish) { return; }

        Team _ballTeam = marbl.GetComponent<BallSettings>().myteam;
        if (localPlayerTurn)
        {
            if (DMactif)
            {
                if (_ballTeam == localPlayerTeam)
                {
                    if (killstreak)
                    {
                        sucideThisRound++;
                    }
                    else
                    {
                        sucideThisRound = 1;
                    }

                    PhotonNetwork.LocalPlayer.AddPlayerScore(-1);
                    PhotonNetwork.CurrentRoom.AddTeamScore(localPlayerTeam, sucidePoint * sucideThisRound);
                }
                else
                {
                    if (killstreak)
                    {
                        killThisRound++;
                    }
                    else
                    {
                        killThisRound = 1;
                    }

                    PhotonNetwork.LocalPlayer.AddPlayerScore(1);
                    PhotonNetwork.CurrentRoom.AddTeamScore(localPlayerTeam, killPoint * killThisRound);
                }
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                try
                {
                    if ((bool)p.CustomProperties["playerTurn"])
                    {
                        Team _currentTeam = (Team)PhotonNetwork.CurrentRoom.CustomProperties["turn"];
                        string _messageKill = "<color=" + _currentTeam + ">" + MarblFactory.FirstCharToUpper(_currentTeam.ToString()) + "</color> eject marbl of <color=" + _ballTeam + ">" + MarblFactory.FirstCharToUpper(_ballTeam.ToString()) + "</color> ";
                        GameModeManager.Instance.SendKillFeedMessage(_messageKill);
                        break;
                    }
                }
                catch { }
            }

            PhotonView ballPV = marbl.GetPhotonView();
            ballPV.TransferOwnership(PhotonNetwork.LocalPlayer);

            PhotonNetwork.Destroy(ballPV);


            GetMyBalls();

            if (GetNumberBallOfTeam(_ballTeam) == 0)
            {
                listTeamsDeath.Add(_ballTeam);

                string _messageEliminated = "<color=" + _ballTeam + ">" + MarblFactory.FirstCharToUpper(_ballTeam.ToString()) + " Team</color> eliminated !";
                GameModeManager.Instance.SendKillFeedMessage(_messageEliminated);
            }

            DetectEndGame();
        }
    }

    void GetMyBalls()
    {
        GameObject[] Balls = GameObject.FindGameObjectsWithTag("Ball");
        allBalls.Clear();
        allBalls.AddRange(Balls);
    }

    int GetNumberBallOfTeam(Team team)
    {
        int number = 0;

        foreach (GameObject element in allBalls)
        {
            if (element.GetComponent<BallSettings>().myteam == team)
            {
                number++;
            }
        }

        return number;
    }

    void DetectEndGame()
    {
        List<Team> allTeamAlive = new List<Team>();

        foreach (GameObject element in allBalls)
        {
            PunTeams.Team ballTeam = element.GetComponent<BallSettings>().myteam;
            if (!allTeamAlive.Contains(ballTeam))
            {
                allTeamAlive.Add(ballTeam);
            }
        }

        if (allTeamAlive.Count == 1)
        {
            //fin de partie
            int point = 0;
            foreach (Team team in listTeamsDeath)
            {
                PhotonNetwork.CurrentRoom.AddTeamScore(team, point);
                point += Mathf.FloorToInt(winPoint / PhotonNetwork.PlayerList.Length);
            }

            PhotonNetwork.CurrentRoom.AddTeamScore(allTeamAlive[0], winPoint);

            GameModeManager.Instance.EndMode();
        }
        else
        {
            return;
        }
    }
}
