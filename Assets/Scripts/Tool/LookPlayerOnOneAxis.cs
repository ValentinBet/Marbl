using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayerOnOneAxis : MonoBehaviour
{
    Transform camTrans;


    private void Start()
    {
        camTrans = Camera.main.transform;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 lookPos = camTrans.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);
    }
}
