using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaAngle : MonoBehaviour
{

    public UnityEngine.UI.Text text;
    public Transform A;
    public Transform B;

    void Update()
    {
        text.text = "Delta Angle : " + (360-A.rotation.eulerAngles.y+ B.rotation.eulerAngles.y);
    }
}
