using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public GameObject Custom;

    [Header("Menu")]
    public GameObject CustomLabel;

    [Header("Mode")]
    public List<Image> OutlineMode = new List<Image>();


    public int mode;
    public int map;

    public bool customMode = false;

    List<GameObject> allCustomMode = new List<GameObject>();
    public GameObject customGamemodePrefab;
    public Transform gamemodeParent;

    private static RoomScripts _instance;
    public static RoomScripts Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


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
                    Custom.SetActive(false);
                    break;

                case "Gamemode":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(true);
                    Map.SetActive(false);
                    Custom.SetActive(false);
                    break;

                case "Map":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(false);
                    Map.SetActive(true);
                    Custom.SetActive(false);
                    break;

                case "Custom":
                    PlayersList.SetActive(false);
                    GameMode.SetActive(false);
                    Map.SetActive(false);
                    Custom.SetActive(true);
                    break;
            }
        }
    }

    public void SetMode(int value)
    {

        OutlineMode[mode].color = Color.white;
        mode = value;
        OutlineMode[mode].color = new Color(1, 0.1986281f, 0);

        CustomLabel.SetActive(false);

        if (customMode)
        {
            customMode = false;
            OutlineMode[5].color = Color.white;
        }
    }

    public void SetCustom()
    {
        customMode = true;
        CustomLabel.SetActive(true);

        OutlineMode[mode].color = Color.white;
        OutlineMode[5].color = new Color(1, 0.1986281f, 0);
    }

    public void SetMap(int value)
    {
        map = value;
        PhotonNetwork.CurrentRoom.SetMap(Mathf.RoundToInt(value));
    }

    public void SetMapCustom(bool value)
    {
        PhotonNetwork.CurrentRoom.SetCustomMap(value);
    }

    public void Refresh()
    {
        foreach (GameObject element in allCustomMode)
        {
            Destroy(element);
        }

        var info = new DirectoryInfo(Application.streamingAssetsPath + "/GameModesCustom/");
        var fileInfo = info.GetFiles();

        foreach (FileInfo file in fileInfo)
        {
            if (file.Name.Contains(".meta")) { continue; }

            GameObject newCustomGameMode = Instantiate(customGamemodePrefab, gamemodeParent);

            allCustomMode.Add(newCustomGameMode);

            print(file.Name);

            newCustomGameMode.GetComponent<GamemodeElement>().labelMode =

            WWW data = new WWW(Application.streamingAssetsPath + "/GameModesDefault/" + file.Name + ".json");
            GameModeSettings modeSettings = new GameModeSettings();
            modeSettings = JsonUtility.FromJson<GameModeSettings>(data.text);


        }
    }
}
