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

    public Dropdown dropTeam;

    public void Initialize(Player player,  Color playerColor, string playerName)
    {
        if(player != PhotonNetwork.LocalPlayer)
        {
            PlayerReadyButton.gameObject.SetActive(false);
            dropTeam.gameObject.SetActive(false);
        }

        PlayerColorImage.color = playerColor;
        PlayerNameText.text = playerName;
    }

    public void Refresh(Color playerColor)
    {
        PlayerColorImage.color = playerColor;
    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
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
}
