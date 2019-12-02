using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsPreview : MonoBehaviour
{
    public Transform A;
    public Transform B;
    public Vector3 velocity;
    /*    public GameObject bA;
        public GameObject bB;*/
    public LayerMask lm;

    void Update()
    {
        Debug.DrawLine(A.position, B.position,Color.cyan);
        Vector3 dir = B.position - A.position;
        Vector3 pA = A.position + dir.normalized/2;
        Debug.DrawLine(pA, pA + Vector3.up * 4, Color.green);
        Debug.DrawRay(pA, -velocity, Color.red);
        //Vector3 pB = B.position - dir.normalized;
        Ray ra = new Ray(pA, -velocity);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ra, out hit, 5, lm))
        {
            Vector3 rpA = hit.point;
            Debug.DrawLine(rpA, rpA + Vector3.up * 4, Color.blue);
            Debug.DrawLine(pA, rpA, Color.magenta);
            Debug.DrawLine(rpA,B.position,Color.yellow);
            Debug.DrawLine(rpA, new Vector3(-B.position.x, B.position.y, B.position.z),Color.grey);
        }

    }
}
