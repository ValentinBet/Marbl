using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSettings : MonoBehaviour
{
    public PunTeams.Team myteam;
    public ParticleSystem myTrail;

    private void Start()
    {
        myTrail.startColor = MarblGame.GetColor((int) myteam);
    }
}
