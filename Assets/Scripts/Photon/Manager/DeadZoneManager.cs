using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            DeathMatchManager.Instance.OnMarblDie(other.gameObject);
        }
    }
}
