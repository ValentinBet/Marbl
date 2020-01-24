using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;
using static PopupManager;

public class LobbyMainPanel : MonoBehaviourPunCallbacks
{
    [Header("Multiplayer Scene")]
    public string multiplayerScene;

    public InputField PlayerNameInput;

    [Header("Selection Panel")]
    public GameObject SelectionPanel;

    [Header("Reconnect")]
    public GameObject ReconnectButton;
    public Button QuickGameButton;
    public Button CreateLobbyButton;
    public Button JoinLobbyButton;

    [Header("Create Room Panel")]
    public GameObject CreateRoomPanel;

    public InputField RoomNameInputField;
    public InputField MaxPlayersInputField;

    [Header("Join Random Room Panel")]
    public GameObject JoinRandomRoomPanel;

    [Header("Room List Panel")]
    public GameObject RoomListPanel;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomPanel;
    public Transform parentPlayerList;

    [Header("Chat")]
    public GameObject ChatPlayerObject;

    [Header("Other")]
    public Button StartGameButton;
    public GameObject hostPanel;
    public GameObject clientPanel;
    public GameObject settingsPanel;
    public GameObject PlayerListEntryPrefab;
    public Animator MainMenuAnim;

    private bool locked = false;
    private bool restartedGame = false;
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<Player, GameObject> playerListEntries;


    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

        PhotonNetwork.ConnectUsingSettings();

        if (!PlayerPrefs.HasKey("Name"))
        {
            PlayerPrefs.SetString("Name", "Player" + Random.Range(100, 999));
        }

        PlayerNameInput.text = PlayerPrefs.GetString("Name");

        ReconnectButton.SetActive(false);

