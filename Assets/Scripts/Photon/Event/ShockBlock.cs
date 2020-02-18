using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockBlock : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip takingSound;

    private bool isTake = false;

    public GameObject powerBall;

    public PhotonView pv;

    private void Start()
    {
        EventManager.Instance.SetFollowObj(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTake && other.CompareTag("Ball"))
        {
            GivePower(other);

            HidePower();

            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(wait());
            }
        }
    }

    private void GivePower(Collider other)
    {
        isTake = true;

        if (GameModeManager.Instance.localPlayerTurn)
        {
            other.GetComponent<BallSettings>().isPowered = true;
        }
    }

    void HidePower()
    {
        powerBall.SetActive(false);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.Destroy(pv);
    }
}

