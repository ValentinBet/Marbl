using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    GameObject followObj;
    public Transform forceCam;

    Rigidbody currentRigidbody;
    float currentVelocity = 0;

    PhotonView pv;

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
            currentVelocity = Mathf.Lerp(currentVelocity, currentRigidbody.velocity.magnitude, Time.deltaTime * 10);

            forceCam.position = Vector3.MoveTowards(forceCam.position, followObj.transform.position, 100 * Time.deltaTime);
        }
        else
        {
            currentVelocity = 0;
        }
    }

    public void EndTurn()
    {
        StartCoroutine(SpawnGift(3));
    }

    public void EndRound()
    {

    }

    IEnumerator SpawnGift(int number)
    {
        PhotonNetwork.CurrentRoom.SetForceMap(true);
        for (int i = 0; i < number; i++)
        {
            GameObject newGift = PhotonNetwork.Instantiate("Gift", GetPosition(), Quaternion.Euler(0, Random.Range(0, 360), 0));

            Rigidbody myBody = newGift.GetComponent<Rigidbody>();

            //myBody.AddTorque(newGift.transform.position / 1000, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);

            while (currentVelocity > 0.1f)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.1f);
        }

        PhotonNetwork.CurrentRoom.SetForceMap(false);

        pv.RPC("RpcSetFollowObjNull", RpcTarget.All);

        yield return null;
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
