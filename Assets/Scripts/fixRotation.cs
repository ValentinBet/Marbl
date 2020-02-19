using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixRotation : MonoBehaviour
{
    public Vector3 rotation = new Vector3(0,0,0);

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(rotation);
    }
}
