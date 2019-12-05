using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonMapLoader : MonoBehaviour
{
    public TextAsset jsonMap;

    [Space(16)]
    public GameObject mapObj;
    public GameObject fixedSpawnPos;
    public GameObject randomSpawnPos;

    [Space(16)]
    public GameObject team1;
    public GameObject team2;
    public GameObject team3;
    public GameObject team4;

    private void Start()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        Map LoadedMap = new Map();

        LoadedMap = JsonUtility.FromJson<Map>(jsonMap.text);

        mapObj.transform.localScale = LoadedMap.mapObjectsScale;

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
                        objInstance = Instantiate(obj, mapObj.transform);
                    }
                }
                else if (mapObject.rootParent == fixedSpawnPos.name)
                {
                    objInstance = new GameObject();
                    switch (mapObject.teamNumber)
                    {
                        case "Team1":
                            objInstance.transform.parent = team1.transform;
                            break;
                        case "Team2":
                            objInstance.transform.parent = team2.transform;
                            break;
                        case "Team3":
                            objInstance.transform.parent = team3.transform;
                            break;
                        case "Team4":
                            objInstance.transform.parent = team4.transform;
                            break;
                        default:
                            Debug.Log("Error");
                            break;
                    }
                }
                else if (mapObject.rootParent == randomSpawnPos.name)
                {
                    objInstance = new GameObject();

                    objInstance.transform.parent = randomSpawnPos.transform;
                }
                else if (mapObject.rootParent == "PosDeadZone")
                {
                    objInstance = new GameObject();
                    objInstance.name = mapObject.objectName;
                }

            } else
            {
                switch (mapObject.objectName)
                {
                    case "Team1":
                        team1.transform.position = mapObject.position;
                        team1.transform.rotation = mapObject.rotation;
                        team1.transform.localScale = mapObject.scale;
                        break;
                    case "Team2":
                        team2.transform.position = mapObject.position;
                        team2.transform.rotation = mapObject.rotation;
                        team2.transform.localScale = mapObject.scale;
                        break;
                    case "Team3":
                        team3.transform.position = mapObject.position;
                        team3.transform.rotation = mapObject.rotation;
                        team3.transform.localScale = mapObject.scale;
                        break;
                    case "Team4":
                        team4.transform.position = mapObject.position;
                        team4.transform.rotation = mapObject.rotation;
                        team4.transform.localScale = mapObject.scale;
                        break;
                    default:
                        Debug.Log("Error");
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
    }
}
