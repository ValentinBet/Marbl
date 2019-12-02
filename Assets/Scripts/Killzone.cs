using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ScoreManager.LowerScore(other.GetComponent<TurnBasedItem>().GetID());
            CameraAverage.RemoveMarble(other.GetComponent<TurnBasedItem>().GetID(), other.GetComponent<DirtyID>().myID);
            Destroy(other.gameObject);
        }
    }
}
