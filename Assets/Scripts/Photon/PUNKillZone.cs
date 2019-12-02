using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUNKillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            
        }
    }
}
