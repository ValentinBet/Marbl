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

    bool canMove = false;
    bool canRestartCoroutine = true;

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
        if (pv.IsMine && canMove)
        {
            Vector3 fwd = transform.parent.TransformDirection(Vector3.forward);
            Vector3 down = transform.parent.TransformDirection(Vector3.down);
            Debug.DrawRay(transform.parent.position, (fwd + down * 0.5f) * 0.9f, Color.green);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, (fwd + down * 0.5f), out hit, 0.9f))
            {
                if(hit.collider.gameObject.layer == 10)
                {
                    transform.parent.Translate(transform.forward * Time.deltaTime * 0.25f, Space.World);
                    myAnimator.SetBool("IsMoving", true);
                }
                else
                {
                    canMove = false;
                    myAnimator.SetBool("IsMoving", false);
                }
            }
            else
            {
                canMove = false;
                myAnimator.SetBool("IsMoving", false);
            }
        }

        if(!canMove && canRestartCoroutine)
        {
            canRestartCoroutine = false;
            StartCoroutine(Rotate());
        }
    }

    IEnumerator Rotate()
    {
        float randomRota = Random.Range(0, 360);

        yield return new WaitForSeconds(1f);

        transform.parent.eulerAngles = new Vector3(0, randomRota, 0);

        yield return new WaitForSeconds(1f);

        canMove = true;
        canRestartCoroutine = true;
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
