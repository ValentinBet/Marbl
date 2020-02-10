using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObj : MonoBehaviour
{
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

        GameObject newGift = PhotonNetwork.Instantiate("Egg", transform.GetChild(0).position, Quaternion.Euler(0, Random.Range(0, 360), 0));

        Rigidbody myBody = newGift.GetComponent<Rigidbody>();
        myBody.AddTorque(newGift.transform.position / 1000, ForceMode.Force);

        yield return new WaitForSeconds(3);

        PhotonNetwork.Destroy(gameObject.GetPhotonView());
    }
}
