using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour {

    private Rigidbody rb;
    private CatapultShoot reference;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetForce(Vector3 force, CatapultShoot parentCS)
    {
        reference = parentCS;
        rb.AddForce(force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            StartCoroutine("TimedDestruction",0.1f);

        }
        if (collision.gameObject.layer == 10)
        {
            rb.velocity = Vector3.zero;
            Debug.Log("CONGRATS !");
            StartCoroutine("TimedDestruction",3.0f);
        }
    }

    IEnumerator TimedDestruction(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
