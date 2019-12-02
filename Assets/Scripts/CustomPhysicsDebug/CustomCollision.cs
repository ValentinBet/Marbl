using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollision : MonoBehaviour
{
    public BallMovements bm;
    private void OnTriggerEnter(Collider other)
    {
        bm.colSeq(other);
    }
}
