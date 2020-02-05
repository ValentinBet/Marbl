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
            currentVelocity = Mathf.Lerp(currentVelocity, currentRigidbody.velocity.magnitude, 1 * Time.deltaTime);

            forceCam.position = Vector3.MoveTowards(forceCam.position, followObj.transform.position, 100 * Time.deltaTime);
        }
    }

    public void EndTurn()
    {
        StartCoroutine(SpawnGift(1));
    }

    public void EndRound()
    {

    }

    IEnumerator SpawnGift(int number)
    {
        PhotonNetwork.CurrentRoom.SetForceMap(true);
        for (int i = 0; i < number; i++)
        {
            GameObject newGift = PhotonNetwork.Instantiate("Gift", GetPosition(), Quaternion.identity);
            followObj = newGift;

            currentRigidbody = newGift.GetComponent<Rigidbody>();

            while (currentVelocity > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2f);
        }

        PhotonNetwork.CurrentRoom.SetForceMap(false);
        followObj = null;

        yield return null;
    }

    Vector3 GetPosition()
    {
        Vector3 randomPos = new Vector3(Random.Range(-50, 50), 20, Random.Range(-50, 50));

        RaycastHit hit;

        while(!Physics.Raycast(randomPos, transform.TransformDirection(Vector3.down), out hit, 200))
        {
            randomPos = new Vector3(Random.Range(-50, 50), 20, Random.Range(-50, 50));
        }

        randomPos = new Vector3(randomPos.x, 20, randomPos.z);

        return randomPos;
    }
}
