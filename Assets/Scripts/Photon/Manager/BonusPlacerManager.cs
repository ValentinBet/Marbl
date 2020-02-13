using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjManager;
using static Photon.Pun.UtilityScripts.PunTeams;

public class BonusPlacerManager : MonoBehaviour
{
    List<ObjToSpawn> allObjToSpawn = new List<ObjToSpawn>();

    public GameObject textNamePrefab;

    public PhotonView pv;

    private static BonusPlacerManager _instance;
    public static BonusPlacerManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            enabled = false;
        }
    }

    public void AddObject(string _name, Team _team, Vector3 _position, Quaternion _rotation, string _obj)
    {
        ObjToSpawn newObj = new ObjToSpawn(_name, _team, _position, _rotation, _obj);
        allObjToSpawn.Add(newObj);
    }

    public void SpawnAllObjects()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        if (!PhotonNetwork.CurrentRoom.GetForceCam() && allObjToSpawn.Count > 0) {
            PhotonNetwork.CurrentRoom.SetForceCam(true);
        }

        foreach (ObjToSpawn element in allObjToSpawn)
        {
            PhotonNetwork.Instantiate(element.obj, element.postion, element.rotation);
            //pv.RPC("RpcSpawnNamePlayer", RpcTarget.All, element.playerName, element.playerTeam, element.postion);

            yield return new WaitForSeconds(1.5f);
        }

        allObjToSpawn.Clear();

        PhotonNetwork.CurrentRoom.SetForceCam(false);
    }

    [PunRPC]
    void RpcSpawnNamePlayer(string _playerName, Team _playerTeam, Vector3 _position)
    {
        Destroy(Instantiate(textNamePrefab, _position, Quaternion.identity), 2);
    }
}

public class ObjToSpawn
{
    public string playerName;
    public Team playerTeam;
    public Vector3 postion;
    public Quaternion rotation;
    public string obj;

    public ObjToSpawn(string _name, Team _team, Vector3 _position, Quaternion _rotation, string _obj)
    {
        playerName = _name;
        playerTeam = _team;
        postion = _position;
        rotation = _rotation;
        obj = _obj;
    }
}
