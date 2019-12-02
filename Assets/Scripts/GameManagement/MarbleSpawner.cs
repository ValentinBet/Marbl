using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleSpawner : MonoBehaviour
{
    [SerializeField] private List<Vector3> player1Pos;
    [SerializeField] private List<Vector3> player2Pos;
    [SerializeField] private List<Vector3> player3Pos;
    [SerializeField] private List<Vector3> player4Pos;

    private List<Transform> p1T;
    private List<Transform> p2T;
    private List<Transform> p3T;
    private List<Transform> p4T;

    [SerializeField] private List<GameObject> playerMarbles;
    [SerializeField] private List<Transform> teamParents;

    [SerializeField] private CameraAverage ca;

    void Awake()
    {
        p1T = SpawnMarbles(player1Pos,0);
        p2T = SpawnMarbles(player2Pos,1);
        p3T = SpawnMarbles(player3Pos,2);
        p4T = SpawnMarbles(player4Pos,3);
    }

    void Start()
    {
        ca.AssignPlayerMarbles(p1T, 0);
        ca.AssignPlayerMarbles(p2T, 1);
        if (GameData.playerAmount > 2)
            ca.AssignPlayerMarbles(p3T, 2);
        if (GameData.playerAmount > 3)
            ca.AssignPlayerMarbles(p4T, 3);
    }

    List<Transform> SpawnMarbles(List<Vector3> positions,int pIndex)
    {
        if (pIndex < GameData.playerAmount)
        {
            List<Transform> tempTrans = new List<Transform>();
            if (positions != null)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    if (i == GameData.actualProfile.GetProperty("MarblePerPlayer"))
                        break;
                    GameObject go = Instantiate(playerMarbles[pIndex], positions[i], Quaternion.identity, teamParents[pIndex]);
                    go.GetComponent<DirtyID>().myID = i;
                    tempTrans.Add(go.transform);
                }
            }
            return tempTrans;
        }
        else
        {
            return null;
        }
    }
}
