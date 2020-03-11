using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    public Transform selector;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray,out hit))
        {
            selector.transform.position = new Vector3(Mathf.RoundToInt(hit.point.x), 0.1f, Mathf.RoundToInt(hit.point.z));
        }
    }
}
