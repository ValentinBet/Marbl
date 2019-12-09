using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSettings : MonoBehaviourPunCallbacks, IPunObservable
{
    public PunTeams.Team myteam;
    public ParticleSystem myTrail;

    public bool isVisible = true;

    private void Start()
    {
        Material[] _mats = this.GetComponent<Renderer>().materials;
        _mats[1] = GameModeManager.Instance.colors[(int)myteam];
        this.GetComponent<Renderer>().materials = _mats;
        myTrail.startColor = MarblGame.GetColor((int)myteam);
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myteam);
        } else
        {
            myteam = (PunTeams.Team) stream.ReceiveNext();
        }
    }
}
