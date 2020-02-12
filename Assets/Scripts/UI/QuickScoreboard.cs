using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class QuickScoreboard : MonoBehaviour
{
    public Transform rankParent;
    List<GameObject> rankObj = new List<GameObject>();

    public Transform teamParent;
    List<GameObject> teamObj = new List<GameObject>();

    public Dictionary<Team, int> teamAndBall = new Dictionary<Team, int>();
    public GameObject[] Balls;


    private static QuickScoreboard _instance;
    public static QuickScoreboard Instance { get { return _instance; } }

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

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in rankParent)
        {
            rankObj.Add(child.gameObject);
        }

        foreach (Transform child in teamParent)
        {
            teamObj.Add(child.gameObject);
        }

        int i = 0;

        foreach(Team element in CreateTeamList())
        {
            rankObj[i].SetActive(true);
            teamObj[i].SetActive(true);
            i++;
        }

        Dictionary<Team, int> listTeams = ScoreboardManager.Instance.GetTeamListSortedByPoints();

        i = 0;
        foreach (KeyValuePair<Team, int> element in listTeams)
        {
            teamObj[i].GetComponent<TeamListElement>().SetElements(element.Key, element.Value, PhotonNetwork.CurrentRoom.GetNbrbBallProp());
            teamAndBall[element.Key] = PhotonNetwork.CurrentRoom.GetNbrbBallProp();
            i++;
        }
    }

    public void Refresh()
    {
        Dictionary<Team, int> listTeams = ScoreboardManager.Instance.GetTeamListSortedByPoints();

        int i = 0;
        foreach(KeyValuePair<Team, int> element in listTeams)
        {
            int ballNumber = CountTeam(element.Key);
            teamObj[i].GetComponent<TeamListElement>().SetElements(element.Key, element.Value, ballNumber);
            teamAndBall[element.Key] = ballNumber;
            i++;
        }
    }
    

    int CountTeam(Team myTeam)
    {
        int number = 0;

        Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in Balls)
        {
            if( ball.GetComponent<BallSettings>().myteam == myTeam)
            {
                number++;
            }
        }

        int neutralBall = 0;
        foreach (GameObject ball in Balls)
        {
            if (ball.GetComponent<BallSettings>().myteam == Team.neutral)
            {
                neutralBall++;
            }
        }
        teamAndBall[Team.neutral] = neutralBall;

        return number;
    }

    List<Team> CreateTeamList()
    {
        List<Team> presentTeam = new List<Team>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Team _pTeam = p.GetTeam();

            if (!presentTeam.Contains(_pTeam))
            {
                presentTeam.Add(_pTeam);
                teamAndBall.Add(_pTeam, 0);
            }
        }

        if (PhotonNetwork.CurrentRoom.GetHue())
        {
            teamAndBall.Add(Team.neutral, 0);
        }

        return presentTeam;
    }

    public int CountBallOfThisTeam(Team myTeam)
    {
        int number = 0;

        Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in Balls)
        {
            if (ball.GetComponent<BallSettings>().myteam == myTeam)
            {
                number++;
            }
        }

        return number;
    }
}
