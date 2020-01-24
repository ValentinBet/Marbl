
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class ChatManager : MonoBehaviour
{

    [Header("Settings")]
    [HideInInspector]
    public int GroupID = 0;
    public List<bl_GroupInfo> Groups = new List<bl_GroupInfo>();
    public string ClientName = string.Empty;

    public bool useBothSides = true;
    public bool useGroupPrefix = true;
    public bool ShowPlayerNameInput = true; //If you setup your own method for sen the name, desactive this.
    public int MaxMessages = 15;

    [TextArea(3, 7)] public string BlackList;
    public string ReplaceString = "*";
    [HideInInspector] public bool ShowBlackListEditor;

    public bool FadeMessage = true;
    public float FadeMessageIn = 7;
    public float FadeMessageSpeed = 2;
    public bool isLobbyChat;
    public bl_ChatUI ChatUI = null;

    public LobbyChat lobbyPlayerChat;
    private static ChatManager _instance;
    public static ChatManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        ClientName = string.Empty;
        ChatUI = FindObjectOfType<bl_ChatUI>();
        ChatUI.MaxMessages = MaxMessages;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Send Message
        {
            if (ChatUI.myInputField.isActiveAndEnabled)
            {
                SendChatText(ChatUI.myInputField);
            }
        }


        if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.Chat))
        {
            if (!isLobbyChat)
            {
                if (!ChatUI.InputFieldGroup.activeInHierarchy)
                {
                    ChatUI.InputFieldGroup.SetActive(true);
                }
                else
                {
                    ChatUI.InputFieldGroup.SetActive(false);
                }
            }

            ChatUI.OnChatDisplay();
        }

    }


            public void SendChatText(InputField field)
            {
                string text = field.text;
                if (string.IsNullOrEmpty(text))
                    return;

                if (text.Length > 300)
                {
                    text = text.Substring(0, 300);
                }

                if (!isLobbyChat)
                {
                    text = "<color=" + MarblGame.GetColorUI((int)GameModeManager.Instance.localPlayerTeam) + ">" + PhotonNetwork.LocalPlayer.NickName + "</color> : " + text;

                    GameModeManager.Instance.localPlayerObj.GetComponent<LocalPlayerManager>().SendMessageString(text);

                }
                else
                {
                    text = "<color=" + MarblGame.GetColorUI((int)PhotonNetwork.LocalPlayer.GetTeam()) + ">" + PhotonNetwork.LocalPlayer.NickName + "</color> : " + text;
                    lobbyPlayerChat.SendMessageString(text);

                }

                field.text = string.Empty;

            }


            public void OnChatMessage(string message)
            {
                if (message != null)
                {
                    ChatUI.AddNewLine(message, FadeMessage, FadeMessageIn, FadeMessageSpeed, true);
                }

            }


            public string GetMessageFormat(string msn, string sender, bl_GroupInfo group)
            {
                string m = "";
#if UNITY_5_2 || UNITY_5_3_OR_NEWER

                string hex = ColorUtility.ToHtmlStringRGBA(group.GroupColor);
#else
         string hex = group.GroupColor.ToHexStringRGBA();
#endif
                string filterText = msn;
                //Apply word filter
                if (GetBlackListArray.Length > 0)
                {
                    filterText = bl_ChatUtils.FilterWord(GetBlackListArray, msn, ReplaceString);
                }
                if (useGroupPrefix)
                {
                    m = string.Format("<color=#{0}>[{1}]{3}: </color>{2}", hex, group.Name, filterText, sender);
                }
                else
                {
                    m = string.Format("{2}<color=#{0}>[{1}]</color>", hex, filterText, sender);
                }
                return m;
            }

            public bl_GroupInfo GetGroup(int id)
            {
                return Groups[id];
            }

    public string[] GetGroupArray
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < Groups.Count; i++)
            {
                list.Add(Groups[i].Name);
            }
            return list.ToArray();
        }
    }

    private string[] GetBlackListArray
    {
        get
        {
            List<string> list = new List<string>();
            string[] split = BlackList.Split(',');
            foreach (string str in split)
            {
                string t = str.Trim();
                if (!string.IsNullOrEmpty(t))
                {
                    list.Add(t);
                }
            }
            return list.ToArray();
        }
    }
}