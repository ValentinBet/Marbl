using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public GameObject followObj;
    public Transform forceCam;

    Rigidbody currentRigidbody;

    PhotonView pv;

    int numberOfBonus = 0;
    public bool canDrop = true;

    private static EventManager _instance;
    public static EventManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(followObj != null)
        {
            forceCam.position = Vector3.MoveTowards(forceCam.position, followObj.transform.position, 20 * Time.deltaTime);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (canDrop)
        {
            if(numberOfBonus > 0)
            {
                SpawnGift();
            }
            else
            {
                if (PhotonNetwork.CurrentRoom.GetForceCam())
                {
                    PhotonNetwork.CurrentRoom.SetForceMap(false);
                }
            }
        }
    }

    public void EndTurn()
    {
        numberOfBonus = 3;
    }

    public void EndRound()
    {

    }

    void SpawnGift()
    {
        numberOfBonus--;
        canDrop = false;
        PhotonNetwork.CurrentRoom.SetForceMap(true);
        GameObject newGift = PhotonNetwork.Instantiate("SeagullDrop", GetPosition(), Quaternion.Euler(0, Random.Range(0, 360), 0));
    }

    void EndDrop()
    {
        PhotonNetwork.CurrentRoom.SetForceMap(false);

        pv.RPC("RpcSetFollowObjNull", RpcTarget.All);
    }

    Vector3 GetPosition()
    {
        RaycastHit hit;
        Vector3 randomPos = Vector3.zero;

        do
        {
            randomPos = new Vector3(Random.Range(-50f, 50f), 20, Random.Range(-50f, 50f));
            if(!Physics.Raycast(randomPos, transform.TransformDirection(Vector3.down), out hit, 200))
            {
                continue;
            }

        } while (hit.collider == null || hit.collider.gameObject.layer != 10);

        randomPos = new Vector3(randomPos.x, 20, randomPos.z);

        return randomPos;
    }


    [PunRPC]
    void RpcSetFollowObjNull()
    {
        followObj = null;
    }

    public void SetFollowObj(GameObject obj)
    {
        followObj = obj;
        currentRigidbody = obj.GetComponent<Rigidbody>();
    }
}
