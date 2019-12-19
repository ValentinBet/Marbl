﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEntryBlock : MonoBehaviour
{
    public PipeBlock mainPipe;
    public GameObject exitDirection;
    public int entryNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (!mainPipe.ignoredBall.ContainsKey(other.gameObject))
            {
                mainPipe.InitPipeTP(entryNumber, other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (mainPipe.ignoredBall.ContainsKey(other.gameObject))
            {
                if (mainPipe.ignoredBall[other.gameObject].entryGo.gameObject != gameObject)
                {
                    mainPipe.ignoredBall.Remove(other.gameObject);
                }
            }
        }
    }

    public Vector3 GetExitDirectionVector()
    {
        return exitDirection.transform.position - this.transform.position;
    }
}
