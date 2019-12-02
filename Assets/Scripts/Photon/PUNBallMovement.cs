using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
public class PUNBallMovement : MonoBehaviour
{
    public float MovementSpeed = 2.0f;
    public float MaxSpeed = 0.2f;
    public bool controllable = true;

    private PhotonView photonView;
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;
    private int amplify = 0;

    private float impactPower;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();

        impactPower = PhotonNetwork.CurrentRoom.GetImpactPower();
        MovementSpeed = PhotonNetwork.CurrentRoom.GetLaunchPower();
    }

    private void Update()
    {
        if (!photonView.IsMine || !controllable)
        {
            return;
        }


    }

    public void MoveBall(Vector3 direction, float angle, float dragForce)
    {
        direction = new Vector3(direction.x * Mathf.Cos(Mathf.Deg2Rad * angle), ((45 - angle) / 45.0f + 1) * Mathf.Sin(Mathf.Deg2Rad * angle), direction.z * Mathf.Cos(Mathf.Deg2Rad * angle));
        Vector3 _impulse = direction * (dragForce * MovementSpeed);

        this.GetComponent<Rigidbody>().AddForce(_impulse, ForceMode.Impulse);
    }


    public void ColSeq(Collider other)
    {
        if (other.gameObject.GetComponent<PUNBallMovement>() != null)
        {
            string colliderName = gameObject.name;

            amplify = 0;
            if (other.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > rigidbody.velocity.sqrMagnitude)
            {
                colliderName = other.gameObject.name;

                amplify = 1;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<PUNBallMovement>() != null)
        {
            if (Mathf.Abs(Mathf.Abs(collision.gameObject.transform.position.y) - Mathf.Abs(transform.position.y)) < 0.3f && collision.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > Physics.bounceThreshold * Physics.bounceThreshold && rigidbody.velocity.sqrMagnitude > Physics.bounceThreshold * Physics.bounceThreshold)
            {
                if (amplify == 0)
                {
                    rigidbody.velocity *= 1.8f * impactPower;
                }
                else
                {
                    rigidbody.velocity *= 1.2f * impactPower;
                }
            }
        }
    }




    //public void MoveBall(Vector3 direction, float dragForce)
    //{
    //    direction = new Vector3(direction.x, 0, direction.z);
    //    Vector3 _impulse = direction * (dragForce * MovementSpeed);

    //    this.GetComponent<Rigidbody>().AddForce(_impulse, ForceMode.Impulse);
    //}
}
