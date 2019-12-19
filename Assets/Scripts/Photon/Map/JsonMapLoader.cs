using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun.UtilityScripts;
using System.Linq;

public class JsonMapLoader : MonoBehaviour
{
    private string jsonMap;

    [Space(16)]
    public GameObject mapObj;
    public GameObject fixedSpawnPos;
    public GameObject randomSpawnPos;
    public GameObject hillPos;
    public GameObject hueNeutralBallPos;

    [Space(16)]
    public GameObject team1;
    public GameObject team2;
    public GameObject team3;
    public GameObject team4;

    private List<MapObject> fixSpawn = new List<MapObject>();
    private PhotonView myPV;

    int mapIndex;

    private static JsonMapLoader _instance;
    public static JsonMapLoader Instance { get { return _instance; } }

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
        myPV = GetComponent<PhotonView>();
    }


    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            mapIndex = PhotonNetwork.CurrentRoom.GetMap();

            string path = "";
            switch (PhotonNetwork.CurrentRoom.GetCustomMap())
            {
                case true:
                    path = "/MapsCustom/";
                    break;

                case false:
                    path = "/Maps/";
                    break;
            }


            var info = new DirectoryInfo(Application.streamingAssetsPath + path);
            var files = info.GetFiles();

            List<FileInfo> fileInfo = files.ToList();

            for (int i = 0; i < fileInfo.Count; i++)
            {
                if (fileInfo[i].Name.Contains(".meta"))
                {
                    fileInfo.RemoveAt(i);
                }
            }  
            
            WWW data = new WWW(Application.streamingAssetsPath + path + fileInfo[mapIndex].Name);

            jsonMap = data.text;

            JsonMapLoader.Instance.LoadMap(jsonMap);
            myPV.RPC("RpcSendMapToAllClient", RpcTarget.Others, jsonMap);
        }
    }

    [PunRPC]
    public void RpcSendMapToAllClient(string map)
    {
        JsonMapLoader.Instance.LoadMap(map);
    }

    
    public void LoadMap(string value)
    {
        Map LoadedMap = new Map();

        LoadedMap = JsonUtility.FromJson<Map>(value);

        mapObj.transform.localScale = LoadedMap.mapObjectsScale;
        mapObj.transform.position = LoadedMap.mapObjectsPosition;
        mapObj.transform.rotation = LoadedMap.mapObjectsRotation;

        fixedSpawnPos.transform.localScale = LoadedMap.fixedSpawnScale;
        fixedSpawnPos.transform.position = LoadedMap.fixedSpawnPosition;
        fixedSpawnPos.transform.rotation = LoadedMap.fixedSpawnRotation;

        randomSpawnPos.transform.localScale = LoadedMap.randomSpawnScale;
        randomSpawnPos.transform.position = LoadedMap.randomSpawnPosition;
        randomSpawnPos.transform.rotation = LoadedMap.randomSpawnRotation;

        if (hueNeutralBallPos != null)
        {
            hueNeutralBallPos.transform.localScale = LoadedMap.neutralSpawnScale;
            hueNeutralBallPos.transform.position = LoadedMap.neutralSpawnPosition;
            hueNeutralBallPos.transform.rotation = LoadedMap.neutralSpawnRotation;
        }

        team1.transform.localScale = LoadedMap.team1Scale;
        team2.transform.localScale = LoadedMap.team2Scale;
        team3.transform.localScale = LoadedMap.team3Scale;
        team4.transform.localScale = LoadedMap.team4Scale;

        foreach (MapObject mapObject in LoadedMap.mapObjects)
        {
            GameObject obj = null;
            GameObject objInstance = null;

            if (!mapObject.isRootParent)
            {
                if (mapObject.rootParent == mapObj.name)
                {
                    if (mapObject.prefabName != null && mapObject.prefabName != "")
                    {
                        obj = Resources.Load("MapLoad/" + mapObject.prefabName) as GameObject;

                        if (obj != null)
                        {
                            objInstance = Instantiate(obj, mapObj.transform);
                        }
                    }
                }
                else if (mapObject.rootParent == fixedSpawnPos.name)
                {
                    fixSpawn.Add(mapObject);
                }
                else if (mapObject.rootParent == randomSpawnPos.name)
                {
                    objInstance = new GameObject();
                    objInstance.transform.parent = randomSpawnPos.transform;
                }
                else if (mapObject.rootParent == "DeadZone")
                {
                    objInstance = new GameObject();
                    objInstance.name = mapObject.objectName;
                }
                else if (mapObject.rootParent == hueNeutralBallPos.name)
                {
                    objInstance = new GameObject();
                    objInstance.transform.parent = hueNeutralBallPos.transform;
                }
                else if (mapObject.rootParent == "HillsPos")
                {
                    objInstance = new GameObject();
                    objInstance.name = mapObject.objectName;

                    objInstance.transform.parent = hillPos.transform;
                }

            } else
            {
                switch (mapObject.objectName)
                {
                    case "Team1":
                        team1.transform.position = mapObject.position;
                        team1.transform.rotation = mapObject.rotation;
                        break;
                    case "Team2":
                        team2.transform.position = mapObject.position;
                        team2.transform.rotation = mapObject.rotation;
                        team2.transform.localScale = mapObject.scale;
                        break;
                    case "Team3":
                        team3.transform.position = mapObject.position;
                        team3.transform.rotation = mapObject.rotation;
                        break;
                    case "Team4":
                        team4.transform.position = mapObject.position;
                        team4.transform.rotation = mapObject.rotation;
                        break;
                    default:
                        break;
                }
            }

            if (objInstance != null)
            {
                objInstance.transform.localScale = mapObject.scale;
                objInstance.transform.position = mapObject.position;
                objInstance.transform.rotation = mapObject.rotation;
                objInstance.name = mapObject.objectName;
            }

        }

        LoadSpawns();


        PhotonNetwork.LocalPlayer.SetPlayerMapState(true);
    }

    private void LoadSpawns()
    {
        foreach (MapObject spawn in fixSpawn)
        {
            GameObject spawnGO = new GameObject();

            switch (spawn.teamNumber)
            {
                case "Team1":
                    spawnGO.transform.parent = team1.transform;
                    break;
                case "Team2":
                    spawnGO.transform.parent = team2.transform;
                    break;
                case "Team3":
                    spawnGO.transform.parent = team3.transform;
                    break;
                case "Team4":
                    spawnGO.transform.parent = team4.transform;
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }

            spawnGO.transform.localScale = spawn.scale;
            spawnGO.transform.position = spawn.position;
            spawnGO.transform.rotation = spawn.rotation;
            spawnGO.name = spawn.objectName;
        }
    }
}
