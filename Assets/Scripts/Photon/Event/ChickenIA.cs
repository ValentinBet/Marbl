using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class ChickenIA : MonoBehaviour
{
    public Animator myAnimator;
    public PhotonView pv;

    public GameObject EggParticule;

    public GameObject prefabParticule;

    private void Start()
    {
        Destroy(Instantiate(EggParticule, transform.position, Random.rotation), 2);

        if (!PhotonNetwork.IsMasterClient)
        {
            this.enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (pv.IsMine)
        {
            Vector3 fwd = transform.parent.TransformDirection(Vector3.forward);
            Vector3 down = transform.parent.TransformDirection(Vector3.down);
            Debug.DrawRay(transform.parent.position, fwd * 0.5f + down * 0.5f, Color.green);
            RaycastHit hit;

            if (!Physics.Raycast(transform.position, fwd, out hit, 0.5f))
            {
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, fwd * 0.5f + down * 0.5f, 0.3f * Time.deltaTime);
                myAnimator.SetBool("IsMoving", true);
            }
            else
            {
                myAnimator.SetBool("IsMoving", false);
            }
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            Destroy(Instantiate(prefabParticule, transform.position + Vector3.up * 0.9f, Random.rotation), 1);

            Team myTeam = GameModeManager.Instance.localPlayerTeam;

            if (GameModeManager.Instance.localPlayerTurn)
            {
                if (GameModeManager.Instance.localPlayerTeam == myTeam)
                {
                    ObjManager.Instance.GiveRandomObj();
                }
                else
                {
                    Player pGetGift = GetRandomPlayerOfTeam(myTeam);
                    pGetGift.SetPlayerGetGift(true);
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(pv);
            }
        }
    }

    Player GetRandomPlayerOfTeam(Team team)
    {
        List<Player> playersOfThisTeam = new List<Player>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p.GetTeam() == team)
            {
                playersOfThisTeam.Add(p);
            }
        }

        return playersOfThisTeam[Random.Range(0, playersOfThisTeam.Count)];
    }
}
