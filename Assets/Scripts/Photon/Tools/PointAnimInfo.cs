using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointAnimInfo : MonoBehaviour
{
    public Text myText;

    public void SetColor(Color color, int value)
    {
        myText.color = color;
        myText.text = value.ToString();

        StartCoroutine(DestroyObj());
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(GetComponent<PhotonView>());
    }
}
