using Photon.Pun;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteamApiManager : MonoBehaviour
{
    public Image avatarPlayer;
    public TextMeshProUGUI userName;

    public Transform parentFriendList;
    public GameObject prefabFriend;

    public GameObject panelSteam;

    private string[] names = new string[] { "Peter", "Ron", "Satchmo", "Sam", "Val", "Axel" , "Pierre", "Tom", "Yoda", "Gotaga", "Diablox9", "Michou" };

    void Start()
    {
        TryConnect();
    }

    public void TryConnect() {
        bool m_bInitialized = SteamAPI.Init();

        if (!m_bInitialized)
        {

            PopupManager.Instance.DisplayPopup(PopupManager.popUpType.Exclamation, "Steam can't be found");

            PhotonNetwork.NickName = names[Random.Range(0, names.Length)] + Random.Range(100, 1000);
            return;
        }

        panelSteam.SetActive(false);

        userName.text = SteamFriends.GetPersonaName();
        PhotonNetwork.NickName = SteamFriends.GetPersonaName();
        avatarPlayer.sprite = GetSteamImageAsTexture2D(SteamFriends.GetLargeFriendAvatar(SteamUser.GetSteamID()));

        DisplayFriend();
    }

    void DisplayFriend()
    {
        int friendCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        //Debug.Log("[STEAM-FRIENDS] Listing " + friendCount + " Friends.");

        for (int i = 0; i < friendCount; ++i)
        {
            CSteamID friendSteamId = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            string friendName = SteamFriends.GetFriendPersonaName(friendSteamId);
            EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendSteamId);

            if(friendState == EPersonaState.k_EPersonaStateOnline)
            {
                GameObject newFriend = Instantiate(prefabFriend, parentFriendList);

                newFriend.GetComponent<FriendElement>().InitValue(GetSteamImageAsTexture2D(SteamFriends.GetLargeFriendAvatar(friendSteamId)) ,friendName, friendSteamId);
            }
        }
    }

    public static Sprite GetSteamImageAsTexture2D(int iImage)
    {
        Texture2D ret = null;
        uint ImageWidth;
        uint ImageHeight;
        bool bIsValid = SteamUtils.GetImageSize(iImage, out ImageWidth, out ImageHeight);
        Sprite mySprite;

        if (bIsValid)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];

            bIsValid = SteamUtils.GetImageRGBA(iImage, Image, (int)(ImageWidth * ImageHeight * 4));
            if (bIsValid)
            {
                ret = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
                ret.LoadRawTextureData(Image);
                ret.Apply();
            }
        }

        try
        {
            mySprite = Sprite.Create(ret, new Rect(0.0f, 0.0f, ret.width, ret.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        catch {
            return null;
        }

        return mySprite;
    }
}
