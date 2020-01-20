using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapElement : MonoBehaviour
{
    public string nameMap;

    public Text NameMap;
    public Image myOutline;

    public void SetMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RoomScripts.Instance.SetMap(nameMap);
        }

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
