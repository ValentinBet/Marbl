using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class BallSettings : MonoBehaviourPunCallbacks, IPunObservable
{
    public Team myteam;
    public ParticleSystem myTrail;
    public ParticleSystem fx_Overcharged;

    public bool isVisible = true;
    public Rigidbody myRigid;

    public float currentSpeed = 0;

    private Team lastTeam;

    private void Start()
    {
        SetColor();
        myTrail.startColor = MarblGame.GetColor((int)myteam);

        lastTeam = myteam;
    }

    private void Update()
    {
        CheckTeam();
        SetColor();
        currentSpeed = Mathf.Lerp(currentSpeed ,myRigid.velocity.sqrMagnitude, 1 * Time.deltaTime);
    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }

    public void CheckTeam()
    {
        if (myteam != lastTeam)
        {
            SetColor();

            GameModeManager.Instance.localPlayerObj.GetComponent<PUNMouseControl>().DeselectBall();

            lastTeam = myteam;
        }
    }
    public void SetColor()
    {
        Material[] _mats = this.GetComponent<Renderer>().materials;
        _mats[1] = GameModeManager.Instance.colors[(int)myteam];
        this.GetComponent<Renderer>().materials = _mats;

        myTrail.startColor = MarblGame.GetColor((int)myteam);

    }

    public void SpawnOverchargedFx()
    {
        if (fx_Overcharged != null)
        {
            ParticleSystem _temp = Instantiate(fx_Overcharged, this.transform.position, this.transform.rotation);
            _temp.transform.parent = this.transform;
            Destroy(_temp, 2);
        }
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
        }
        else
        {
            myteam = (Team)stream.ReceiveNext();
            QuickScoreboard.Instance.Refresh();
        }
    }
}
