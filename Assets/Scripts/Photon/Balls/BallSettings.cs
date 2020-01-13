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
    public bool isOnPipe = false;
    public float currentSpeed = 0;

    private Team lastTeam;

    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private PhotonView pv;

    private void Awake()
    {
        pv = this.GetComponent<PhotonView>();
    }
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
        currentSpeed = Mathf.Lerp(currentSpeed, myRigid.velocity.sqrMagnitude, 1 * Time.deltaTime);
    }

    public void FixedUpdate()
    {        
        if (!pv.IsMine)
        {
            myRigid.position = Vector3.MoveTowards(myRigid.position, networkPosition, Time.fixedDeltaTime);
            myRigid.rotation = Quaternion.RotateTowards(myRigid.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
        }
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
        if (this.gameObject == GameModeManager.Instance.localPlayerObj.GetComponent<PUNMouseControl>().actualSelectedBall)
        {
            if (myteam != lastTeam)
            {
                GameModeManager.Instance.localPlayerObj.GetComponent<PUNMouseControl>().DeselectBall();
            }
        }

        if (myteam != lastTeam)
        {
            SetColor();
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
        HueManager.Instance.OnCollisionBall(myteam, team);

        myteam = team;
        SetColor();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext((int)myteam);

            stream.SendNext(this.myRigid.position);
            stream.SendNext(this.myRigid.rotation);
            stream.SendNext(this.myRigid.velocity);
        }
        else
        {
            myteam = MarblGame.GetTeam((int)stream.ReceiveNext());
            QuickScoreboard.Instance.Refresh();

            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            myRigid.velocity = (Vector3)stream.ReceiveNext();
        }
    }




}
