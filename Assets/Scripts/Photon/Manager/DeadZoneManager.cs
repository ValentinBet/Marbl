using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeadZoneManager : MonoBehaviour
{

    public GameObject fx_MarblDie;
    public float fxDieWidth = 1;
    bool DMactif = false;

    public void Start()
    {
        DMactif = PhotonNetwork.CurrentRoom.GetDeathmatch();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            AudioManager.Instance.PlayThisSound(AudioManager.Instance.ballDeath);

            GameObject _fx = Instantiate(fx_MarblDie, other.transform.position, Random.rotation);
            _fx.transform.localScale = new Vector3(fxDieWidth, fxDieWidth, fxDieWidth);
            Destroy(_fx, 2);

            DeathMatchManager.Instance.OnMarblDie(other.gameObject);

            QuickScoreboard.Instance.Refresh(); 
        }

        if(other.tag == "Egg")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                EventManager.Instance.canDrop = true;
            }

            AudioManager.Instance.PlayThisSound(AudioManager.Instance.ballDeath);

            GameObject _fx = Instantiate(fx_MarblDie, other.transform.position, Random.rotation);
            _fx.transform.localScale = new Vector3(fxDieWidth, fxDieWidth, fxDieWidth);
            Destroy(_fx, 2);


            if (GameModeManager.Instance.localPlayerTurn)
            {
                PhotonView pv = other.GetComponent<PhotonView>();

                pv.RequestOwnership();
                PhotonNetwork.Destroy(pv);
            }
        }

        if (other.tag == "Gift")
        {
            AudioManager.Instance.PlayThisSound(AudioManager.Instance.ballDeath);

            GameObject _fx = Instantiate(fx_MarblDie, other.transform.position, Random.rotation);
            _fx.transform.localScale = new Vector3(fxDieWidth, fxDieWidth, fxDieWidth);
            Destroy(_fx, 2);


            if (GameModeManager.Instance.localPlayerTurn)
            {
                PhotonView pv = other.GetComponent<PhotonView>();

                pv.RequestOwnership();
                PhotonNetwork.Destroy(pv);
            }
        }

    }
}
