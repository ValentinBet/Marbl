using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using static UnityEngine.UI.Dropdown;
using System.IO;
using UnityEditor;

public class RoomSettings : MonoBehaviour
{
    public Text saveSettingsText;

    [Header("Room")]
    public bool deathmatch = true;
    public bool hill = false;
    public bool coins = false;
    public bool hue = false;

    public SliderSettings turnLimit;
    public SliderSettings round;

    [Header("Ball")]
    public SliderSettings nbrBall;
    public DropdownSettings spawnMode;
    public SliderSettings launchPower;
    public SliderSettings impactPower;

    [Header("DM")]
    public SliderSettings winPointDM;
    public SliderSettings elimPointDM;
    public CheckBoxSettings killstreakDM;
    public SliderSettings suicidePointDM;

    [Header("HILL")]
    public SliderSettings nbrHill;
    public DropdownSettings hillMode;
    public SliderSettings hillPoint;

    [Header("COINS")]
    public DropdownSettings coinsAmount;

    [Header("HUE")]
    public SliderSettings hueNutralBall;


    private static RoomSettings _instance;
    public static RoomSettings Instance { get { return _instance; } }

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

    private void Start()
    {
        Refresh();
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        turnLimit.gameObject.SetActive(true);
        round.gameObject.SetActive(true);
    }

    void ShowHide(List<GameObject> elements, bool value)
    {
        foreach (GameObject element in elements)
        {
            element.SetActive(value);
        }
    }

    public void SaveSettings()
    {
        PhotonNetwork.CurrentRoom.SetTurnLimit(Mathf.RoundToInt(turnLimit.mySlider.value));
        PhotonNetwork.CurrentRoom.SetRoundProp(Mathf.RoundToInt(round.mySlider.value));

        PhotonNetwork.CurrentRoom.SetNbrBallProp(Mathf.RoundToInt(nbrBall.mySlider.value));
        PhotonNetwork.CurrentRoom.SetSpawnMode(Mathf.RoundToInt(spawnMode.myDropdown.value));
        PhotonNetwork.CurrentRoom.SetLaunchPower(launchPower.mySlider.value);
        PhotonNetwork.CurrentRoom.SetImpactPower(impactPower.mySlider.value);


        PhotonNetwork.CurrentRoom.SetDeathmatch(deathmatch);
        if (deathmatch)
        {
            PhotonNetwork.CurrentRoom.SetWinPointDM(Mathf.RoundToInt(winPointDM.mySlider.value));
            PhotonNetwork.CurrentRoom.SetElimPointDM(Mathf.RoundToInt(elimPointDM.mySlider.value));
            PhotonNetwork.CurrentRoom.SetKillstreakDMProp(killstreakDM.statut);
            PhotonNetwork.CurrentRoom.SetSuicidePointDM(Mathf.RoundToInt(suicidePointDM.mySlider.value));
        }

        PhotonNetwork.CurrentRoom.SetHill(hill);
        if (hill)
        {
            PhotonNetwork.CurrentRoom.SetNbrHill(Mathf.RoundToInt(nbrHill.mySlider.value));
            PhotonNetwork.CurrentRoom.SetHillMode(Mathf.RoundToInt(hillMode.myDropdown.value));
            PhotonNetwork.CurrentRoom.SetHillPoint(Mathf.RoundToInt(hillPoint.mySlider.value));
        }

        PhotonNetwork.CurrentRoom.SetCoins(coins);
        if (coins)
        {
            PhotonNetwork.CurrentRoom.SetCoinsAmount(Mathf.RoundToInt(coinsAmount.myDropdown.value));
        }

        PhotonNetwork.CurrentRoom.SetHue(hue);
        if (hue)
        {
            PhotonNetwork.CurrentRoom.SetHueNutralBall(Mathf.RoundToInt(hueNutralBall.mySlider.value));
        }
    }

