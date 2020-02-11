﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaDropObj : MonoBehaviour
{
    GameObject newGift;

    void Start()
    {
        EventManager.Instance.SetFollowObj(transform.GetChild(0).gameObject);

        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        StartCoroutine(SpawnEgg());
    }
    
    IEnumerator SpawnEgg()
    {
        yield return new WaitForSeconds(2);

        DropDontFollow();

        yield return new WaitForSeconds(0.2f);

        DropDontFollow();

        yield return new WaitForSeconds(0.2f);

        Drop();

        yield return new WaitForSeconds(3);

        EventManager.Instance.canDrop = true;

        PhotonNetwork.Destroy(gameObject.GetPhotonView());
    }

    void Drop()
    {
        newGift = PhotonNetwork.Instantiate("Egg", transform.GetChild(0).position, Quaternion.Euler(0, Random.Range(0, 360), 0));

        Rigidbody myBody = newGift.GetComponent<Rigidbody>();
        myBody.AddTorque(newGift.transform.position / 1000, ForceMode.Force);
    }

    void DropDontFollow()
    {
        newGift = PhotonNetwork.Instantiate("Egg_DontFollow", transform.GetChild(0).position, Quaternion.Euler(0, Random.Range(0, 360), 0));

        Rigidbody myBody = newGift.GetComponent<Rigidbody>();
        myBody.AddTorque(newGift.transform.position / 1000, ForceMode.Force);
    }
}
