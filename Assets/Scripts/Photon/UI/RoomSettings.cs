using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class RoomSettings : MonoBehaviour
{
    public Animator myAnimator;
    bool isOpen = false;



    [Header("Room")]

    public MapSettings map;
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
    }

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
}
