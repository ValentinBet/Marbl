using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMapEventManager : MonoBehaviour, IPunObservable
{
    public int activatedSwitchCounter = 0;
    public int switchNumber = 4;

    private bool isMapEventTriggered = false;

    private static SwitchMapEventManager _instance;
    public static SwitchMapEventManager Instance { get { return _instance; } }

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
    private void Update()
    {
        if (activatedSwitchCounter == switchNumber && !isMapEventTriggered)
        {
            isMapEventTriggered = true;

            // TRIGGER MAP EVENT          
            SwitchMapEvent();
        }
    }
    public void AddSwitchCount(int value)
    {
        activatedSwitchCounter += value;
    }


    private void SwitchMapEvent()
    {
        print("TRIGGER MAP EVENT");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(activatedSwitchCounter);
        }
        else
        {
            activatedSwitchCounter = (int)stream.ReceiveNext();
        }
    }

}
