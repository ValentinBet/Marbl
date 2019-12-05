using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using static UnityEngine.UI.Dropdown;
using System.IO;

public class RoomSettings : MonoBehaviour
{
    public Animator myAnimator;
    bool isOpen = false;

    public Transform parent;

    public GameObject roomSettingsLabel;
    public GameObject saveSettingsObj;
    public Text saveSettingsText;

    [Header("Room")]

    public MapSettings map;
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
        SetDropDownSettings();

        Refresh();
    }

    void SetDropDownSettings()
    {
        var info = new DirectoryInfo(Application.streamingAssetsPath + "/GameModes/");
        var fileInfo = info.GetFiles();

        foreach (FileInfo file in fileInfo)
        {
            if (file.Name.Contains(".meta")){ return; }

            WWW data = new WWW(Application.streamingAssetsPath + "/GameModes/" + file.Name);
        }

        return;

    }
        /*
        List<OptionData> listSettings = new List<OptionData>();

        foreach (GameModeSettings element in scriptableSettings)
        {
            listSettings.Add(new OptionData(element.name.Replace(".asset", "")));
        }

        listSettings.Add(new OptionData("Custom"));

        gameMode.myDropdown.AddOptions(listSettings);
        */
    

    public void Refresh()
    {

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

        if (hue.statut)
        {
            ShowHide(hueObj, true);
        }
        else
        {
            ShowHide(hueObj, false);
        }

        if (billard.statut)
        {
            ShowHide(billardObj, true);
        }
        else
        {
            ShowHide(billardObj, false);
        }
    }

    void ShowHide(List<GameObject> elements, bool value)
    {
        foreach(GameObject element in elements)
        {
            element.SetActive(value);
        }
    }

    public void OpenOrClose()
    {
        myAnimator.enabled = true;
        if (isOpen)
        {
            myAnimator.SetTrigger("Close");
        }
        else
        {
            myAnimator.SetTrigger("Open");
        }
        isOpen = !isOpen;
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
        if(gameMode.myDropdown.value +1 == gameMode.myDropdown.options.Count)
        {
            foreach(Transform element in parent)
            {
                element.gameObject.SetActive(true);
            }
            Refresh();
            return;
        }

        foreach (Transform element in parent)
        {
            element.gameObject.SetActive(false);
        }

        map.gameObject.SetActive(true);
        gameMode.gameObject.SetActive(true);
        roomSettingsLabel.gameObject.SetActive(true);

        GameModeSettings mySettings = Resources.Load<GameModeSettings>("GameModes/" + gameMode.myDropdown.options[gameMode.myDropdown.value].text);

        map.dropMap.value = mySettings.map;
        deathmatch.SetValue(mySettings.deathmatch);
        hill.SetValue(mySettings.hill);
        coins.SetValue(mySettings.coins);
        potato.SetValue(mySettings.potato);
        hue.SetValue(mySettings.hue);
        billard.SetValue(mySettings.billard);
        turnLimit.mySlider.value = mySettings.turnLimit;
        round.mySlider.value = mySettings.round;

        nbrBall.mySlider.value = mySettings.nbrBall;
        spawnMode.myDropdown.value = mySettings.spawnMode;
        launchPower.mySlider.value = mySettings.launchPower;
        impactPower.mySlider.value = mySettings.impactPower;

        winPointDM.mySlider.value = mySettings.winPointDM;
        elimPointDM.mySlider.value = mySettings.elimPointDM;
        killstreakDM.SetValue(mySettings.killstreakDM);
        suicidePointDM.mySlider.value = mySettings.suicidePointDM;

        nbrHill.mySlider.value = mySettings.nbrHill;
        hillMode.myDropdown.value = mySettings.hillMode;
        hillPoint.mySlider.value = mySettings.hillPoint;

        coinsAmount.myDropdown.value = mySettings.coinsAmount;

        potatoTurnMin.mySlider.value = mySettings.potatoTurnMin;
        potatoTurnMax.mySlider.value = mySettings.potatoTurnMax;

        hueNutralBall.mySlider.value = mySettings.hueNutralBall;

        billardBall.mySlider.value = mySettings.billardBall;
    }


    public void SaveCustomSettings()
    {
        
    }
}
