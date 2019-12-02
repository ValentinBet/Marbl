using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglePreview : MonoBehaviour
{
    public Transform angleToTrack;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0,Vector3.SignedAngle(Vector3.left, angleToTrack.position,Vector3.up),0);
    }
}