    public void SetPreset()
    {
        string nameModeFile = "";

        WWW data = new WWW(Application.streamingAssetsPath + "/GameModes/" + nameModeFile + ".json");
        GameModeSettings modeSettings = new GameModeSettings();

        modeSettings = JsonUtility.FromJson<GameModeSettings>(data.text);

        deathmatch = modeSettings.deathmatch;
        hill = modeSettings.hill;
        coins = modeSettings.coins;
        hue = modeSettings.hue;
        turnLimit.mySlider.value = modeSettings.turnLimit;
        round.mySlider.value = modeSettings.round;
        nbrBall.mySlider.value = modeSettings.nbrBall;
        spawnMode.myDropdown.value = modeSettings.spawnMode;
        launchPower.mySlider.value = modeSettings.launchPower;
        impactPower.mySlider.value = modeSettings.impactPower;
        winPointDM.mySlider.value = modeSettings.winPointDM;
        elimPointDM.mySlider.value = modeSettings.elimPointDM;
        killstreakDM.SetValue(modeSettings.killstreakDM);
        suicidePointDM.mySlider.value = modeSettings.suicidePointDM;
        nbrHill.mySlider.value = modeSettings.nbrHill;
        hillMode.myDropdown.value = modeSettings.hillMode;
        hillPoint.mySlider.value = modeSettings.hillPoint;
        coinsAmount.myDropdown.value = modeSettings.coinsAmount;
        hueNutralBall.mySlider.value = modeSettings.hueNutralBall;
    }

    public void SetMode(int indexFile)
    {
        var info = new DirectoryInfo(Application.streamingAssetsPath + "/GameModesDefault/");
        var fileInfo = info.GetFiles();

        WWW data = null;

        int i = 0;
        foreach (FileInfo file in fileInfo)
        {
            if (file.Name.Contains(".meta"))
            {
                continue;
            }

            string modeName = file.Name.Replace(".json", "");

            if (i == indexFile)
            {
                data = new WWW(Application.streamingAssetsPath + "/GameModesDefault/" + modeName + ".json");
                break;
            }
            i++;
        }

        GameModeSettings modeSettings = new GameModeSettings();

        modeSettings = JsonUtility.FromJson<GameModeSettings>(data.text);

        deathmatch = modeSettings.deathmatch;
        hill = modeSettings.hill;
        coins = modeSettings.coins;
        hue = modeSettings.hue;
        turnLimit.mySlider.value = modeSettings.turnLimit;
        round.mySlider.value = modeSettings.round;
        nbrBall.mySlider.value = modeSettings.nbrBall;
        spawnMode.myDropdown.value = modeSettings.spawnMode;
        launchPower.mySlider.value = modeSettings.launchPower;
        impactPower.mySlider.value = modeSettings.impactPower;
        winPointDM.mySlider.value = modeSettings.winPointDM;
        elimPointDM.mySlider.value = modeSettings.elimPointDM;
        killstreakDM.SetValue(modeSettings.killstreakDM);
        suicidePointDM.mySlider.value = modeSettings.suicidePointDM;
        nbrHill.mySlider.value = modeSettings.nbrHill;
        hillMode.myDropdown.value = modeSettings.hillMode;
        hillPoint.mySlider.value = modeSettings.hillPoint;
        coinsAmount.myDropdown.value = modeSettings.coinsAmount;
        hueNutralBall.mySlider.value = modeSettings.hueNutralBall;

        SaveSettings();
    }

    public void SaveFile()
    {
        GameModeSettings modeSettings = new GameModeSettings();

        modeSettings.deathmatch = deathmatch;
        modeSettings.hill = hill;
        modeSettings.coins = coins;
        modeSettings.hue = hue;
        modeSettings.turnLimit = Mathf.RoundToInt(turnLimit.mySlider.value);
        modeSettings.round = Mathf.RoundToInt(round.mySlider.value);
        modeSettings.nbrBall = Mathf.RoundToInt(nbrBall.mySlider.value);
        modeSettings.spawnMode = spawnMode.myDropdown.value;
        modeSettings.launchPower = launchPower.mySlider.value;
        modeSettings.impactPower = impactPower.mySlider.value;
        modeSettings.winPointDM = Mathf.RoundToInt(winPointDM.mySlider.value);
        modeSettings.elimPointDM = Mathf.RoundToInt(elimPointDM.mySlider.value);
        modeSettings.killstreakDM = killstreakDM.defaultValue;
        modeSettings.suicidePointDM = Mathf.RoundToInt(suicidePointDM.mySlider.value);
        modeSettings.nbrHill = Mathf.RoundToInt(nbrHill.mySlider.value);
        modeSettings.hillMode = hillMode.myDropdown.value;
        modeSettings.hillPoint = Mathf.RoundToInt(hillPoint.mySlider.value);
        modeSettings.coinsAmount = coinsAmount.myDropdown.value;
        modeSettings.hueNutralBall = Mathf.RoundToInt(hueNutralBall.mySlider.value);

        string toj = JsonUtility.ToJson(modeSettings);

        File.WriteAllText(Application.streamingAssetsPath + "/GameModesCustom/" + saveSettingsText.text + ".json", toj);
        Refresh();
    }
}
