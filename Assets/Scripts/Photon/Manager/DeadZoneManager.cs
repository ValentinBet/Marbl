using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneManager : MonoBehaviour
{
    public List<PunTeams.Team> listTeamsDeath = new List<PunTeams.Team>();

    public bool localPlayerTurn = false;
    public PunTeams.Team localPlayerTeam;

    List<GameObject> allBalls = new List<GameObject>();

    bool gameFinish = false;


    bool DMactif = false;
    int winPoint = 0;
    int killPoint = 0;
    int sucidePoint = 0;
    bool killstreak = false;
    int killThisRound = 0;
    int sucideThisRound = 0;


    private static DeadZoneManager _instance;
    public static DeadZoneManager Instance { get { return _instance; } }

    void Awake()
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

    private void Start()
    {
        DMactif = PhotonNetwork.CurrentRoom.GetDeathmatch();

        if (DMactif)
        {
            killPoint = PhotonNetwork.CurrentRoom.GetElimPointDM();
            sucidePoint = PhotonNetwork.CurrentRoom.GetSuicidePointDM();
            killstreak = PhotonNetwork.CurrentRoom.GetKillstreakDMProp();
            winPoint = PhotonNetwork.CurrentRoom.GetWinPointDM();
        }
    }

    public void NewTrun()
    {
        killThisRound = 0;
        sucideThisRound = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball" && !gameFinish)
        {
            PunTeams.Team _ballTeam = other.gameObject.GetComponent<BallSettings>().myteam;
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
                            PunTeams.Team _currentTeam = (PunTeams.Team)PhotonNetwork.CurrentRoom.CustomProperties["turn"];
                            string _messageKill = "<color=" + _currentTeam + ">" + StringExtensions.FirstCharToUpper(_currentTeam.ToString()) + "</color> eject marbl of <color=" + _ballTeam + ">" + StringExtensions.FirstCharToUpper(_ballTeam.ToString()) + "</color> ";
                            GameModeManager.Instance.SendKillFeedMessage(_messageKill);
                            break;
                        }
                    }
                    catch { }
                }

                PhotonView ballPV = other.gameObject.GetPhotonView();
                ballPV.TransferOwnership(PhotonNetwork.LocalPlayer);

                PhotonNetwork.Destroy(ballPV);


                GetMyBalls();

                if(GetNumberBallOfTeam(_ballTeam) == 0)
                {
                    listTeamsDeath.Add(_ballTeam);

                    string _messageEliminated = "<color=" + _ballTeam + ">" + StringExtensions.FirstCharToUpper(_ballTeam.ToString()) + " Team</color> eliminated !";
                    GameModeManager.Instance.SendKillFeedMessage(_messageEliminated);
                }

                DetectEndGame();
            }
        }
    }

    void GetMyBalls()
    {
        GameObject[] Balls = GameObject.FindGameObjectsWithTag("Ball");
        allBalls.Clear();
        allBalls.AddRange(Balls);
    }

    int GetNumberBallOfTeam(PunTeams.Team team)
    {
        int number = 0;

        foreach(GameObject element in allBalls)
        {
            if(element.GetComponent<BallSettings>().myteam == team)
            {
                number++;
            }
        }

        return number;
    }

    void DetectEndGame()
    {
        List<PunTeams.Team> allTeamAlive = new List<PunTeams.Team>();

        foreach(GameObject element in allBalls)
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
            foreach(PunTeams.Team team in listTeamsDeath)
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
