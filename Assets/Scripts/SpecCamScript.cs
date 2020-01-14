using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecCamScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CameraManager.Instance.CameraSpec = transform;
    }

}
