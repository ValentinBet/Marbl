using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostBlock : MonoBehaviour
{
    public GameObject fx_ElectricGround;
    public AudioSource audioSource;

    public AudioClip electricSound;
    public int speedBoostPower;
    public float addPowerAtHighSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<BallSettings>().SpawnOverchargedFx();
            audioSource.PlayOneShot(electricSound);

            if (other.GetComponent<Rigidbody>() != null && other.GetComponent<PhotonView>().IsMine)
            {
                Rigidbody rb = other.GetComponent<Rigidbody>();
                Vector3 _temp = rb.velocity.normalized;
                if (rb.velocity.sqrMagnitude > ((_temp * speedBoostPower).sqrMagnitude))
                {
                    rb.velocity *= addPowerAtHighSpeed;
                } else
                {
                    rb.velocity = _temp * speedBoostPower;
                }
                
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Ball"))
    //    {
    //        if (other.GetComponent<Rigidbody>() != null && other.GetComponent<PhotonView>().IsMine)
    //        {
    //            other.GetComponent<Rigidbody>().velocity *= 1.1f;
    //        }
    //    }
    //}
}
