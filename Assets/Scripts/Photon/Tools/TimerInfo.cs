using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerInfo : MonoBehaviour
{
    public Image myImg;
    public Text mytext;

    int maxTime;

    void Start()
    {
        maxTime = PhotonNetwork.CurrentRoom.GetTurnLimit();
        SetTime(maxTime);
    }

    public void SetTime(int value)
    {
        mytext.text = value.ToString();
        myImg.fillAmount = (float)value / (float)maxTime;
    }
}
