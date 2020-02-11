using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour
{
    bool haveSpawnChicken = false;

    public bool follow = true;

    void Start()
    {
        if (follow)
        {
            EventManager.Instance.SetFollowObj(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient && !haveSpawnChicken)
        {
            haveSpawnChicken = true;
            GameObject newChicken = PhotonNetwork.Instantiate("Chicken", transform.position + Vector3.up * 0.5f, Quaternion.Euler(0, Random.Range(0, 360), 0));

            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }
}
