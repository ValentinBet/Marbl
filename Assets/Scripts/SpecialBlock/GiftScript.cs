using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.SetFollowObj(gameObject);
    }
}
