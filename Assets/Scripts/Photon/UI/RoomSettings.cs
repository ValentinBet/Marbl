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
    bool isOpen = false;

    public Transform parent;

    public GameObject roomSettingsLabel;
    public GameObject saveSettingsObj;
    public Text saveSettingsText;

    [Header("Room")]
    public DropdownSettings gameMode;
    public CheckBoxSettings deathmatch;
    public CheckBoxSettings hill;
    public CheckBoxSettings coins;
    public CheckBoxSettings potato;
    public CheckBoxSettings hue;
    public CheckBoxSettings billard;
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

    [Header("POTATO")]
    public SliderSettings potatoTurnMin;
    public SliderSettings potatoTurnMax;

    [Header("HUE")]
    public SliderSettings hueNutralBall;

    [Header("BILLARD")]
    public SliderSettings billardBall;


    [Header("List obj at disable")]
    public List<GameObject> deathMatchObj;
    public List<GameObject> hillObj;
    public List<GameObject> coinsObj;
    public List<GameObject> hotCustomObj;
    public List<GameObject> hueObj;
    public List<GameObject> billardObj;
    public List<GameObject> ballObj;

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

        ShowHide(ballObj, true);


        if (deathmatch.statut)
        {
            ShowHide(deathMatchObj, true);
        }
        else
        {
            ShowHide(deathMatchObj, false);
        }

        if (hill.statut)
        {
            ShowHide(hillObj, true);
        }
        else
        {
            ShowHide(hillObj, false);
        }

        if (hue.statut)
        {
            ShowHide(hueObj, true);
        }
        else
        {
            ShowHide(hueObj, false);
        }


        ShowHide(coinsObj, false);
        ShowHide(hotCustomObj, false);
        ShowHide(billardObj, false);

        /*
        if (coins.statut)
        {
            ShowHide(coinsObj, true);
        }
        else
        {
            ShowHide(coinsObj, false);
        }

        if (potato.statut)
        {
            ShowHide(hotCustomObj, true);
        }
        else
        {
            ShowHide(hotCustomObj, false);
        }

        if (billard.statut)
        {
            ShowHide(billardObj, true);
        }
        else
        {
            ShowHide(billardObj, false);
        }
        */
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


        PhotonNetwork.CurrentRoom.SetDeathmatch(deathmatch.statut);
        if (deathmatch.statut)
        {
            PhotonNetwork.CurrentRoom.SetWinPointDM(Mathf.RoundToInt(winPointDM.mySlider.value));
            PhotonNetwork.CurrentRoom.SetElimPointDM(Mathf.RoundToInt(elimPointDM.mySlider.value));
            PhotonNetwork.CurrentRoom.SetKillstreakDMProp(killstreakDM.statut);
            PhotonNetwork.CurrentRoom.SetSuicidePointDM(Mathf.RoundToInt(suicidePointDM.mySlider.value));
        }

        PhotonNetwork.CurrentRoom.SetHill(hill.statut);
        if (hill.statut)
        {
            PhotonNetwork.CurrentRoom.SetNbrHill(Mathf.RoundToInt(nbrHill.mySlider.value));
            PhotonNetwork.CurrentRoom.SetHillMode(Mathf.RoundToInt(hillMode.myDropdown.value));
            PhotonNetwork.CurrentRoom.SetHillPoint(Mathf.RoundToInt(hillPoint.mySlider.value));
        }

        PhotonNetwork.CurrentRoom.SetCoins(coins.statut);
        if (coins.statut)
        {
            PhotonNetwork.CurrentRoom.SetCoinsAmount(Mathf.RoundToInt(coinsAmount.myDropdown.value));
        }

        PhotonNetwork.CurrentRoom.SetPotato(potato.statut);
        if (potato.statut)
        {
            PhotonNetwork.CurrentRoom.SetPotatoTurnMin(Mathf.RoundToInt(potatoTurnMin.mySlider.value));
            PhotonNetwork.CurrentRoom.SetPotatoTurnMax(Mathf.RoundToInt(potatoTurnMax.mySlider.value));
        }

        PhotonNetwork.CurrentRoom.SetHue(hue.statut);
        if (hue.statut)
        {
            PhotonNetwork.CurrentRoom.SetHueNutralBall(Mathf.RoundToInt(hueNutralBall.mySlider.value));
        }

        PhotonNetwork.CurrentRoom.SetBillard(billard.statut);
        if (billard.statut)
        {
            PhotonNetwork.CurrentRoom.SetBillardBall(Mathf.RoundToInt(billardBall.mySlider.value));
        }
    }

    public void SetPreset()
    {
        if (gameMode.myDropdown.value + 1 == gameMode.myDropdown.options.Count)
        {
            foreach (Transform element in parent)
            {
                element.gameObject.SetActive(true);
            }
            Refresh();

            //add pour les profs
            coins.gameObject.SetActive(false);
            billard.gameObject.SetActive(false);
            potato.gameObject.SetActive(false);
            return;
        }

        foreach (Transform element in parent)
        {
            element.gameObject.SetActive(false);
        }

        gameMode.gameObject.SetActive(true);
        roomSettingsLabel.gameObject.SetActive(true);

        string nameModeFile = gameMode.myDropdown.options[gameMode.myDropdown.value].text;
        nameModeFile = nameModeFile.Replace("<color=red>", "");
        nameModeFile = nameModeFile.Replace("</color>", "");

        WWW data = new WWW(Application.streamingAssetsPath + "/GameModes/" + nameModeFile + ".json");
        GameModeSettings modeSettings = new GameModeSettings();

        modeSettings = JsonUtility.FromJson<GameModeSettings>(data.text);

        deathmatch.SetValue(modeSettings.deathmatch);
        hill.SetValue(modeSettings.hill);
        coins.SetValue(modeSettings.coins);
        potato.SetValue(modeSettings.potato);
        hue.SetValue(modeSettings.hue);
        billard.SetValue(modeSettings.billard);
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
        potatoTurnMin.mySlider.value = modeSettings.potatoTurnMin;
        potatoTurnMax.mySlider.value = modeSettings.potatoTurnMax;
        hueNutralBall.mySlider.value = modeSettings.hueNutralBall;
        billardBall.mySlider.value = modeSettings.billardBall;
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

        deathmatch.SetValue(modeSettings.deathmatch);
        hill.SetValue(modeSettings.hill);
        coins.SetValue(modeSettings.coins);
        potato.SetValue(modeSettings.potato);
        hue.SetValue(modeSettings.hue);
        billard.SetValue(modeSettings.billard);
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
        potatoTurnMin.mySlider.value = modeSettings.potatoTurnMin;
        potatoTurnMax.mySlider.value = modeSettings.potatoTurnMax;
        hueNutralBall.mySlider.value = modeSettings.hueNutralBall;
        billardBall.mySlider.value = modeSettings.billardBall;

        SaveSettings();
    }

    public void SaveCustomSettings()
    {
        SaveFile();
    }

    void SaveFile()
    {
        GameModeSettings modeSettings = new GameModeSettings();

        modeSettings.deathmatch = deathmatch.statut;
        modeSettings.hill = hill.statut;
        modeSettings.coins = coins.statut;
        modeSettings.potato = potato.statut;
        modeSettings.hue = hue.statut;
        modeSettings.billard = billard.statut;
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
        modeSettings.potatoTurnMin = Mathf.RoundToInt(potatoTurnMin.mySlider.value);
        modeSettings.potatoTurnMax = Mathf.RoundToInt(potatoTurnMax.mySlider.value);
        modeSettings.hueNutralBall = Mathf.RoundToInt(hueNutralBall.mySlider.value);
        modeSettings.billardBall = Mathf.RoundToInt(billardBall.mySlider.value);

        string toj = JsonUtility.ToJson(modeSettings);

        File.WriteAllText(Application.streamingAssetsPath + "/GameModesCustom/" + saveSettingsText.text + ".json", toj);
        Refresh();
    }
}
