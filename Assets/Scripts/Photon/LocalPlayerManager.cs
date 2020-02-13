﻿using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;
using static Photon.Pun.UtilityScripts.PunTeams;

public class LocalPlayerManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> allBalls = new List<GameObject>();
    public List<GameObject> teamBalls = new List<GameObject>();
    public bool canShoot;
    PhotonView PV;
    PUNMouseControl mousControl;
    Team myTeam;
    Color myColorTeam;
    public bool haveShoot = false;
    bool haveWait = false;
    bool endCoroutine = true;
    bool blockShoot = false;

    int startTimer;
    float currentTimer = 0;
    TimerInfo myTimerInfo;
    public bool doTimer = false;

    CameraPlayer myPlayerCam;
    Transform camSpec;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            canShoot = false;
            mousControl = GetComponent<PUNMouseControl>();
            mousControl.enabled = true;
            myPlayerCam = GetComponent<CameraPlayer>();
            myPlayerCam.enabled = true;

            GameModeManager.Instance.localPhotonView = PV;
        }
        else
        {
            this.enabled = false;
            return;
        }

        RemovePlayerTurn();

        GameModeManager.Instance.localPlayerTeam = PhotonNetwork.LocalPlayer.GetTeam();
        GameModeManager.Instance.localPlayerObj = gameObject;

        PhotonNetwork.AutomaticallySyncScene = true;

        startTimer = PhotonNetwork.CurrentRoom.GetTurnLimit();
        myTimerInfo = UIManager.Instance.timer;

        myTeam = PhotonNetwork.LocalPlayer.GetTeam();
        myColorTeam = MarblGame.GetColor((int)PhotonNetwork.LocalPlayer.GetTeam());

        myTimerInfo.myImg.color = myColorTeam;
        myTimerInfo.mytext.color = myColorTeam;
    }

    private void OnEnable()
    {
        if (PV.IsMine)
        {
            mousControl.OnShooted += PlayerShooted;
        }
    }

    private void OnDisable()
    {
        if (PV.IsMine)
        {
            mousControl.OnShooted -= PlayerShooted;
        }
    }

    private void Start()
    {
        PhotonNetwork.LocalPlayer.SetPlayerMapState(true);
    }

    private void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        try
        {
            if (!canShoot && (bool)PhotonNetwork.LocalPlayer.CustomProperties["playerTurn"])
            {
                YourTurnToPlay();
            }

            canShoot = (bool)PhotonNetwork.LocalPlayer.CustomProperties["playerTurn"];
            GameModeManager.Instance.localPlayerTurn = canShoot;
        }
        catch
        {
            canShoot = false;
        }

        if (teamBalls.Count == 0)
        {
            GetMyBalls();
        }

        if (canShoot && !haveShoot && doTimer)
        {
            if (!PhotonNetwork.CurrentRoom.GetForceCam())
            {
                currentTimer -= Time.deltaTime;
            }

            myTimerInfo.SetTime(Mathf.RoundToInt(currentTimer));

            if (currentTimer < 0)
            {
                TurnFinish();
            }
        }
        else
        {
            myTimerInfo.gameObject.SetActive(false);
        }

        if (haveShoot)
        {
            doTimer = false;
            if (!haveWait && endCoroutine)
            {
                endCoroutine = false;
                StartCoroutine(WaitAfterShoot());
                return;
            }

            float speedTotal = 0;
            RefreshBall();
            foreach (GameObject _element in allBalls)
            {
                speedTotal += _element.GetComponent<Rigidbody>().velocity.magnitude;
            }

            if (speedTotal <= 0 && haveWait)
            {
                //fin de tour
                TurnFinish();
            }
        }
    }

    IEnumerator WaitAfterShoot()
    {
        yield return new WaitForSeconds(2);
        haveWait = true;
        endCoroutine = true;

    }

    void RemoveAutority()
    {
        foreach (GameObject _element in allBalls)
        {
            _element.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.MasterClient);
        }

    }

    void TurnFinish()
    {
        UIManager.Instance.SetSpecCam();

        haveShoot = true;
        doTimer = false;
        UIManager.Instance.OnEndTurn();
        haveShoot = false;
        haveWait = false;

        RemoveAutority();
        RemovePlayerTurn();
        mousControl.haveShoot = false;
        GetComponent<TrajectoryRenderer>().ResetLandingZone();
    }

    public void YourTurnToPlay()
    {
        if(myPlayerCam.GetCurrentMode() == MarblGame.CameraMode.SpecMode)
        {
            UIManager.Instance.SetSavedCam();
        }

        if(camSpec == null)
        {
            camSpec = CameraManager.Instance.CamSpecNetwork;
        }

        currentTimer = startTimer;
        doTimer = true;
        myTimerInfo.gameObject.SetActive(true);
        GetMyBalls();

        this.GetComponent<PUNMouseControl>().DisableShootInTime();
    }

    public void GetMyBalls()
    {
        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");
        teamBalls.Clear();
        allBalls.Clear();
        allBalls.AddRange(_Balls);
        foreach (GameObject ball in _Balls)
        {
            if (ball.GetComponent<BallSettings>().myteam == PhotonNetwork.LocalPlayer.GetTeam())
            {
                teamBalls.Add(ball);
            }
        }
    }

    void RefreshBall()
    {
        foreach (GameObject element in allBalls)
        {
            if (element == null)
            {
                GetMyBalls();
                return;
            }
        }
    }

    public bool GetCanShoot()
    {
        if (canShoot && !blockShoot)
        {
            return true;
        }

        return false;
    }

    public void SetBlock(bool value)
    {
        blockShoot = value;
    }

    void PlayerShooted()
    {
        haveShoot = true;
    }

    void RemovePlayerTurn()
    {
        Hashtable _turnPlayer = new Hashtable();
        _turnPlayer["playerTurn"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(_turnPlayer);
        DeathMatchManager.Instance.NewTrun();
    }

    public void SendMessageString(string value)
    {
        PV.RPC("RpcChat", RpcTarget.AllViaServer, value);
    }

    [PunRPC]
    void RpcChat(string _text)
    {
        ChatManager.Instance.OnChatMessage(_text);
    }
}
