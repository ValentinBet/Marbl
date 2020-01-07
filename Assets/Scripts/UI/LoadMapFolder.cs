using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadMapFolder : MonoBehaviour
{
    public GameObject prefabMap;

    public Transform parent;

    void Start()
    {
        var info = new DirectoryInfo(Application.streamingAssetsPath + "/MapsCustom/");
        var fileInfo = info.GetFiles();

        int i = 0;

        foreach (FileInfo file in fileInfo)
        {
            if (file.Name.Contains(".meta")){ continue; }

            GameObject newMap = Instantiate(prefabMap, parent);
            newMap.GetComponent<MapElement>().NameMap.text = (file.Name).Replace(".json", "");
            newMap.GetComponent<MapElement>().nameMap = file.Name;
            i++;
        }
    }
}
