using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEntryBlock : MonoBehaviour
{
    public PipeBlock mainPipe;

    public int entryNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (!mainPipe.ignoredBall.Contains(other.gameObject))
            {
                mainPipe.InitPipeTP(entryNumber, other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (mainPipe.ignoredBall.Contains(other.gameObject))
            {
                mainPipe.ignoredBall.Remove(other.gameObject);
            }
        }
    }
}
