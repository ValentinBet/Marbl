using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovements : MonoBehaviour
{
    [SerializeField]
    private float powerMultiplier = 1;
    private Rigidbody rb;
    private string role;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveBall(Vector3 direction, float angle, float dragForce)
    {
        direction = new Vector3(direction.x *Mathf.Cos(Mathf.Deg2Rad*angle), ((45 - angle) / 45.0f + 1) * Mathf.Sin(Mathf.Deg2Rad*angle), direction.z*Mathf.Cos(Mathf.Deg2Rad*angle));
        Debug.Log(direction);
        Vector3 _impulse = direction * (dragForce * powerMultiplier);
        this.GetComponent<Rigidbody>().AddForce(_impulse, ForceMode.Impulse);
    }
    public void colSeq(Collider other)
    {
        if (other.gameObject.GetComponent<BallMovements>() !=null)
        {
            string colliderName = gameObject.name;
            Debug.Log(rb.velocity.sqrMagnitude + "  " + gameObject.name);
            Debug.Log(other.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude + "  " + other.gameObject.name);
            role = "Killer";
            if (other.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > rb.velocity.sqrMagnitude)
            {
                colliderName = other.gameObject.name;
                role = "Victim";
            }
            Debug.Log(gameObject.name + " colllided with " + other.gameObject.name + " and " + colliderName + " initiated this collision");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallMovements>()!=null)
        {
            if (role == "Victim")
            {
                rb.velocity *= 2.6f;
            } else
            {
                rb.velocity *= 1.3f;
            }
            role = "";
        }
    }
}
