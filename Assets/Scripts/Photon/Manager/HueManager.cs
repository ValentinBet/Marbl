using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
using static Photon.Pun.UtilityScripts.PunTeams;
using Photon.Realtime;

public class HueManager : MonoBehaviour
{
    private static HueManager _instance;
    public static HueManager Instance { get { return _instance; } }

    List<Team> listTeamInGame = new List<Team>();

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

    private void InitNeutralBalls()
    {
        List<Transform> spawnPos = MarblFactory.GetListOfAllChild(GameModeManager.Instance.listNeutralPos);
        spawnPos = MarblFactory.ShuffleList(spawnPos);

        for (int i = 0; i < PhotonNetwork.CurrentRoom.GetHueNutralBall(); i++)
        {
            GameObject _neutralBall = PhotonNetwork.Instantiate("Marbl", spawnPos[0].position, Quaternion.identity);
            _neutralBall.GetComponent<BallSettings>().myteam = MarblGame.GetTeam(4);
            spawnPos.Remove(spawnPos[0]);
        }

        foreach(Team team in GameModeManager.Instance.presentTeam)
        {
            PhotonNetwork.CurrentRoom.AddTeamScore(team, PhotonNetwork.CurrentRoom.GetNbrbBallProp());
        }
    }

    public void EndGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            listTeamInGame = GetTeamsInGame();

            foreach (Team team in listTeamInGame)
            {
                PhotonNetwork.CurrentRoom.AddTeamScore(team,GetBallNumber(team));
            }
        }
    }

    public List<Team> GetTeamsInGame()
    {
        List<Team> _tempTeamInGame = new List<Team>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!_tempTeamInGame.Contains(p.GetTeam()))
            {
                _tempTeamInGame.Add(p.GetTeam());
            }
        }

        return _tempTeamInGame;
    }

    public int GetBallNumber(Team team)
    {
        int count = 0;

        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject b in _Balls)
        {
            if (b.GetComponent<BallSettings>() != null)
            {
                if (team == b.GetComponent<BallSettings>().myteam)
                {
                    count++;
                } 
            }
        }

        return count;
    }

    public void ActiveThisMode(bool value)
    {
        if (value)
        {
            InitNeutralBalls();
            return;
        }
        else
        {
            enabled = false;
        }
    }

    public void OnCollisionBall(Team oldTeam, Team newTeam)
    {
        if(oldTeam != Team.neutral)
        {
            PhotonNetwork.CurrentRoom.AddTeamScore(oldTeam, -1);
        }

        if (newTeam != Team.neutral)
        {
            PhotonNetwork.CurrentRoom.AddTeamScore(newTeam, +1);
        }
    }
}



//public bool IsMultipleTeamAreAlive()
//{
//    listTeamAlive.Clear();

//    GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");

//    foreach (GameObject ball in _Balls)
//    {
//        BallSettings _tempBallSettings;
//        _tempBallSettings = ball.GetComponent<BallSettings>();

//        if (listTeamAlive.Count < 2)
//        {
//            if (!listTeamAlive.Contains(_tempBallSettings.myteam))
//            {
//                listTeamAlive.Add(_tempBallSettings.myteam);
//            }
//        }
//        else
//        {
//            return true;
//        }
//    }

//    if (listTeamAlive.Count < 2)
//    {
//        return false;
//    }
//    else
//    {
//        return true;
//    }
// }