        PhotonNetwork.SendRate = 100;
        PhotonNetwork.SerializationRate = 100;
    }

    private void Start()
    {
        if(PhotonNetwork.PlayerList.Length > 0) {
            restartedGame = true;
            OnJoinedRoom();
            MainMenuAnim.Play("RestartGame");
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;

            foreach(Player p in PhotonNetwork.PlayerList)
            {
                PhotonNetwork.CurrentRoom.SetTeamScore(p.GetTeam(), 0);
            }
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        QuickGameButton.interactable = false;
        CreateLobbyButton.interactable = false;
        JoinLobbyButton.interactable = false;
        ReconnectButton.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        this.SetActivePanel(SelectionPanel.name);

        PhotonNetwork.LocalPlayer.NickName = PlayerNameInput.text;
        //On Connection
        QuickGameButton.interactable = true;
        CreateLobbyButton.interactable = true;
        JoinLobbyButton.interactable = true;
        PhotonNetwork.LocalPlayer.SetTeam(MarblFactory.GetRandomTeam());

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();

        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(SelectionPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PopupManager.Instance.DisplayPopup(popUpType.Exclamation, "No game available \n Creating ...");

        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 8};

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        SetActivePanel(InsideRoomPanel.name);

        PhotonNetwork.CurrentRoom.SetMap(RoomScripts.Instance.map);
        RoomSettings.Instance.SaveSettings();


        GameObject playerChat = PhotonNetwork.Instantiate("PlayerChat", Vector3.zero, Quaternion.identity);

        ChatManager.Instance.lobbyPlayerChat = playerChat.GetComponent<LobbyChat>();

        PhotonNetwork.LocalPlayer.SetPlayerReadyState(false);
        PhotonNetwork.LocalPlayer.SetScore(0);

        playerListEntries = new Dictionary<Player, GameObject>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(parentPlayerList);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListEntry>().Initialize(p, p.GetTeam(), p.NickName);

            entry.GetComponent<PlayerListEntry>().SetPlayerReady(p.GetPlayerReadyState());

            playerListEntries.Add(p, entry);

        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());

        if (PhotonNetwork.IsMasterClient)
        {
            hostPanel.SetActive(true);
            clientPanel.SetActive(false);
        }
        else
        {
            hostPanel.SetActive(false);
            clientPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(SelectionPanel.name);

        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        ChatManager.Instance.lobbyPlayerChat.DestroyThis();

        playerListEntries.Clear();
        playerListEntries = null;
        hostPanel.SetActive(false);
        settingsPanel.SetActive(false);
        restartedGame = false;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(parentPlayerList.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListEntry>().Initialize(newPlayer, newPlayer.GetTeam(), newPlayer.NickName);

        playerListEntries.Add(newPlayer, entry);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer].gameObject);
        playerListEntries.Remove(otherPlayer);

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            PlayerListEntry pList = GetPlayerListEntry(p);
            pList.Refresh(MarblGame.GetColor((int) p.GetTeam()));
            pList.SetPlayerReady(p.GetPlayerReadyState());
        }

        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    PlayerListEntry GetPlayerListEntry(Player p)
    {
        foreach(KeyValuePair<Player, GameObject> element in playerListEntries)
        {
            if(element.Key == p)
            {
                return element.Value.GetComponent<PlayerListEntry>();
            }
        }
        return null;
    }

    public void Reconnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        ReconnectButton.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        PlayMenuSound();
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        SetActivePanel(SelectionPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        PlayMenuSound();
        string roomName = RoomNameInputField.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1, 1000) : roomName;

        byte maxPlayers;
        byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
        maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void OnCreateLobbyButtonClicked()
    {
        PlayMenuSound();
        SetActivePanel(CreateRoomPanel.name);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        PlayMenuSound();
        SetActivePanel(JoinRandomRoomPanel.name);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLeaveGameButtonClicked()
    {
        PlayMenuSound();
        PhotonNetwork.LeaveRoom();
    }

    public void ChangeName()
    {
        string playerName = PlayerNameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PlayerPrefs.SetString("Name", playerName);
            PopupManager.Instance.DisplayPopup(popUpType.Confirmation, "Name changed");
        }
        else
        {
            PopupManager.Instance.DisplayPopup(popUpType.Forbident, "Failed to change name");
        }
    }

    public void OnRoomListButtonClicked()
    {
        PlayMenuSound();

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        SetActivePanel(RoomListPanel.name);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.DestroyAll();

        PlayMenuSound();

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        if (!RoomScripts.Instance.customMode)
        {
            RoomSettings.Instance.SetMode(RoomScripts.Instance.fileModeName, RoomScripts.Instance.customModeSave);
        }

        RoomSettings.Instance.SaveSettings();

        multiplayerScene = PhotonNetwork.CurrentRoom.GetMap();

        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if(p == PhotonNetwork.LocalPlayer)
            {
                continue;
            }

            if (p.GetPlayerReadyState())
            {
                GetPlayerListEntry(p).PlayerReadyImage.gameObject.SetActive(true);
                continue;
            }
            else
            {
                GetPlayerListEntry(p).PlayerReadyImage.gameObject.SetActive(false);
                return false;
            }
        }

        return true;
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }

    public void LocalPlayerPropertiesUpdated()
    {
        StartGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    private void SetActivePanel(string activePanel)
    {
        if ((activePanel != SelectionPanel.name && MainMenuAnim != null && locked == false) || (activePanel == SelectionPanel.name && locked == true))
        {
            if (!locked)
            {
                locked = true;
            }
            else
            {
                locked = false;
            }
            //Return to menu from lobby after first game
            if (activePanel == SelectionPanel.name && locked == false && restartedGame)
            {
                MainMenuAnim.SetTrigger("ReturnToMenu");
            }
            else if (!restartedGame)
            {
                MainMenuAnim.SetTrigger("Contextual");
            }
        }
        SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
        CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
        JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
        RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
        InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }

    private void PlayMenuSound()
    {
        AudioManager.Instance.PlayThisSound(AudioManager.Instance.menuSong,1);
    }

}