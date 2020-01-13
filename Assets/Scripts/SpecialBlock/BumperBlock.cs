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
                Rigidbody _rb = _ball.GetComponent<Rigidbody>();
                _rb.AddExplosionForce(power, this.transform.position + ExplosionOffset, radius, upForce, ForceMode.Impulse);
                _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + ExplosionOffset, radius);
    }
}
