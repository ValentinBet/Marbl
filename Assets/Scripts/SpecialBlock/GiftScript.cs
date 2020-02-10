using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour
{
    
    void Start()
    {
        EventManager.Instance.SetFollowObj(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Chicken", transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }
}
