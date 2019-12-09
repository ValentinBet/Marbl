using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObstacle : MonoBehaviour
{

    public LayerMask ObsAndBall;

    GameObject objHide;

    List<Renderer> oldObs = new List<Renderer>();

    float start;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray forwardRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);


        if (Physics.Raycast(forwardRay, out hit, ObsAndBall))
        {
            if(hit.transform.gameObject.layer == 14)
            {
                if(hit.transform.gameObject != objHide)
                {
                    objHide = hit.transform.gameObject;
                    start = objHide.GetComponent<Renderer>().material.GetFloat("_Alpha");

                }
                else
                {
                    start = Mathf.Lerp(start, 0.2f, 3 * Time.deltaTime);
                    objHide.GetComponent<Renderer>().material.SetFloat("_Alpha", start);
                }
            }
            else
            {
                if (objHide != null && !oldObs.Contains(objHide.GetComponent<Renderer>()))
                {
                    oldObs.Add(objHide.GetComponent<Renderer>());
                }
                objHide = null;
            }
        }
        else
        {
            if (objHide != null && !oldObs.Contains(objHide.GetComponent<Renderer>()))
            {
                oldObs.Add(objHide.GetComponent<Renderer>());
            }
            objHide = null;
        }


        List<Renderer> removeThis = new List<Renderer>();
        foreach(Renderer element in oldObs)
        {
            start = Mathf.Lerp(start, 1f, 3 * Time.deltaTime);
            element.material.SetFloat("_Alpha", start);

            if(start > 0.99f)
            {
                removeThis.Add(element);
            }
        }

        foreach(Renderer element in removeThis)
        {
            oldObs.Remove(element);
        }
    }
}
