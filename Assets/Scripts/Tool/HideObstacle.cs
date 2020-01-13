using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideObstacle : MonoBehaviour
{

    public LayerMask ObsAndBall;

    public List<Renderer> objHide = new List<Renderer>();

    public List<Renderer> oldObs = new List<Renderer>();

    List<GameObject> objTouch = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        List<RaycastHit> hits = new List<RaycastHit>();
        hits.AddRange(Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward));

        objTouch.Clear();

        foreach (RaycastHit hit in hits)
        {
            objTouch.Add(hit.transform.gameObject);
            if (hit.transform.gameObject.layer == 14)
            {
                if (!objHide.Contains(hit.transform.gameObject.GetComponent<Renderer>()))
                {
                    objHide.Add(hit.transform.gameObject.GetComponent<Renderer>());
                }

                if (oldObs.Contains(hit.transform.gameObject.GetComponent<Renderer>()))
                {
                    oldObs.Remove(hit.transform.gameObject.GetComponent<Renderer>());
                }
            }
            break;
        }

        List<Renderer> removeThis = new List<Renderer>();
        foreach (Renderer obj in objHide)
        {
            if( !objTouch.Contains(obj.gameObject))
            {
                oldObs.Add(obj.GetComponent<Renderer>());
                removeThis.Add(obj.GetComponent<Renderer>());
            }
            else
            {
                //Hide

                float currentAlpha = Mathf.Lerp(obj.material.GetFloat("_Alpha"), 0f, 3 * Time.deltaTime);
                obj.material.SetFloat("_Alpha", currentAlpha);
            }
        }

        foreach (Renderer element in removeThis)
        {
            objHide.Remove(element);
        }


        List<Renderer> removeThis2 = new List<Renderer>();
        foreach(Renderer element in oldObs)
        {
            float currentAlpha = Mathf.Lerp(element.material.GetFloat("_Alpha"), 1f, 3 * Time.deltaTime);
            element.material.SetFloat("_Alpha", currentAlpha);

            if(currentAlpha > 0.99f)
            {
                removeThis.Add(element);
            }
        }

        foreach(Renderer element in removeThis2)
        {
            oldObs.Remove(element);
        }
    }
}
