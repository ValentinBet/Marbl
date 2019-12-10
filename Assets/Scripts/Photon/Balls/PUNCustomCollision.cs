using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUNCustomCollision : MonoBehaviour
{
    public PUNBallMovement bm;

    private void OnTriggerEnter(Collider other)
    {
        bm.ColSeq(other);
    }
}

