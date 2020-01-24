using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class bl_ChatUI : MonoBehaviour {

    [SerializeField]private GameObject LinePrefabClient = null;
    [SerializeField]private Transform ChatPanel = null;

    private List<GameObject> cacheMessages = new List<GameObject>();
    [HideInInspector]public int MaxMessages = 10;
    private ChatManager Chat;

    public InputField myInputField;
    public GameObject InputFieldGroup;

    bool isShow = false;

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
    }

    public void ShowHisto(Image backGround)
    {
        backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, 0.1f);
        foreach (GameObject obj in cacheMessages)
        {
            obj.SetActive(true);
            obj.GetComponent<bl_ChatLine>().ShowObj(1);
        }
    }

    public void Hide(Image backGround)
    {
        backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, 0.003921569f);
        foreach (GameObject obj in cacheMessages)
        {
            obj.GetComponent<bl_ChatLine>().FadeInTime(0.1f, 1);
        }
    }

    public void OnChatDisplay()
    {
        myInputField.Select();
        myInputField.ActivateInputField();
    }

}