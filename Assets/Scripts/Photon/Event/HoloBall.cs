using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class HoloBall : MonoBehaviour
{
    Renderer myRenderer;
    public Animator myAnimator;

    PhotonView pv;

    bool isDestroying = false;

    public PingElement pingObj;

    public Team myTeam;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        transform.rotation = Random.rotation;

        pv = GetComponent<PhotonView>();

        if(GameModeManager.Instance.localPlayerTeam == myTeam)
        {
            pingObj.SetAlphaCircles(0.3f);
        }
        else
        {
            pingObj.SetAlphaCircles(1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        myAnimator.SetBool("Blink", true);

        if (other.tag == "Ball")
        {
            if (PhotonNetwork.IsMasterClient && !isDestroying)
            {
                isDestroying = true;
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        PhotonNetwork.Destroy(pv);
    }

    private void OnTriggerExit(Collider other)
    {
        myAnimator.SetBool("Blink", false);
    }

    private void OnTriggerStay(Collider other)
    {
        myAnimator.SetBool("Blink", true);
    }
}
