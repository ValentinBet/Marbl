using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MapTools : EditorWindow
{
    [SerializeField]
    string mapName = "NewMap";

    GameObject mapObj;
    GameObject fixedSpawnPos;
    GameObject randomSpawnPos;
    GameObject hillPos;
    GameObject hueNeutralBallPos;

    TextAsset mapToLoad;

    List<GameObject> ObjectInScene = new List<GameObject>();
    Map map = new Map();

    TextAsset jsonMap;

    [MenuItem("Tools/Windows/Map Editor")]
    public static void OpenThisWindow()
    {
        MapTools editorWindow = EditorWindow.GetWindow(typeof(MapTools)) as MapTools;
        editorWindow.Init();
    }

    public void Init()
    {
        Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Save Actual Scene");
        GUILayout.Space(16);
        mapName = EditorGUILayout.TextField("Map Name", mapName);
        GUILayout.Space(16);
        EditorGUILayout.LabelField("Add root parents");

        mapObj = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Add Map Objects"), mapObj, typeof(GameObject), true);
        fixedSpawnPos = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Add Fixed Spawn"), fixedSpawnPos, typeof(GameObject), true);
        randomSpawnPos = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Add Random Spawn"), randomSpawnPos, typeof(GameObject), true);
        hillPos = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Add Hill Pos"), hillPos, typeof(GameObject), true);
        hueNeutralBallPos = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Add Neutral Ball Pos"), hueNeutralBallPos, typeof(GameObject), true);

        GUILayout.Space(16);
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        ObjectInScene = allObjects.OfType<GameObject>().ToList();

        if (fixedSpawnPos == null || randomSpawnPos == null)
        {
            EditorGUILayout.HelpBox("Ajouter les GO parents concernant les spawns ", MessageType.Info);
        }
        else
        {
            if (GUILayout.Button("Save actual scene as JSON"))
            {
                string _ToJson = SaveMap();

                File.WriteAllText(Application.streamingAssetsPath + "/MapsCustom/" + mapName + ".json", _ToJson);
                AssetDatabase.Refresh();
            }
        }

    }

    public string SaveMap()
    {
        if(map.mapObjects != null)
        {
            map.mapObjects.Clear();
        }

        map.mapObjectsPosition = mapObj.transform.position;
        map.mapObjectsRotation = mapObj.transform.rotation;
        map.mapObjectsScale = mapObj.transform.localScale;

        map.randomSpawnPosition = randomSpawnPos.transform.position;
        map.randomSpawnRotation = randomSpawnPos.transform.rotation;
        map.randomSpawnScale = randomSpawnPos.transform.localScale;

        map.fixedSpawnPosition = fixedSpawnPos.transform.position;
        map.fixedSpawnRotation = fixedSpawnPos.transform.rotation;
        map.fixedSpawnScale = fixedSpawnPos.transform.localScale;

        map.neutralSpawnPosition = hueNeutralBallPos.transform.position;
        map.neutralSpawnRotation = hueNeutralBallPos.transform.rotation;
        map.neutralSpawnScale = hueNeutralBallPos.transform.localScale;

        foreach (GameObject go in ObjectInScene)
        {
            MapObject _object = new MapObject();

            if (go.transform.root.gameObject == go || (go.name == "Team1") || (go.name == "Team2") || (go.name == "Team3") || (go.name == "Team4"))
            {
                _object.isRootParent = true;

                switch (go.name)
                {
                    case "Team1":
                        map.team1Scale = go.transform.localScale;
                        break;
                    case "Team2":
                        map.team2Scale = go.transform.localScale;
                        break;
                    case "Team3":
                        map.team3Scale = go.transform.localScale;
                        break;
                    case "Team4":
                        map.team4Scale = go.transform.localScale;
                        break;

                    default:
                        break;
                }
            }

            if ((go != fixedSpawnPos) && (go != mapObj) && (go != randomSpawnPos))
            {
                if (go.transform.root.name == fixedSpawnPos.name && (go != fixedSpawnPos))
                {
                    if (go.transform.parent.name != fixedSpawnPos.gameObject.name)
                    {
                        _object.teamNumber = go.transform.parent.name;
                    }
                }

                _object.rootParent = go.transform.root.name;
                _object.objectName = go.name;

                if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(go) != null)
                {
                    _object.prefabName = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go).name;
                }

                _object.position = go.transform.position;
                _object.rotation = go.transform.rotation;
                _object.scale = go.transform.localScale;

                if (_object == null)
                {
                    Debug.Log(go.name);
                }
                Debug.Log(map.mapObjects);
                    map.mapObjects.Add(_object); 
            } 
        }

        map.mapName = mapName;

        return JsonUtility.ToJson(map, true);
    }

}




