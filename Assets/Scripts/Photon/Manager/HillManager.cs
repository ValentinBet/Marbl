using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class HillManager : MonoBehaviour
{
    public List<HillObj> allHills = new List<HillObj>();

    int currentMode = 0;
    int currentPoint = 1;


    private static HillManager _instance;
    public static HillManager Instance { get { return _instance; } }

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

    private void Start()
    {
        currentMode = PhotonNetwork.CurrentRoom.GetHillMode();
        currentPoint = PhotonNetwork.CurrentRoom.GetHillPoint();

    }

    public void ActiveThisMode(bool value)
    {
        if (value)
        {
            SpawnHill();
        }
        else
        {
            enabled = false;
        }
    }

    void SpawnHill()
    {
        List<Transform> _spawnPos = new List<Transform>();
        _spawnPos = MarblFactory.GetListOfAllChild(GameObject.Find("HillsPos").transform);

        MarblFactory.ShuffleList(_spawnPos);

        int _nbrHill = PhotonNetwork.CurrentRoom.GetNbrHill();

        for(int i =0; i< _nbrHill; i++)
        {
            GameObject _newBall = PhotonNetwork.Instantiate("Hill", _spawnPos[i].position, _spawnPos[i].rotation);
            allHills.Add(_newBall.GetComponent<HillObj>());
        }
    }

    public void EndTurn()
    {
        print(currentMode);
        switch (currentMode)
        {
            case 0:
                OneForOne();
                break;

            case 1:
                Contest();
                break;

            case 2:
                Domination();
                break;
        }
    }

    void OneForOne()
    {
        foreach(HillObj _hill in allHills)
        {
            foreach(BallSettings _ball in _hill.ballInside)
            {
                PhotonNetwork.CurrentRoom.AddTeamScore(_ball.myteam, currentPoint);
            }
        }
    }

    void Contest()
    {
        foreach (HillObj _hill in allHills)
        {
            if (_hill.ballInside.Count == 0)
            {
                return;
            }

            Team teamAlone = _hill.ballInside[0].myteam;

            foreach (BallSettings _element in _hill.ballInside)
            {
                if(_element.myteam != teamAlone)
                {
                    return;
                }
            }

            PhotonNetwork.CurrentRoom.AddTeamScore(teamAlone, currentPoint);
        }
    }

    void Domination()
    {
        print("test");
        foreach (HillObj _hill in allHills)
        {
            Dictionary<Team, int> teamValue = new Dictionary<Team, int>() { { Team.red, 0 }, { Team.green, 0 }, { Team.blue, 0 }, { Team.yellow, 0 } };

            if (_hill.ballInside.Count == 0)
            {
                return;
            }

            foreach (BallSettings _element in _hill.ballInside)
            {
                teamValue[_element.myteam] += 1;
            }

            Team keyOfMaxValue = teamValue.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            foreach (KeyValuePair<Team, int> _element in teamValue)
            {
                print(_element.Key + " : " + _element.Value);
                if (_element.Key != keyOfMaxValue && _element.Value == teamValue[keyOfMaxValue])
                {
                    return;
                }
            }

            PhotonNetwork.CurrentRoom.AddTeamScore(keyOfMaxValue, currentPoint);
        }
    }
}
