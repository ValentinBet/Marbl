using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour
{
    bool haveSpawnChicken = false;

    void Start()
    {
        EventManager.Instance.SetFollowObj(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient && !haveSpawnChicken)
        {
            haveSpawnChicken = true;
            PhotonNetwork.Instantiate("Chicken", transform.position + Vector3.up * 0.5f, Quaternion.Euler(0, Random.Range(0, 360), 0));
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }
}
