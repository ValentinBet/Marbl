﻿using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class BallSettings : MonoBehaviourPunCallbacks, IPunObservable
{
    public Team myteam;
    public ParticleSystem myTrail;

    public bool isVisible = true;

    private void Start()
    {
        SetColor();
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

    public void SetColor()
    {
        Material[] _mats = this.GetComponent<Renderer>().materials;
        _mats[1] = GameModeManager.Instance.colors[(int)myteam];
        this.GetComponent<Renderer>().materials = _mats;
    }

    public void ChangeTeam(Team team)
    {
        myteam = team;
        SetColor();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myteam);
        } else
        {
            myteam = (Team) stream.ReceiveNext();
            QuickScoreboard.Instance.Refresh();
        }
    }
}
