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
    public AudioSource audioSource;
    public AudioClip ballHit;

    public Animator myAnimator;

    public GameObject fxPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            myAnimator.SetTrigger("Boom");

            Destroy(Instantiate(fxPrefab, collision.contacts[0].point, Random.rotation), 2);

            GameObject _ball = collision.collider.gameObject;

            if (ballHit != null)
            {
                audioSource.PlayOneShot(ballHit);
            }

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
