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
    public Button PlayerKickButton;
    public Image PlayerReadyImage;

    public Dropdown dropTeam;

    public Text winNumber;

    Player myPlayer;

    public ColorBlock normalColor;
    public ColorBlock lockedColor;

    public void Initialize(Player player, Team team, string playerName)
    {
        myPlayer = player;

        if (player != PhotonNetwork.LocalPlayer)
        {
            PlayerReadyButton.gameObject.SetActive(false);
            dropTeam.gameObject.SetActive(false);

            if (PhotonNetwork.IsMasterClient)
            {
                PlayerKickButton.gameObject.SetActive(true);
            }
        }
        else
        {
            dropTeam.value = (int) team;

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LocalPlayer.SetPlayerReadyState(true);
                PlayerReadyButton.gameObject.SetActive(false);
            }
        }

        winNumber.text = player.GetPlayerPersistantScore().ToString();

        PlayerColorImage.color = MarblGame.GetColor((int) team);
        PlayerNameText.text = playerName;
    }

    public void Refresh(Color playerColor)
    {
        PlayerColorImage.color = playerColor;
    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
        PlayerReadyButton.colors = playerReady ? lockedColor : normalColor;
        PlayerReadyImage.enabled = playerReady;
    }

    public void SwitchReady()
    {
        if (PhotonNetwork.LocalPlayer.GetPlayerReadyState())
        {
            PhotonNetwork.LocalPlayer.SetPlayerReadyState(false);
            SetPlayerReady(false);
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetPlayerReadyState(true);
            SetPlayerReady(true);
        }
    }

    public void ChangeTeam()
    {
        int idTeam = dropTeam.value;
        Team myTeam = MarblGame.GetTeam(idTeam);

        PhotonNetwork.LocalPlayer.SetTeam(myTeam);
        PlayerColorImage.color = MarblGame.GetColor(idTeam);
    }

    public void KickPlayer()
    {
        PhotonNetwork.CloseConnection(myPlayer);
    }
}

