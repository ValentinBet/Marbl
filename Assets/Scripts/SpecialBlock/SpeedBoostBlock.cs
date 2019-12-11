using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && other.GetComponent<Rigidbody>() != null && other.GetComponent<PhotonView>().IsOwnerActive)
        {
            other.GetComponent<Rigidbody>().velocity *= 2;
        }
    }
}
