using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamScript : MonoBehaviour
{
    Transform MYcamera;

    void Start()
    {
        MYcamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(MYcamera);
    }
}
