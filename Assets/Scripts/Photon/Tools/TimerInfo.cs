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

    public Animator myAnimator;
    public AudioClip bipTimer;

    int maxTime;

    int oldValue = 6;

    void Start()
    {
        maxTime = PhotonNetwork.CurrentRoom.GetTurnLimit();
        SetTime(maxTime);
    }

    public void SetTime(int value)
    {
        mytext.text = value.ToString();
        myImg.fillAmount = (float)value / (float)maxTime;

        if(value <= 5 && oldValue != value)
        {
            oldValue = value;
            myAnimator.SetTrigger("Shake");
            AudioManager.Instance.PlayThisSound(bipTimer);
        }
    }
}
