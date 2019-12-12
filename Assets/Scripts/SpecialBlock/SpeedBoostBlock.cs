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
        if (other.CompareTag("Ball") && other.GetComponent<Rigidbody>() != null && other.GetComponent<PhotonView>().IsOwnerActive)
        {
            other.GetComponent<Rigidbody>().velocity *= 2;
            other.GetComponent<BallSettings>().SpawnOverchargedFx();
            audioSource.PlayOneShot(electricSound);

            fx_ElectricGround.SetActive(false);
        }
    }
}
