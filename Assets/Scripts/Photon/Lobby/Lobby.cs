using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.IO;

public class Lobby : MonoBehaviourPunCallbacks
{
    public static Lobby lobby;

    public GameObject buttonHost;
    public GameObject buttonJoin;

    public GameObject panelCreation;
    public GameObject buttonCreate;
    public Text roomName;
    public Text numbreOfPlayer;
    public Text logCreate;

    public Text statut;

    public GameObject panelRooms;
    public Transform listOfRooms;
    public GameObject prefabRoom;
    List<GameObject> roomsList = new List<GameObject>();

    public InputField pseudo;

    private void Awake()
    {
        lobby = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Name"))
        {
            PlayerPrefs.SetString("Name", "Player" + Random.Range(100, 999));
        }

        pseudo.text = PlayerPrefs.GetString("Name");
        PhotonNetwork.NickName = pseudo.text;


        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        buttonHost.GetComponent<Button>().interactable = true;
        buttonJoin.GetComponent<Button>().interactable = true;
        statut.text = "Online";
        statut.color = Color.green;
    }

    public void HostRoom()
    {
        buttonHost.GetComponent<Button>().interactable = false;
        buttonJoin.GetComponent<Button>().interactable = false;

        panelCreation.SetActive(true);
    }

    public void JoinRoom()
    {
        panelRooms.SetActive(true);
    }

    public void CreateRoom()
    {
        int numberPlayersInt;
        if (!int.TryParse(numbreOfPlayer.text, out numberPlayersInt))
        {
            logError("Number of players is not a number");
            return;
        }

        RoomOptions myRoomOption = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)numberPlayersInt };
        PhotonNetwork.CreateRoom(roomName.text, myRoomOption);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        logError(message);
    }

    void logError(string message)
    {
        logCreate.text = message;
        logCreate.gameObject.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print(roomList.Count);
        foreach(RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
            {
                foreach(GameObject element in roomsList)
                {
                    if (element.transform.GetChild(0).GetComponent<Text>().text == room.Name)
                    {
                        roomsList.Remove(element);
                        Destroy(element);
                        return;
                    }
                }
            }
            else
            {
                foreach (GameObject element in roomsList)
                {
                    if (element.transform.GetChild(0).GetComponent<Text>().text == room.Name)
                    {
                        element.transform.GetChild(1).GetComponent<Text>().text = room.PlayerCount + "/" + room.MaxPlayers + " Players";
                        return;
                    }
                }

                GameObject newRoom = Instantiate(prefabRoom, listOfRooms);
                newRoom.transform.GetChild(0).GetComponent<Text>().text = room.Name;
                newRoom.transform.GetChild(1).GetComponent<Text>().text = room.PlayerCount + "/" + room.MaxPlayers + " Players";

                roomsList.Add(newRoom);
            }
        }
    }

    public void ConnectToThisRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        panelRooms.SetActive(false);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        panelRooms.SetActive(true);
    }


    public void SetName()
    {
        PlayerPrefs.SetString("Name", pseudo.text);
        PhotonNetwork.NickName = pseudo.text;
    }
}
