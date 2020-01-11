using UnityEngine;
using System.Collections;
using Photon.Pun;

public class CustomTeleporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.GetComponent<Rigidbody>() != null && other.GetComponent<PhotonView>().IsMine)
            {
                Vector3 _temp = other.GetComponent<Rigidbody>().velocity.normalized;
            }
        }
    }
}
