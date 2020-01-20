﻿using Photon.Pun;
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

    public string map = "Close.json";

    public bool customMode = false;
    public bool customModeSave = false;
    public string fileModeName;

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
        PhotonNetwork.CurrentRoom.SetMap(map);

        Refresh();
    }




    public void SetMode(Image img)
    {

        OutlinedMode.color = Color.white;
        OutlinedMode = img;
        OutlinedMode.color = new Color(1, 0.1986281f, 0);

        CustomLabel.SetActive(false);
    }

    public void SetCustom(Image img)
    {
        OutlinedMode.color = Color.white;
        OutlinedMode = img;
        OutlinedMode.color = new Color(1, 0.1986281f, 0);

        customMode = true;
        customModeSave = false;
        CustomLabel.SetActive(true);
    }

    public void SetMap(string value)
    {
        map = value;
        PhotonNetwork.CurrentRoom.SetMap(value);
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
            if(infoMode != "")
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
}
