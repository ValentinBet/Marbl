using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour
{
    bool haveSpawnChicken = false;
    public bool IsLast = true;

    void Start()
    {
        EventManager.Instance.SetFollowObj(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient && !haveSpawnChicken)
        {
            haveSpawnChicken = true;
            GameObject newChicken = PhotonNetwork.Instantiate("Chicken", transform.position + Vector3.up * 0.5f, Quaternion.Euler(0, Random.Range(0, 360), 0));

            if (IsLast)
            {
                newChicken.GetComponent<ChickenIA>().IsLast = true;
            }

            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }
}
