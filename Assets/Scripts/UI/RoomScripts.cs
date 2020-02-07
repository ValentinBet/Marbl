using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RoomScripts : MonoBehaviour
{
    public RoomSettings myRoomSetting;

    [Header("Menu")]
    public GameObject CustomLabel;

    [Header("Mode")]
    public Image OutlinedMode;

    public string map = "VKing";

    public bool customMode = false;
    public bool customModeSave = false;
    public string fileModeName;

    List<GameObject> allCustomMode = new List<GameObject>();
    public GameObject customGamemodePrefab;
    public Transform gamemodeParent;

    public List<MapElement> allMaps = new List<MapElement>();

    [Header("QuickMode")]
    public HostPanel hostPanel;
    public MapPool mapPool;
    public Text gamemodeReminderText;

    private List<string> mapPoolChoose = new List<string>();

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
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.SetMap(map);
            Refresh();
        }
    }


    public void SetMode(Image img)
    {
        OutlinedMode.color = Color.white;
        OutlinedMode = img;
        OutlinedMode.color = new Color(1, 0.1986281f, 0);

        CustomLabel.SetActive(false);

        gamemodeReminderText.text = GetActualMode();
    }

    public void SetCustom(Image img)
    {
        OutlinedMode.color = Color.white;
        OutlinedMode = img;
        OutlinedMode.color = new Color(1, 0.1986281f, 0);

        customMode = true;
        customModeSave = false;
        CustomLabel.SetActive(true);

        gamemodeReminderText.text = GetActualMode();
    }

    public void SetMap(string value)
    {
        map = value;
        PhotonNetwork.CurrentRoom.SetMap(value);

        gamemodeReminderText.text = GetActualMode();
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

            GamemodeElement gModeElement = newCustomGameMode.GetComponent<GamemodeElement>();

            gModeElement.labelMode.text = file.Name.Replace(".json", "");

            WWW data = new WWW(Application.streamingAssetsPath + "/GameModesCustom/" + file.Name);
            GameModeSettings modeSettings = new GameModeSettings();

            modeSettings = JsonUtility.FromJson<GameModeSettings>(data.text);

            string infoMode = CheckInfoMode(modeSettings, gModeElement);

            gModeElement.Description.text = infoMode;
            gModeElement.fileName = file.Name;
        }

        gamemodeReminderText.text = GetActualMode();

    }


    string CheckInfoMode(GameModeSettings modeSettings, GamemodeElement gModeElement)
    {
        string infoMode = "";

        if (modeSettings.deathmatch)
        {
            infoMode += "DeathMatch";
            gModeElement.Deathmatch = true;
        }

        if (modeSettings.hill)
        {
            if (infoMode != "")
            {
                infoMode += "<color=red> X </color>";
            }

            infoMode += "King of the hill";
            gModeElement.KingOfTheHill = true;

            switch (modeSettings.hillMode)
            {
                case 0:
                    infoMode += " (One for One)";
                    break;

                case 1:
                    infoMode += " (Contest)";
                    break;

                case 2:
                    infoMode += " (Domination)";
                    break;
            }
        }

        if (modeSettings.hue)
        {
            if (infoMode != "")
            {
                infoMode += "<color=red> X </color>";
            }

            infoMode += "Hue";
            gModeElement.Hue = true;
        }

        return infoMode;
    }

    private string GetActualMode()
    {
        return "Mode : " + fileModeName.Replace(".json", "");
    }

    public void OnGameModeChoose(string gamemode)
    {
        switch (gamemode)
        {
            case "Deathmatch":
                ModeChoose("DeathMatch.json", mapPool.DeathmatchPool);
                break;
            case "Hue":
                ModeChoose("HUE.json", mapPool.HuePool);
                break;
            case "Koth":
                ModeChoose("King of the hill - One for One.json", mapPool.KothPool);
                break;
            case "Bomb":
                ModeChoose("DeathMatch.json", mapPool.DeathmatchPool);
                break;
            case "Custom":
                hostPanel.topPanel.SetActive(true);
                fileModeName = "DeathMatch.json";
                break;
            default:
                print("error");
                break;
        }

        Refresh();
        RefreshMaps();
    }

    private void ModeChoose(string ModeFileName, List<string> mapPool)
    {
        mapPoolChoose = mapPool;
        fileModeName = ModeFileName;
        hostPanel.topPanel.SetActive(false);
        chooseMapRandomly();
    }
    private void chooseMapRandomly()
    {
        int x = Random.Range(0, mapPoolChoose.Count);
        map = mapPoolChoose[x];
        SetMap(map);
    }

    private void RefreshMaps()
    {
        foreach (MapElement _map in allMaps)
        {
            if (_map.nameMap == PhotonNetwork.CurrentRoom.GetMap())
            {
                _map.SetMap();
                return;
            }
        }
    }
}
