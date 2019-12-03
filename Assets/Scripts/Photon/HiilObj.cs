using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillObj : MonoBehaviour
{

    public List<BallSettings> ballInside = new List<BallSettings>();

    public List<SpriteRenderer> imgWall = new List<SpriteRenderer>();


    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        if(other.transform.tag == "Ball")
        {
            ballInside.Add(other.gameObject.GetComponent<BallSettings>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        if (other.transform.tag == "Ball")
        {
            ballInside.Remove(other.gameObject.GetComponent<BallSettings>());
        }
    }


    public void SetColor(Color newColor)
    {
        foreach(SpriteRenderer img in imgWall)
        {
            img.color = newColor;
        }
    }
}
