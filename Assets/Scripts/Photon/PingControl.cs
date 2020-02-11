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
    private PingChoiceSettings choiceSettings;
    private float offsetAxis;
    private bool isActive;
    private int pingChosen;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        PingChoice = UIManager.Instance.PingChoice;
        choiceSettings = PingChoice.GetComponent<PingChoiceSettings>();
    }
    void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.Ping) && !isActive && !ChatManager.Instance.ChatUI.myInputField.isFocused)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                PingChoice.transform.position = Input.mousePosition;
                PingChoice.SetActive(true);
                isActive = true;
            }
        }

        if (Input.GetKey(InputManager.Instance.Inputs.inputs.Ping) && isActive)
        {
            PingChoiceHold();
        }
        if (Input.GetKeyUp(InputManager.Instance.Inputs.inputs.Ping) && isActive)
        {
            if (pingChosen != 10)
            {
                InitSpawnPing(pingChosen);
            }
            pingChosen = 10;
            offsetAxis = 0;
            ResetCornerColor();
            PingChoice.SetActive(false);
            isActive = false;
        }
    }

    private void PingChoiceHold()
    {
        offsetAxis += Input.GetAxis("Mouse X");

        if (!choiceSettings.onEnterCenter)
        {
            if (offsetAxis > 0)
            {
                pingChosen = 1;
                choiceSettings.rightCornerImage.color = choiceSettings.HighlightedCornerColor;
                choiceSettings.leftCornerImage.color = Color.clear;
            }
            else if (offsetAxis < 0)
            {
                pingChosen = 0;
                choiceSettings.leftCornerImage.color = choiceSettings.HighlightedCornerColor;
                choiceSettings.rightCornerImage.color = Color.clear;
            }
        }
        else
        {
            pingChosen = 10;
            ResetCornerColor();
        }

    }

    private void ResetCornerColor()
    {
        choiceSettings.rightCornerImage.color = Color.clear;
        choiceSettings.leftCornerImage.color = Color.clear;
    }

    private void InitSpawnPing(int ping)
    {
        Quaternion rota = Quaternion.LookRotation(hit.normal);
        //rota *= Quaternion.Euler(0, 0, 0);

        SpawnPingLocal(ping, hit.point, rota, PhotonNetwork.LocalPlayer.GetTeam()); // Spawn le ping en local

        pv.RPC("RpcSpawnPing", RpcTarget.Others, ping, hit.point, rota, PhotonNetwork.LocalPlayer.GetTeam()); // Spawn le ping sur les autres joueurs
    }

    [PunRPC]
    private void RpcSpawnPing(int ping, Vector3 pos, Quaternion rota, Team team)
    {
        SpawnPingLocal(ping, pos, rota, team);
    }

    private void SpawnPingLocal(int ping, Vector3 pos, Quaternion rota, Team team)
    {
        GameObject _ping = null;

        switch (ping)
        {
            case 0:
                _ping = Instantiate(BasicPing, pos, rota);
                break;
            case 1:
                _ping = Instantiate(QuestionMarkPing, pos, rota);
                break;
            default:
                Debug.Log("Erreur, Ping non reconnu");
                return;
        }

        //Pour le sortir du sol
        _ping.transform.position += _ping.transform.forward * 0.1f;

        if (_ping.GetComponent<PingSettings>() != null)
        {
            _ping.GetComponent<PingSettings>().mainSprite.color = MarblGame.GetColor((int)team);
            _ping.GetComponent<PingSettings>().pingSprite.color = MarblGame.GetColor((int)team);
        }

        Destroy(_ping, 1.5f);
    }
}
