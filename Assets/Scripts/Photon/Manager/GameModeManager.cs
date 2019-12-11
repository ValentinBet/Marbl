using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

public class GameModeManager : MonoBehaviourPunCallbacks
{
    public List<Transform> listPos;
    public Transform listPosRandom;
    public Transform listNeutralPos;
    public string lobbyScene;

    public List<Material> colors = new List<Material>();
    PhotonView myPV;

    Dictionary<Player, Team> teamPlayer = new Dictionary<Player, Team>();
    List<Team> presentTeam = new List<Team>();

    Team teamPlayed;
    Player playerplayed;

    List<Player> playerAlreadyPlay = new List<Player>();
    int indexTeamPlaying = 0;

    bool turnStart = false;
    public bool gameFinish = false;

    public Text playerTurnText;
    public Text teamMarblText;

    int roundNumber = 0;
    int currentRound = 1;

    bool allHaveLoadMap = false;


    //----------------    BOOL DE CHAQUE MODE   -----------------
    bool modeDM = false;
    bool modeHill = false;
    bool modeHue = false;
    bool modeCoins = false;
    bool modePotato = false;
    bool modeBillard = false;

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
    }

    private void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        //GET MODE ACTIF
        modeDM = PhotonNetwork.CurrentRoom.GetDeathmatch();
        modeHill = PhotonNetwork.CurrentRoom.GetHill();
        modeHue = PhotonNetwork.CurrentRoom.GetHue();
        modeCoins = PhotonNetwork.CurrentRoom.GetCoins();
        modePotato = PhotonNetwork.CurrentRoom.GetPotato();
        modeBillard = PhotonNetwork.CurrentRoom.GetBillard();


        myPV = GetComponent<PhotonView>();

        List<int> _indexList = new List<int>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            int index = (int) p.GetTeam();

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
                    GameObject _newBall = PhotonNetwork.Instantiate("Marbl", element.position, Quaternion.identity);
                    _newBall.GetComponent<BallSettings>().myteam = MarblGame.GetTeam(index);

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
            List<Transform> spawnPos = MarblFactory.GetListOfAllChild(listPosRandom);

            spawnPos = MarblFactory.ShuffleList(spawnPos);

            foreach (int index in _indexList)
            {
                for(int i=0; i < numbrBallByTeam; i++)
                {
                    GameObject _newBall = PhotonNetwork.Instantiate("Marbl", spawnPos[0].position, Quaternion.identity);
                    _newBall.GetComponent<BallSettings>().myteam = MarblGame.GetTeam(index);
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

        myPV.RPC("RpcSetRoundText", RpcTarget.AllViaServer, "Round" + "\n" + "<size=180> " + currentRound + " / " + roundNumber + "</size>");

        myPV.RPC("RpcStartGame", RpcTarget.All);



        //-----------------------------ACTIVATION DES MODES ACTIF-------------------------------------

        DeathMatchManager.Instance.ActiveThisMode(modeDM);
        HillManager.Instance.ActiveThisMode(modeHill);
        HueManager.Instance.ActiveThisMode(modeHue);

    }

    void CreateTeamList()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Team _pTeam = p.GetTeam();
            teamPlayer.Add(p, _pTeam);

            if (!presentTeam.Contains(_pTeam))
            {
                presentTeam.Add(_pTeam);
            }
        }
    }

    List<Player> GetPlayerOfOneTeam(Team _team)
    {
        List<Player> _sameTeamPlayer = new List<Player>();

        foreach(KeyValuePair<Player, Team> element in teamPlayer)
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
        _player.SetPlayerTurnState(true);

        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in _Balls)
        {
            ball.GetComponent<PhotonView>().TransferOwnership(_player);
        }

        string message = "<color=" + _player .GetTeam() + ">" + MarblFactory.FirstCharToUpper(_player.GetTeam().ToString()) + "</color>'s turn";
        myPV.RPC("RpcDisplayMessage", RpcTarget.AllViaServer, message);
    }

    void RemovePlayerTurn(Player _player)
    {
        _player.SetPlayerTurnState(false);
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
                if (modeDM)
                {
                    DeathMatchManager.Instance.GivePoint(presentTeam);
                }
                EndMode();
                return;
            }

            myPV.RPC("RpcSetRoundText", RpcTarget.AllViaServer, "Round" + "\n" + "<size=180> " + currentRound + " / " + roundNumber + "</size>");
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
                if (p.GetPlayerTurnState())
                {
                    playerTurnText.text = p.NickName + "'s turn !";
                    Team currentTeam = (Team)PhotonNetwork.CurrentRoom.CustomProperties["turn"];
                    playerTurnText.color = MarblGame.GetColor((int)currentTeam);

                    teamMarblText.text = MarblFactory.FirstCharToUpper(GetNumberBallOfTeam(currentTeam).ToString()) + " Marbls left";
                    teamMarblText.color = MarblGame.GetColor((int)currentTeam);

                    if(p.UserId == PhotonNetwork.LocalPlayer.UserId)
                    {
                        playerTurnText.text = "Your turn !";
                    }

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
                if (p.GetPlayerTurnState())
                {
                    playerTurnText.text = p.NickName + "'s turn !";
                    Team currentTeam = (Team)PhotonNetwork.CurrentRoom.CustomProperties["turn"];
                    playerTurnText.color = MarblGame.GetColor((int)currentTeam);

                    teamMarblText.text = MarblFactory.FirstCharToUpper(GetNumberBallOfTeam(currentTeam).ToString())  + " Marbls left";
                    teamMarblText.color = MarblGame.GetColor((int)currentTeam);

                    if (p.UserId == PhotonNetwork.LocalPlayer.UserId)
                    {
                        playerTurnText.text = "Your turn !";
                    }
                    break;
                }
            }
            catch { }
        }

        if (!PhotonNetwork.IsMasterClient) { return; }

        if (_target.GetPlayerTurnState() == true && _target == playerplayed && !turnStart && allHaveLoadMap)
        {
            turnStart = true;
        }

        if (_target.GetPlayerTurnState() == false && _target == playerplayed && turnStart && allHaveLoadMap)
        {
            CallEndTurnMode();
            NextTurn();
        }

        if (!allHaveLoadMap)
        {
            if (CheckAllHaveLoadMap())
            {
                allHaveLoadMap = true;
                StartCoroutine(WaitToStart());
            }
        }
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2);
        StartGame();
    }

    bool CheckAllHaveLoadMap()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!p.GetPlayerMapState())
            {
                return false;
            }
        }
        return true;
    }

    void CallEndTurnMode()
    {
        if (modeHill)
        {
            HillManager.Instance.EndTurn();
        }
    }

    int GetNumberBallOfTeam(Team _team)
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

    [PunRPC]
    void RpcStartGame()
    {
        UIManager.Instance.EnablePing();
        UIManager.Instance.LoadingPanel.SetActive(false);
    }

    public void EndMode()
    {
        if (gameFinish) { return; }

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

    public void DisplayPoint(Vector3 position, int value)
    {
        PhotonNetwork.Instantiate("Points", position, Quaternion.identity);
    }

    public void DetectEndGame()
    {
        GameObject[] Balls = GameObject.FindGameObjectsWithTag("Ball");
        List<GameObject> allBalls = new List<GameObject>();
        allBalls.AddRange(Balls);

        List<Team> allTeamAlive = new List<Team>();

        foreach (GameObject element in allBalls)
        {
            Team ballTeam = element.GetComponent<BallSettings>().myteam;
            if(ballTeam == Team.neutral) { continue; }

            if (!allTeamAlive.Contains(ballTeam))
            {
                allTeamAlive.Add(ballTeam);
            }
        }

        if (allTeamAlive.Count == 1)
        {
            EndMode();
        }
        else
        {
            return;
        }
    }
}
