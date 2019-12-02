using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviourPunCallbacks
{
    public List<Transform> listPos;
    public Transform listPosRandom;
    public string lobbyScene;

    List<string> prefabTeam = new List<string>();

    PhotonView myPV;

    Dictionary<Player, PunTeams.Team> teamPlayer = new Dictionary<Player, PunTeams.Team>();
    List<PunTeams.Team> presentTeam = new List<PunTeams.Team>();

    PunTeams.Team teamPlayed;
    Player playerplayed;

    List<Player> playerAlreadyPlay = new List<Player>();
    int indexTeamPlaying = 0;

    bool turnStart = false;
    bool gameFinish = false;

    public Text playerTurnText;
    public Text teamMarblText;

    int roundNumber = 0;
    int currentRound = 1;

    private static GameModeManager _instance;
    public static GameModeManager Instance { get { return _instance; } }

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
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        if (!PhotonNetwork.IsMasterClient) { return; }

        myPV = GetComponent<PhotonView>();

        List<int> _indexList = new List<int>();

        prefabTeam.Add("BallTeam1");
        prefabTeam.Add("BallTeam2");
        prefabTeam.Add("BallTeam3");
        prefabTeam.Add("BallTeam4");

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            int index = (int) p.GetTeam();
            print(p.NickName);
            print(MarblGame.GetColor(index));

            if (!_indexList.Contains(index))
            {
                _indexList.Add(index);
            }
        }

        int numbrBallByTeam = PhotonNetwork.CurrentRoom.GetNbrbBallProp();

        if (PhotonNetwork.CurrentRoom.GetSpawnMode() == 0)
        {
            //------------------------------ MODE TEAM SPAWN ----------------------------

            foreach (int index in _indexList)
            {
                int currentIndex = 0;
                foreach (Transform element in listPos[index].transform)
                {
                    currentIndex++;
                    GameObject _newBall = PhotonNetwork.Instantiate(prefabTeam[index], element.position, Quaternion.identity);

                    if (currentIndex == numbrBallByTeam)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            //------------------------------ MODE RANDOM SPAWN ----------------------------
            Transform[] childs = listPosRandom.GetComponentsInChildren<Transform>();
            List<Transform> spawnPos = new List<Transform>();
            spawnPos.AddRange(childs);
            spawnPos = ShuffleList(spawnPos);

            foreach (int index in _indexList)
            {
                for(int i=0; i < numbrBallByTeam; i++)
                {
                    GameObject _newBall = PhotonNetwork.Instantiate(prefabTeam[index], spawnPos[0].position, Quaternion.identity);
                    spawnPos.Remove(spawnPos[0]);
                }
            }
        }


        CreateTeamList();

        //set turn
        teamPlayed = presentTeam[indexTeamPlaying];
        RoomSetTurn();
        List<Player> _listPlayer = GetPlayerOfOneTeam(teamPlayed);
        playerplayed = CheckWhoHavePlayed(_listPlayer);

        SetPlayerTurn(playerplayed);

        playerAlreadyPlay.Add(playerplayed);

        roundNumber = PhotonNetwork.CurrentRoom.GetRoundProp();

        myPV.RPC("RpcHideScoreBoard", RpcTarget.AllViaServer);

        if(roundNumber == 21) {
            myPV.RPC("RpcDisableRoundText", RpcTarget.AllViaServer);
        }

        myPV.RPC("RpcSetRoundText", RpcTarget.AllViaServer, "Round " + currentRound + " / " + roundNumber);
    }

    List<Transform> ShuffleList(List<Transform> _myList)
    {
        for (int i = 0; i < _myList.Count; i++)
        {
            Transform temp = _myList[i];
            int randomIndex = Random.Range(i, _myList.Count);
            _myList[i] = _myList[randomIndex];
            _myList[randomIndex] = temp;
        }

        return _myList;
    }


    void GivePoint()
    {
        if (PhotonNetwork.CurrentRoom.GetDeathmatch())
        {
            List<PunTeams.Team> listTeamsDeath = DeadZoneManager.Instance.listTeamsDeath;
            int winPoint = PhotonNetwork.CurrentRoom.GetWinPointDM();
            int point = 0;

            foreach (PunTeams.Team team in listTeamsDeath)
            {
                PhotonNetwork.CurrentRoom.AddTeamScore(team, point);
                point += Mathf.FloorToInt(winPoint / PhotonNetwork.PlayerList.Length);
            }

            foreach(PunTeams.Team element in presentTeam)
            {
                if (!listTeamsDeath.Contains(element))
                {
                    PhotonNetwork.CurrentRoom.AddTeamScore(element, winPoint);
                }
            }
        }
    }

    void CreateTeamList()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            PunTeams.Team _pTeam = p.GetTeam();
            teamPlayer.Add(p, _pTeam);

            if (!presentTeam.Contains(_pTeam))
            {
                presentTeam.Add(_pTeam);
            }
        }
    }

    List<Player> GetPlayerOfOneTeam(PunTeams.Team _team)
    {
        List<Player> _sameTeamPlayer = new List<Player>();

        foreach(KeyValuePair<Player, PunTeams.Team> element in teamPlayer)
        {
            if(element.Value == _team)
            {
                _sameTeamPlayer.Add(element.Key);
            }
        }
        return _sameTeamPlayer;
    }


    void SetPlayerTurn(Player _player)
    {
        Hashtable _turnPlayer = new Hashtable();
        _turnPlayer["playerTurn"] = true;
        _player.SetCustomProperties(_turnPlayer);

        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in _Balls)
        {
            ball.GetComponent<PhotonView>().TransferOwnership(_player);
        }

        string message = "<color=" + _player .GetTeam() + ">" + StringExtensions.FirstCharToUpper(_player.GetTeam().ToString()) + "</color>'s turn";
        myPV.RPC("RpcDisplayMessage", RpcTarget.AllViaServer, message);
    }

    void RemovePlayerTurn(Player _player)
    {
        Hashtable _turnPlayer = new Hashtable();
        _turnPlayer["playerTurn"] = false;
        _player.SetCustomProperties(_turnPlayer);
    }

    void RoomSetTurn()
    {
        Hashtable _turnRoom = new Hashtable();
        _turnRoom["turn"] = teamPlayed;
        PhotonNetwork.CurrentRoom.SetCustomProperties(_turnRoom);
    }

    void NextTurn()
    {
        if (gameFinish) { return; }

        indexTeamPlaying++;
        if (indexTeamPlaying == presentTeam.Count)
        {
            indexTeamPlaying = 0;
            currentRound++;

            if (currentRound > roundNumber && roundNumber != 21)
            {
                //fin de partie
                GivePoint();
                EndMode();
                return;
            }

            myPV.RPC("RpcSetRoundText", RpcTarget.AllViaServer, "Round " + currentRound + " / " + roundNumber);
        }

        teamPlayed = presentTeam[indexTeamPlaying];

        //check if team have ball
        if(GetNumberBallOfTeam(teamPlayed) == 0) {
            NextTurn();
            return;
        }

        RoomSetTurn();

        List<Player> _listPlayer = GetPlayerOfOneTeam(teamPlayed);


        playerplayed = CheckWhoHavePlayed(_listPlayer);

        SetPlayerTurn(playerplayed);

        playerAlreadyPlay.Add(playerplayed);
    }

    //Return un joueur qui n'a pas jouer ou redémare le tour des joueurs
    Player CheckWhoHavePlayed(List<Player> _listPlayer)
    {
        foreach(Player _p in _listPlayer)
        {
            if (!playerAlreadyPlay.Contains(_p))
            {
                return _p;
            }
        }

        foreach (Player _p in _listPlayer)
        {
            playerAlreadyPlay.Remove(_p);
        }

        playerAlreadyPlay.Remove(_listPlayer[0]);
        return _listPlayer[0];

    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            try
            {
                if ((bool)p.CustomProperties["playerTurn"])
                {
                    playerTurnText.text = p.NickName + "'s turn !";
                    PunTeams.Team currentTeam = (PunTeams.Team)PhotonNetwork.CurrentRoom.CustomProperties["turn"];
                    playerTurnText.color = MarblGame.GetColor((int)currentTeam);

                    teamMarblText.text = StringExtensions.FirstCharToUpper(GetNumberBallOfTeam(currentTeam).ToString()) + " Marbls left";
                    teamMarblText.color = MarblGame.GetColor((int)currentTeam);
                    return;
                }
            }
            catch { }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player _target, Hashtable changedProps)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            try
            {
                if ((bool)p.CustomProperties["playerTurn"])
                {
                    playerTurnText.text = p.NickName + "'s turn !";
                    PunTeams.Team currentTeam = (PunTeams.Team)PhotonNetwork.CurrentRoom.CustomProperties["turn"];
                    playerTurnText.color = MarblGame.GetColor((int)currentTeam);

                    teamMarblText.text = StringExtensions.FirstCharToUpper(GetNumberBallOfTeam(currentTeam).ToString())  + " Marbls left";
                    teamMarblText.color = MarblGame.GetColor((int)currentTeam);
                    break;
                }
            }
            catch { }
        }

        if (!PhotonNetwork.IsMasterClient) { return; }

        if ((bool)_target.CustomProperties["playerTurn"] == true && _target == playerplayed && !turnStart)
        {
            turnStart = true;
        }

        if ((bool)_target.CustomProperties["playerTurn"] == false && _target == playerplayed && turnStart)
        {
            NextTurn();
        }

    }

    int GetNumberBallOfTeam(PunTeams.Team _team)
    {
        int number = 0;

        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject element in _Balls)
        {
            if (element.GetComponent<BallSettings>().myteam == _team)
            {
                number++;
            }
        }

        return number;
    }

    public void SendKillFeedMessage(string _text)
    {
        myPV.RPC("RpcDisplayMessage", RpcTarget.AllViaServer, _text);
    }

    [PunRPC]
    void RpcDisplayMessage(string _text)
    {
        KillfeedManager.Instance.AddMessage(_text);
    }

    [PunRPC]
    void RpcDisplayScoreBoard()
    {
        ScoreboardManager.Instance.isScoreboardDisplayed = true;
    }

    [PunRPC]
    void RpcHideScoreBoard()
    {
        ScoreboardManager.Instance.isScoreboardDisplayed = false;
    }
    
    [PunRPC]
    void RpcSetRoundText(string roundString)
    {
        UIManager.Instance.round.text = roundString;
    }

    [PunRPC]
    void RpcDisableRoundText()
    {
        UIManager.Instance.round.gameObject.SetActive(false);
    }

    public void EndMode()
    {
        myPV.RPC("RpcDisplayScoreBoard", RpcTarget.AllViaServer);
        gameFinish = true;
        StartCoroutine(WaitToRestartGame());
    }

    IEnumerator WaitToRestartGame()
    {
        yield return new WaitForSeconds(6);

        PhotonNetwork.LoadLevel(lobbyScene);

        //PhotonNetwork.DestroyAll();
    }
}
