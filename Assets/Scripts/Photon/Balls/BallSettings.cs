using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class BallSettings : MonoBehaviourPunCallbacks, IPunObservable
{
    public Team myteam;
    public ParticleSystem myTrail;
    public GameObject chargeFx;
    public ParticleSystem chargeFxChildPS;
    public ParticleSystem fx_Overcharged;
    public bool isVisible = true;
    public Rigidbody myRigid;
    public bool isOnPipe = false;
    public float currentSpeed = 0;
    public float fxStartSpeed = -5.5f;

    private Team lastTeam;

    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private PhotonView pv;
    private float fxChargePower;

    public bool isPowered = false;

    private Material[] _mats;

    public GameObject circlePowered;

    public Gradient chargeColorGradient;

    public float radiusExplo;

    public PingElement myPingElement;
    public ShockwaveCircle shockWaveScript;

    public AudioClip shockwaveSound;

    private void Awake()
    {
        pv = this.GetComponent<PhotonView>();
    }
    private void Start()
    {
        SetColor();
        lastTeam = myteam;
        shockWaveScript.SetColor(myteam);
    }

    private void Update()
    {
        CheckTeam();
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

    public void InitChargeFx()
    {
        chargeFx.transform.localScale = Vector3.zero;
        chargeFx.SetActive(true);
    }

    public void UpdateChargeFx(float dragForce, float dragForceMaxValue)
    {
        if (chargeFx != null)
        {
            float dragPowerPrctg = ((dragForce * 100) / dragForceMaxValue) / 100;

            fxChargePower = Mathf.Lerp(fxChargePower, dragPowerPrctg, Time.deltaTime * 4);
            chargeFx.transform.localScale = new Vector3(fxChargePower, fxChargePower, fxChargePower);

            chargeFxChildPS.startColor = chargeColorGradient.Evaluate(dragPowerPrctg);
            chargeFxChildPS.startSpeed = fxStartSpeed - (dragPowerPrctg * 2);
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
            shockWaveScript.SetColor(myteam);
        }

    }
    public void SetColor()
    {
        //color ball
        _mats = this.GetComponent<Renderer>().materials;
        _mats[0] = GameModeManager.Instance.colorsMat[(int)myteam];
        this.GetComponent<Renderer>().materials = _mats;


        //color trail
        myTrail.startColor = MarblGame.GetColor((int)myteam);


        myPingElement.SetColor(MarblGame.GetColor((int)myteam));
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

    public void ChangePowered(bool value)
    {
        pv.RPC("RpcSetPowered", RpcTarget.All, value);
    }

    [PunRPC]
    void RpcSetPowered(bool value)
    {
        isPowered = value;
        circlePowered.SetActive(value);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPowered && GameModeManager.Instance.localPlayerTurn)
        {
            isPowered = false;
            pv.RPC("RpcSetPowered", RpcTarget.All, false);
            ShockWave();
        }
    }

    public GameObject shockwaveFx;

    void ShockWave()
    {
        pv.RPC("RpcShockwaveExplo", RpcTarget.All);

        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radiusExplo);

        foreach (Collider co in colliders)
        {
            if (co.GetComponent<Rigidbody>() != null && co.GetComponent<PhotonView>() != null)
            {
                if (co.GetComponent<PhotonView>().IsMine)
                {
                    if(co.gameObject != gameObject)
                    {
                        co.GetComponent<Rigidbody>().AddExplosionForce(0.2f, this.transform.position, radiusExplo, 0f, ForceMode.Impulse);

                        if (co.gameObject.CompareTag("Ball") && PhotonNetwork.CurrentRoom.GetHue())
                        {
                            co.GetComponent<BallSettings>().ChangeTeam(myteam);
                        }
                    }
                }
            }
        }
    }

    [PunRPC]
    void RpcShockwaveExplo()
    {
        GameModeManager.Instance.localPlayerObj.GetComponent<CameraPlayer>().InitShakeScreen(50 * 0.5f, 0.2f);
        Instantiate(shockwaveFx, this.transform.position, this.transform.rotation);
        AudioManager.Instance.PlaySoundAtPoint(shockwaveSound, transform.position);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, radiusExplo);
    }
}
