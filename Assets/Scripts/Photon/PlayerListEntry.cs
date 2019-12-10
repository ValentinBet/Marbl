using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using static Photon.Pun.UtilityScripts.PunTeams;

public class PlayerListEntry : MonoBehaviour
{
    [Header("UI References")]
    public Text PlayerNameText;

    public Image PlayerColorImage;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;

    private int ownerId;
    private bool isPlayerReady;

    public Dropdown dropTeam;

    #region UNITY

    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {
            PlayerReadyButton.gameObject.SetActive(false);
            dropTeam.gameObject.SetActive(false);
        }
        else
        {
            Hashtable initialProps = new Hashtable() { { MarblGame.PLAYER_READY, isPlayerReady }};
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            PhotonNetwork.LocalPlayer.SetScore(0);

            dropTeam.value = (int)PhotonNetwork.LocalPlayer.GetTeam();

            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                Hashtable props = new Hashtable() { { MarblGame.PLAYER_READY, isPlayerReady } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                dropTeam.gameObject.SetActive(false);


                if (PhotonNetwork.IsMasterClient)
                {
                    FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                }
            });
        }
    }

    public void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }

    #endregion

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
    }

    public void OnPlayerNumberingChanged()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.ActorNumber == ownerId)
            {
                PlayerColorImage.color = MarblGame.GetColor((int) p.GetTeam());
            }
        }
    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
        PlayerReadyImage.enabled = playerReady;
    }

    public void ReFreshTeam() {

        int idTeam = (int) PhotonNetwork.LocalPlayer.GetTeam();

        dropTeam.value = idTeam;

        Team myTeam = Team.red;

        switch (idTeam)
        {
            case 0: myTeam = Team.red; break;
            case 1: myTeam = Team.green; break;
            case 2: myTeam = Team.blue; break;
            case 3: myTeam = Team.yellow; break;
        }

        PhotonNetwork.LocalPlayer.SetTeam(myTeam);
        PlayerColorImage.color = MarblGame.GetColor(idTeam);
        PhotonNetwork.LocalPlayer.SetPlayerNumber(idTeam);
    }

    public void ChangeTeam()
    {
        int idTeam = dropTeam.value;
        Team myTeam = Team.red;

        switch (idTeam)
        {
            case 0: myTeam = Team.red; break;
            case 1: myTeam = Team.green; break;
            case 2: myTeam = Team.blue; break;
            case 3: myTeam = Team.yellow; break;
        }

        PhotonNetwork.LocalPlayer.SetTeam(myTeam);
        PlayerColorImage.color = MarblGame.GetColor(idTeam);
        PhotonNetwork.LocalPlayer.SetPlayerNumber(idTeam);
    }
}
