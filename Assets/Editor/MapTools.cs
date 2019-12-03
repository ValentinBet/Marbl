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

    TextAsset mapToLoad;

    List<GameObject> ObjectInScene = new List<GameObject>();
    Map map = new Map();

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

                File.WriteAllText(Application.dataPath + "/Map/" + mapName + ".json", _ToJson);
                AssetDatabase.Refresh();
            }
        }
    }

    public string SaveMap()
    {
        map.mapObjects.Clear();

        foreach (GameObject go in ObjectInScene)
        {
            MapObject _object = new MapObject();

            if (go.transform.root == fixedSpawnPos && (go != fixedSpawnPos))
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

            map.mapObjects.Add(_object);
        }

        map.mapName = mapName;

        return JsonUtility.ToJson(map, true);
    }
}

[Serializable]
public class Map
{
    public string mapName;

    public List<MapObject> mapObjects = new List<MapObject>();
}

[Serializable]
public class MapObject
{
    public string objectName;
    public string prefabName;
    public string rootParent;
    public string teamNumber;

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}


