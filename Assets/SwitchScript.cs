using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchScript : BumperBlock, IPunObservable
{
    [Header("Switch Variables")]
    [SerializeField] private Animator animator;
    [SerializeField] private Text switchText;

    private SwitchMapEventManager switchMapEvent;

    private void Start()
    {
        if (SwitchMapEventManager.Instance != null)
        {
            switchMapEvent = SwitchMapEventManager.Instance;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (animator.GetBool("IsOn") == false)
            {
                animator.SetBool("IsOn", true);
                switchText.text = "ON";
                switchMapEvent.AddSwitchCount(1);
            }
            else
            {
                animator.SetBool("IsOn", false);
                switchText.text = "OFF";
                switchMapEvent.AddSwitchCount(-1);
            }
        }
        Bump(other.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(animator.GetBool("IsOn"));
        }
        else
        {
            animator.SetBool("IsOn",(bool)stream.ReceiveNext());
        }


    }
}
