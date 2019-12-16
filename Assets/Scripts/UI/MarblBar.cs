using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

public class MarblBar : MonoBehaviour
{
    public GameObject prefab;
    List<Team> presentTeam = new List<Team>();
    public Transform parent;

    Dictionary<Team, ElementTeamBar> obj = new Dictionary<Team, ElementTeamBar>();

    List<GameObject> allBalls = new List<GameObject>();

    Dictionary<Team, int> teamBall = new Dictionary<Team, int>();

    List<Team> allBallsTeam = new List<Team>();

    // Start is called before the first frame update
    void Start()
    {
        CreateTeamList();
        CreateMarblCount();
    }

    // Update is called once per frame
    void Update()
    {
        if(allBalls.Count == 0)
        {
            GetBall();
            return;
        }

        Refresh();

        foreach (KeyValuePair<Team, ElementTeamBar> marblTeam in obj)
        {
            print(teamBall[marblTeam.Key]);
            float newValue = (float) teamBall[marblTeam.Key] * (1000 / allBalls.Count);

            marblTeam.Value.myText.text = teamBall[marblTeam.Key].ToString();
            marblTeam.Value.myRec.sizeDelta = new Vector2(Mathf.Lerp(marblTeam.Value.myRec.sizeDelta.x, newValue, 3),  marblTeam.Value.myRec.sizeDelta.y);
        }
    }
    
    void GetBall()
    {
        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");
        allBalls.AddRange(_Balls);

        if(allBalls.Count == 0) { return; }

        foreach(GameObject ball in allBalls)
        {
            allBallsTeam.Add(ball.GetComponent<BallSettings>().myteam);
        }
    }

    void Refresh()
    {
        teamBall.Clear();
        
        foreach(Team team in presentTeam)
        {
            teamBall.Add(team, 0);
        }

        foreach (Team element in allBallsTeam)
        {
            if(allBallsTeam == null) { continue; }

            teamBall[element] = teamBall[element] + 1;
        }
    }


    void CreateTeamList()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Team _pTeam = p.GetTeam();

            if (!presentTeam.Contains(_pTeam))
            {
                presentTeam.Add(_pTeam);
            }
        }

        if (PhotonNetwork.CurrentRoom.GetHue())
        {
            presentTeam.Add(Team.neutral);
        }
    }

    void CreateMarblCount()
    {
        int numberMarbl = 5 * presentTeam.Count;

        foreach(Team element in presentTeam)
        {
            GameObject newObj = Instantiate(prefab, parent);
            newObj.GetComponent<Image>().color = MarblGame.GetColor((int) element);

            ElementTeamBar newElement = new ElementTeamBar();
            newElement.myRec = newObj.GetComponent<RectTransform>();
            newElement.myText = newObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            newElement.myRec.sizeDelta = new Vector2(100 * 5, newElement.myRec.sizeDelta.y);

            obj.Add(element, newElement);
        }
    }
}

public class ElementTeamBar
{
    public RectTransform myRec;
    public TextMeshProUGUI myText;
}
