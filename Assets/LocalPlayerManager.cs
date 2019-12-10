using ExitGames.Client.Photon;
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
    List<GameObject> allBalls = new List<GameObject>();
    public List<GameObject> teamBalls = new List<GameObject>();
    bool canShoot;
    PhotonView PV;
    PUNMouseControl mousControl;
    Team myTeam;
    Color myColorTeam;
    bool haveShoot = false;
    bool haveWait = false;
    bool endCoroutine = true;
    bool blockShoot = false;

    int startTimer;
    float currentTimer = 0;
    TimerInfo myTimerInfo;
    bool doTimer = false;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PhotonNetwork.LocalPlayer.ActorNumber == PV.ControllerActorNr)
        {
            canShoot = false;
            mousControl = GetComponent<PUNMouseControl>();
            mousControl.enabled = true;
            GetComponent<CameraPlayer>().enabled = true;
        }
        else
        {
            this.enabled = false;
            return;
        }

        RemovePlayerTurn();

        DeathMatchManager.Instance.localPlayerTeam = PhotonNetwork.LocalPlayer.GetTeam();

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
        if (PhotonNetwork.LocalPlayer.ActorNumber == PV.ControllerActorNr)
        {
            mousControl.OnShooted += PlayerShooted;
        }
    }

    private void OnDisable()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == PV.ControllerActorNr)
        {
            mousControl.OnShooted -= PlayerShooted;
        }
    }

    private void Update()
    {
        if (!(PhotonNetwork.LocalPlayer.ActorNumber == PV.ControllerActorNr))
        {
            return;
        }

        try
        {
            if(!canShoot && (bool)PhotonNetwork.LocalPlayer.CustomProperties["playerTurn"])
            {
                currentTimer = startTimer;
                doTimer = true;
                myTimerInfo.gameObject.SetActive(true);
                GetMyBalls();
            }

            canShoot = (bool)PhotonNetwork.LocalPlayer.CustomProperties["playerTurn"];
            DeathMatchManager.Instance.localPlayerTurn = canShoot;
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
            currentTimer -= Time.deltaTime;
            myTimerInfo.SetTime(Mathf.RoundToInt(currentTimer));

            if (currentTimer < 0)
            {
                RemovePlayerTurn();
                haveShoot = true;
                doTimer = false;
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
                haveShoot = false;
                haveWait = false;
                RemoveAutority();
                RemovePlayerTurn();
                mousControl.haveShoot = false;
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

    public void YourTurnToPlay()
    {
        GetMyBalls();

        currentTimer = startTimer;
        myTimerInfo.gameObject.SetActive(true);
        doTimer = true;
    }

    public void GetMyBalls()
    {
        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");
        teamBalls.Clear();
        allBalls.Clear();
        allBalls.AddRange(_Balls);
        Debug.Log("refresh");
        foreach (GameObject ball in _Balls)
        {
            if (ball.GetComponent<BallSettings>().myteam == PhotonNetwork.LocalPlayer.GetTeam())
            {
                teamBalls.Add(ball);
                Debug.Log("found");
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
        UIManager.Instance.DisablePing();
        haveShoot = true;
    }

    void RemovePlayerTurn()
    {
        Hashtable _turnPlayer = new Hashtable();
        _turnPlayer["playerTurn"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(_turnPlayer);
        DeathMatchManager.Instance.NewTrun();
    }
}
