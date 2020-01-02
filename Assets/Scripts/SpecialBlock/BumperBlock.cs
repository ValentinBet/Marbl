using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBlock : MonoBehaviour
{
    public float radius = 1;
    public float upForce = 0f;
    public float power = 0.2f;

    public Vector3 ExplosionOffset = Vector3.zero;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            GameObject _ball = collision.collider.gameObject;

            if (_ball.GetComponent<PhotonView>().IsMine)
            {
                _ball.GetComponent<Rigidbody>().AddExplosionForce(power, this.transform.position + ExplosionOffset, radius, upForce, ForceMode.Impulse);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + ExplosionOffset, radius);
    }
}
