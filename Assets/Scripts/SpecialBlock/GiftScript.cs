using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour
{
    public GameObject prefabParticule;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.SetFollowObj(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag  == "Ball")
        {
            if (GameModeManager.Instance.localPlayerTurn)
            {
                PhotonView pv = GetComponent<PhotonView>();

                pv.RequestOwnership();

                Destroy(Instantiate(prefabParticule, transform.position, Random.rotation), 1);

                pv.RPC("RpcSpawnParticule", RpcTarget.Others, transform.position);

                PhotonNetwork.Destroy(pv);
            }
        }
    }


    [PunRPC]
    void RpcSpawnParticule(Vector3 pos)
    {
        Destroy(Instantiate(prefabParticule, pos, Random.rotation), 1);
    }
}
