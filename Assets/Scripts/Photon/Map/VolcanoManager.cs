using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class VolcanoManager : MonoBehaviour
{
    public List<GameObject> rockList = new List<GameObject>();
    public Transform spawnPoints;

    public GameObject actualRock;

    public void LaunchRock()
    {        
        PhotonNetwork.CurrentRoom.SetForceCam(true);
        EventManager.Instance.SetFollowObj(actualRock);
    }

    [PunRPC]
     void SpawnRocks()
    {
        actualRock = Instantiate(rockList[Random.Range(0, rockList.Count)], spawnPoints.transform.position, Quaternion.identity);
    }

    public void EndRockLaunch()
    {

    }
}
