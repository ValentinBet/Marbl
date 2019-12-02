using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;

public class ScoreboardManager : MonoBehaviourPunCallbacks
{
    private static ScoreboardManager _instance;
    public static ScoreboardManager Instance { get { return _instance; } }

    public Dictionary<int, PlayerScoreUI> playerScoreDict = new Dictionary<int, PlayerScoreUI>();
    public GameObject scoreboardPanel;
    public GameObject teamScore;
    public GameObject playerScore;
    public GameObject teamScores;
    public bool isScoreboardDisplayed = false;

    public Dictionary<PunTeams.Team, TeamUIManager> listTeams = new Dictionary<PunTeams.Team, TeamUIManager>();

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

        InitTeams();
        InitScoreBoard();
    }

    private void Update()
    {
        DisplayScoreboard();
    }



    private void InitScoreBoard()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (listTeams.TryGetValue(p.GetTeam(), out TeamUIManager entry))
            {
                GameObject playerEntry = Instantiate(playerScore);
                PlayerScoreUI playerScoreUI = playerEntry.GetComponent<PlayerScoreUI>();
                playerEntry.transform.SetParent(entry.playersScore.transform);
                playerEntry.transform.localScale = Vector3.one;
                playerScoreUI.NameText.text = p.NickName;
                Color _color = MarblGame.GetColor((int)p.GetTeam());
                playerScoreUI.ColorSeparator.color = _color;
                playerScoreUI.PointText.text = p.GetScore().ToString();
                playerScoreDict.Add(p.ActorNumber, playerScoreUI);
            }
        }
    }

    private void DisplayScoreboard()
    {
        if (scoreboardPanel != null)
        {
            if (!isScoreboardDisplayed)
            {
                if (Input.GetKey(InputManager.Instance.Inputs.inputs.Learderboard))
                {
                    scoreboardPanel.SetActive(true);
                }
                else
                {
                    scoreboardPanel.SetActive(false);
                }
            }
            else
            {
                scoreboardPanel.SetActive(true);
            }
        }
    }

    private void InitTeams()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!listTeams.ContainsKey(p.GetTeam()))
            {
                GameObject _teamScore = Instantiate(teamScore);
                _teamScore.transform.SetParent(teamScores.transform);
                _teamScore.transform.localScale = Vector3.one;
                TeamUIManager _teamUIManager = _teamScore.GetComponent<TeamUIManager>();
                Color _color = MarblGame.GetColor((int)p.GetTeam());
                _teamUIManager.MetaScore.color = new Color(_color.r, _color.g, _color.b, 0.5f);
                _teamUIManager.ScoreText.color = MarblGame.GetColor((int)p.GetTeam());
                listTeams.Add(p.GetTeam(), _teamUIManager);
            }
        }
    }

    public Dictionary<Player, int> GetPlayerListSortedByPoints(PunTeams.Team? team = null)
    {
        Dictionary<Player, int> _temp = new Dictionary<Player, int>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (team == null)
            {
                _temp.Add(p, p.GetScore());
            }
            else
            {
                if (team == p.GetTeam())
                {
                    _temp.Add(p, p.GetScore());
                }
            }
        }

        var list = _temp.OrderByDescending(key => key.Value).ToList();

        return list.ToDictionary(x => x.Key, x => x.Value);
    }

    public Dictionary<PunTeams.Team, int> GetTeamListSortedByPoints()
    {
        Dictionary<PunTeams.Team, int> _temp = new Dictionary<PunTeams.Team, int>();

        foreach (PunTeams.Team team in listTeams.Keys)
        {
            _temp.Add(team, PhotonNetwork.CurrentRoom.GetTeamScore(team));
        }
        var list = _temp.OrderByDescending(key => key.Value).ToList();

        return list.ToDictionary(x => x.Key, x => x.Value);
    }

    public void Adazda()
    {
        PhotonNetwork.LocalPlayer.AddPlayerScore(1);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        Destroy(playerScoreDict[otherPlayer.ActorNumber].gameObject);
        playerScoreDict.Remove(otherPlayer.ActorNumber);

        bool isTeamEmpty = true;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if ((p.GetTeam() == otherPlayer.GetTeam()) && (p != otherPlayer))
            {
                isTeamEmpty = false;
            }
        }

        if (isTeamEmpty == true)
        {
            Destroy(listTeams[otherPlayer.GetTeam()].gameObject);
            listTeams.Remove(otherPlayer.GetTeam());
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        foreach (PunTeams.Team team in listTeams.Keys)
        {
            if (listTeams.TryGetValue(team, out TeamUIManager teamUI))
            {
                teamUI.ScoreText.text = PhotonNetwork.CurrentRoom.GetTeamScore(team).ToString();
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (playerScoreDict.TryGetValue(target.ActorNumber, out PlayerScoreUI entry))
        {
            entry.NameText.GetComponent<Text>().text = target.NickName;
            entry.PointText.GetComponent<Text>().text = target.GetPlayerScore().ToString();          
        }

        // SET POSITION IN SCOREBOARD >>
        foreach (PunTeams.Team team in listTeams.Keys)
        {
            Dictionary<Player, int> _temp = GetPlayerListSortedByPoints();

            for (int i = 0; i < _temp.Count; i++)
            {
                if (playerScoreDict.TryGetValue(_temp.ElementAt(i).Key.ActorNumber, out PlayerScoreUI playerScore))
                {
                    playerScore.gameObject.transform.SetSiblingIndex(i);
                }
            }
        }
        // <<
    }
}
