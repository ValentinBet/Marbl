using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendElement : MonoBehaviour
{
    CSteamID myId;

    public Image icon;
    public TextMeshProUGUI myName;
    public GameObject button;

    public void InitValue(Sprite myIcon, string name, CSteamID idFriend )
    {
        myId = idFriend;

        icon.sprite = myIcon;
        myName.text = name;
        button.SetActive(false);
    }

    public void OnHost()
    {
        button.SetActive(true);
    }


    public void Invite()
    {
        PopupManager.Instance.DisplayPopup(PopupManager.popUpType.Confirmation, SteamFriends.GetFriendPersonaName(myId) + " invited !");
    }
}
