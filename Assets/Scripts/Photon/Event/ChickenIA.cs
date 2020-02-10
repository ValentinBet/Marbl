using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenIA : MonoBehaviour
{
    public Animator myAnimator;
    PhotonView pv;

    public GameObject EggParticule;

    private void Start()
    {
        pv = GetComponent<PhotonView>();

        Destroy(Instantiate(EggParticule, transform.position, Random.rotation), 2);

        if (!PhotonNetwork.IsMasterClient)
        {
            this.enabled = false;
            return;
        }

        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        EventManager.Instance.canDrop = true;
    }
}
