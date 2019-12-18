using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBlock : MonoBehaviour
{
    public float timeToTeleport;
    public List<GameObject> OrderedPipeEntryList = new List<GameObject>();
    public List<GameObject> ignoredBall = new List<GameObject>();
    public bool oneSidedPipe = false;
    public GameObject waitingPos;

    private Vector3 ballEntryVelocity;
    private bool isTping = false;

    public void InitPipeTP(int entry, GameObject ball)
    {
        if (!isTping)
        {
            if (!oneSidedPipe)
            {
                switch (entry)
                {
                    case 0:
                        StartCoroutine(TpBallAtPos(OrderedPipeEntryList[1], ball));
                        break;
                    case 1:
                        StartCoroutine(TpBallAtPos(OrderedPipeEntryList[0], ball));
                        break;
                    default:
                        return;
                }
            }
            else
            {
                if (entry == 0)
                {
                    StartCoroutine(TpBallAtPos(OrderedPipeEntryList[1], ball));
                }
            }
        }
    }

    IEnumerator TpBallAtPos(GameObject exitEntry, GameObject ball)
    {
        Rigidbody _Rb = ball.GetComponent<Rigidbody>();

        // Collider touché >>
        isTping = true;
        ballEntryVelocity = _Rb.velocity;
        ball.transform.position = waitingPos.transform.position;
        ignoredBall.Add(ball);
        ball.SetActive(false);

        yield return new WaitForSeconds(timeToTeleport);

        //TP >>
        ball.transform.position = exitEntry.transform.position;
        _Rb.velocity = ballEntryVelocity;
        ball.SetActive(true);
        isTping = false;
    }
}
