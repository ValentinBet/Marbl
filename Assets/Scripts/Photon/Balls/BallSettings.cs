﻿using Photon.Pun;
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
    private Color fxColor;
    private float fxChargePower;

    private Material[] _mats;

    public GameObject prefabParticule;

    private void Awake()
    {
        pv = this.GetComponent<PhotonView>();
    }
    private void Start()
    {
        SetColor();
        lastTeam = myteam;
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
            fxColor = new Color(0 + dragPowerPrctg, 1 - dragPowerPrctg, 0);
            chargeFxChildPS.startColor = fxColor;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 17 && myteam != Team.neutral)
        {
            print("sfdnyused");
            Transform parentObj = other.transform.parent;

            if (GameModeManager.Instance.localPlayerTurn)
            {
                if(GameModeManager.Instance.localPlayerTeam == myteam)
                {
                    ObjManager.Instance.GiveRandomObj();
                }
                else
                {
                    Player pGetGift = GetRandomPlayerOfTeam(myteam);
                    pGetGift.SetPlayerGetGift(true);
                }

                PhotonView myPv = parentObj.GetComponent<PhotonView>();

                if (!myPv.IsMine)
                {
                    myPv.RequestOwnership();
                }

                Destroy(Instantiate(prefabParticule, transform.position + Vector3.up * 2f, Random.rotation), 1);

                myPv.RPC("RpcSpawnParticule", RpcTarget.Others, transform.position + Vector3.up * 2);

                PhotonNetwork.Destroy(myPv);
            }
        }
    }

    [PunRPC]
    void RpcSpawnParticule(Vector3 pos)
    {
        Destroy(Instantiate(prefabParticule, pos, Random.rotation), 1);
    }

    Player GetRandomPlayerOfTeam(Team team)
    {
        List<Player> playersOfThisTeam = new List<Player>();

        foreach(Player p in PhotonNetwork.PlayerList)
        {
            if(p.GetTeam() == team)
            {
                playersOfThisTeam.Add(p);
            }
        }

        return playersOfThisTeam[Random.Range(0, playersOfThisTeam.Count)];
    }
}
