using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPassif : MonoBehaviour
{
    public bool canPlace = false;

    string passifPrefab;

    Camera myCam;

    public GameObject pointPrefab;
    Transform objPoint;

    GameObject currentObj;

    private void Start()
    {
        myCam = Camera.main;
        objPoint = Instantiate(pointPrefab).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlace)
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                objPoint.gameObject.SetActive(true);
                objPoint.transform.position = hit.point;

                if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton1))
                {
                    currentObj = PhotonNetwork.Instantiate(passifPrefab, hit.point, Quaternion.identity);
                    StartCoroutine(WaitAndDestruct());
                    canPlace = false;
                }
            }
            else
            {
                if(objPoint != null)
                {
                    objPoint.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (objPoint != null)
            {
                objPoint.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator WaitAndDestruct()
    {
        yield return new WaitForSeconds(10);
        PhotonNetwork.Destroy(currentObj);
    }

    public void SetPassif(string element)
    {
        passifPrefab = element;
        canPlace = true;
    }
}
