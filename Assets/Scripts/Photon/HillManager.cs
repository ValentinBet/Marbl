using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _spawnPos.AddRange(GameObject.Find("HillsPos").transform.GetComponentsInChildren<Transform>());

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

    }

    void Domination()
    {

    }
}
