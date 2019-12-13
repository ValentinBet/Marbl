using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollsion : MonoBehaviour
{
    public Rigidbody myRigid;

    private void OnCollisionEnter(Collision collision)
    {
        if (myRigid.velocity.magnitude > collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude)
        {
            print(gameObject.name + " : Im fast");
        }
    }
}
