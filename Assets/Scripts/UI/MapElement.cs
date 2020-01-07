using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapElement : MonoBehaviour
{
    public string nameMap;

    public Text NameMap;
    public Image myOutline;

    public bool isCustom = true;

    public void SetMap()
    {
        RoomScripts.Instance.SetMap(nameMap);
        RoomScripts.Instance.SetMapCustom(isCustom);

        GameObject[] allMap = GameObject.FindGameObjectsWithTag("Map");

        foreach(GameObject map in allMap)
        {
            if(map != gameObject)
            {
                map.GetComponent<MapElement>().myOutline.color = Color.white;
            }
        }

        myOutline.color = new Color(1, 0.1986281f, 0);
    }
}
