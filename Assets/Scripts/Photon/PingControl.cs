using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class PingControl : MonoBehaviour
{
    public GameObject PingChoice;

    public GameObject BasicPing;
    public GameObject QuestionMarkPing;

    private Ray ray;
    private RaycastHit hit;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.Ping))
        {
            DisplayPingChoice();
        }
    }

    private void DisplayPingChoice()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            SpawnPingLocal(1, hit.point, PhotonNetwork.LocalPlayer.GetTeam()); // Spawn le ping en local

            pv.RPC("SpawnPing", RpcTarget.Others, 1, hit.point, PhotonNetwork.LocalPlayer.GetTeam()); // Spawn le ping sur les autres joueurs
        }
    }

    [PunRPC]
    private void RpcSpawnPing(int ping, Vector3 pos, Team team)
    {
        SpawnPingLocal(ping, pos, team);
    }

    private void SpawnPingLocal(int ping, Vector3 pos, Team team)
    {
        GameObject _ping = null;

        switch (ping)
        {
            case 0:
                _ping = Instantiate(BasicPing, pos + new Vector3(0, 0.1f, 0), Quaternion.identity);
                break;
            case 1:
                _ping = Instantiate(QuestionMarkPing, pos + new Vector3(0, 0.1f, 0), Quaternion.identity);
                break;
            default:
                Debug.Log("Erreur, Ping non reconnu");
                return;
        }

        if (_ping.GetComponent<PingSettings>() != null)
        {
            _ping.GetComponent<PingSettings>().mainSprite.color = MarblGame.GetColor((int)team);
            _ping.GetComponent<PingSettings>().pingSprite.color = MarblGame.GetColor((int)team);
        }

        Destroy(_ping, 1.5f);
    }
}
