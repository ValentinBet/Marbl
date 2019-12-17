using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomScripts : MonoBehaviour
{
    [Header("Top")]
    public RectTransform currentChoice;
    public Text currentText;
    List<Text> oldTexts = new List<Text>();
    List<Text> removeTexts = new List<Text>();

    public RectTransform line;
    public RoomSettings myRoomSetting;


    [Header("panel")]
    public GameObject PlayersList;
    public GameObject GameMode;
    public GameObject Map;

    [Header("Mode")]
    public List<Image> OutlineMode = new List<Image>();

    [Header("Map")]
    public List<Image> OutlineMap = new List<Image>();


    public int mode;
    public int map;

    public GameObject settingsCustom;


    // Start is called before the first frame update
    void Start()
    {
        line.position = currentChoice.position;
        PhotonNetwork.CurrentRoom.SetMap(Mathf.RoundToInt(0));
    }

    // Update is called once per frame
    void Update()
    {
        line.position = Vector2.MoveTowards(line.position, currentChoice.position, 3000 * Time.deltaTime);

        removeTexts.Clear();
        foreach (Text oldText in oldTexts)
        {
            oldText.color = Color.Lerp(oldText.color, Color.white, Mathf.PingPong(Time.time, 3));

            if(oldText.color == Color.white)
            {
                removeTexts.Add(oldText);
            }
        }

        foreach (Text text in removeTexts)
        {
            oldTexts.Remove(text);
        }

        currentText.color = Color.Lerp(currentText.color, new Color(1, 0.1986281f, 0), Mathf.PingPong(Time.time, 3));
    }


    public void SetChoice(RectTransform element)
    {
        settingsCustom.SetActive(false);
        Text newtext = element.GetComponent<Text>();

        if (currentChoice != newtext)
        {
            oldTexts.Add(currentText);
            currentChoice = element;
            currentText = newtext;

            switch (newtext.text)
            {
                case "Players":
                    PlayersList.SetActive(true);
                    GameMode.SetActive(false);
                    Map.SetActive(false);
                    break;

                case "Gamemode":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(true);
                    Map.SetActive(false);
                    break;

                case "Map":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(false);
                    Map.SetActive(true);
                    break;
            }
        }
    }

    public void SetMode(int value)
    {
        OutlineMode[mode].color = Color.white;
        mode = value;
        OutlineMode[mode].color = new Color(1, 0.1986281f, 0);

        if(value == 5)
        {
            settingsCustom.SetActive(true);
            settingsCustom.GetComponent<Image>().color = new Color(1,1,1,1);
            return;
        }
        else
        {
            settingsCustom.SetActive(false);
        }

        myRoomSetting.SetMode(mode);
    }

    public void SetMap(int value)
    {
        OutlineMap[map].color = Color.white;
        map = value;
        OutlineMap[map].color = new Color(1, 0.1986281f, 0);
        PhotonNetwork.CurrentRoom.SetMap(Mathf.RoundToInt(value));
        settingsCustom.SetActive(false);
    }
}
