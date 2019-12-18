using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class bl_ChatUI : MonoBehaviour {

    [SerializeField]private GameObject LinePrefabClient = null;
    [SerializeField]private GameObject LinePrefabServer = null;
    [SerializeField]private GameObject PlayerNameUI;
    [SerializeField]private Transform ChatPanel = null;

    private List<GameObject> cacheMessages = new List<GameObject>();
    [HideInInspector]public int MaxMessages = 10;
    private ChatManager Chat;

    public InputField myInputField;

    void Awake()
    {
        Chat = FindObjectOfType<ChatManager>();
    }


    public void AddNewLine(string text, bool fade, float time = 10,float speed = 1, bool isMyTeam = true)
    {
        if (isMyTeam)
        {
            GameObject newline = Instantiate(LinePrefabClient) as GameObject;
            newline.GetComponent<Text>().text = text;
            newline.GetComponent<LayoutElement>().CalculateLayoutInputVertical();
            newline.GetComponent<LayoutElement>().CalculateLayoutInputHorizontal();
            if (fade)
            {
                newline.GetComponent<bl_ChatLine>().FadeInTime(time, speed);
            }
            newline.transform.SetParent(ChatPanel, false);
            cacheMessages.Add(newline);
        }
        else
        {
            GameObject newlineremote = Instantiate(LinePrefabServer) as GameObject;
            newlineremote.GetComponent<Text>().text = text;
            newlineremote.GetComponent<LayoutElement>().CalculateLayoutInputVertical();
            newlineremote.GetComponent<LayoutElement>().CalculateLayoutInputHorizontal();
            if (fade)
            {
                newlineremote.GetComponent<bl_ChatLine>().FadeInTime(time, speed);
            }
            newlineremote.transform.SetParent(ChatPanel, false);
            cacheMessages.Add(newlineremote);
        }
        CheckMessageLenght();
    }

    void CheckMessageLenght()
    {
        if(cacheMessages.Count > MaxMessages)
        {
            if (cacheMessages[0] != null)
            {
                Destroy(cacheMessages[0]);
            }
            cacheMessages.RemoveAt(0);
        }
    }

    public void Clean()
    {
        foreach(GameObject g in cacheMessages)
        {
            Destroy(g);
        }
        cacheMessages.Clear();
    }

}