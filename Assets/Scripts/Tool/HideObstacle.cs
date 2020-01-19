using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObstacle : MonoBehaviour
{
    public LayerMask ObsAndBall;

    public Renderer objHide;

    public List<Renderer> oldObs = new List<Renderer>();

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {

            if (hit.transform.gameObject.layer == 14)
            {
                if (objHide != hit.transform.gameObject.GetComponent<Renderer>())
                {
                    if (objHide != null)
                    {
                        oldObs.Add(objHide);
                    }

                    objHide = hit.transform.gameObject.GetComponent<Renderer>();
                }

                if (oldObs.Contains(hit.transform.gameObject.GetComponent<Renderer>()))
                {
                    oldObs.Remove(hit.transform.gameObject.GetComponent<Renderer>());
                }
            }
            else
            {
                if(objHide != null)
                {
                    oldObs.Add(objHide);
                    objHide = null;
                }
            }
        }

        if(objHide != null)
        {
            foreach (Material mat in objHide.materials)
            {
                float currentAlpha = Mathf.Lerp(mat.GetFloat("_Alpha"), 0.4f, 3 * Time.deltaTime);
                mat.SetFloat("_Alpha", currentAlpha);
            }
        }


        List<Renderer> removeThis = new List<Renderer>();
        foreach(Renderer element in oldObs)
        {
            float currentAlpha = 0;
            foreach (Material mat in element.materials)
            {
                currentAlpha = Mathf.Lerp(mat.GetFloat("_Alpha"), 1f, 3 * Time.deltaTime);
                mat.SetFloat("_Alpha", currentAlpha);
            }

            if(currentAlpha > 0.99f)
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
