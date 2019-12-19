using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostBlock : MonoBehaviour
{
    public GameObject fx_ElectricGround;
    public AudioSource audioSource;

    public AudioClip electricSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<BallSettings>().SpawnOverchargedFx();
            audioSource.PlayOneShot(electricSound);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.GetComponent<Rigidbody>() != null && other.GetComponent<PhotonView>().IsMine)
            {
                other.GetComponent<Rigidbody>().velocity *= 1.1f;
            }
        }
    }
}
