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
    public float speedBoostPowerStay = 1.3f;
    public float addPowerAtHighSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<BallSettings>().SpawnOverchargedFx();
            audioSource.PlayOneShot(electricSound);

            //if (other.GetComponent<Rigidbody>() != null && other.GetComponent<PhotonView>().IsMine)
            //{
            //    Rigidbody rb = other.GetComponent<Rigidbody>();
            //    Vector3 _newVel = rb.velocity.normalized;
            //    _newVel = new Vector3(_newVel.x, 0, _newVel.z);

            //    if (rb.velocity.sqrMagnitude > ((_newVel * speedBoostPower).sqrMagnitude))
            //    {
            //        rb.velocity *= addPowerAtHighSpeed;
            //    } else
            //    {
            //        rb.velocity = _newVel * speedBoostPower;
            //    }
                
            //}
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (other.GetComponent<Rigidbody>() != null && other.GetComponent<PhotonView>().IsMine)
            {
                other.GetComponent<Rigidbody>().velocity *= speedBoostPowerStay;
            }
        }
    }
}
