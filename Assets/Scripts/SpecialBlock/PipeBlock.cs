using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBlock : MonoBehaviour
{
    public float timeToTeleport;
    public float exitPower = 10;
    public List<GameObject> OrderedPipeEntryList = new List<GameObject>();
    public bool oneSidedPipe = false;
    private Vector3 ballEntryVelocity;
    private bool isTping = false;
    public List<GameObject> ignoredBall = new List<GameObject>();
    public GameObject entryGo;

    private void Update()
    {
        MoveBallToPosition();
    }


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
                        entryGo = OrderedPipeEntryList[0];
                        break;
                    case 1:
                        StartCoroutine(TpBallAtPos(OrderedPipeEntryList[0], ball));
                        entryGo = OrderedPipeEntryList[1];
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
        BallSettings _Bs = ball.GetComponent<BallSettings>();
        
        // Collider touché >>
        isTping = true;
        _Bs.isOnPipe = true;
        ballEntryVelocity = _Rb.velocity;
        ignoredBall.Add(ball);
        ball.SetActive(false);
        ball.transform.position = exitEntry.transform.position;
        yield return new WaitForSeconds(timeToTeleport);

        //TP >>
        ball.SetActive(true);
        _Bs.isOnPipe = false;
        _Rb.velocity = GetExitVelocity(exitEntry);
        isTping = false;
        entryGo = null;
    }

    private void MoveBallToPosition()
    {

    }

    private Vector3 GetExitVelocity(GameObject exitEntry)
    {
        PipeEntryBlock entryBlock = exitEntry.GetComponent<PipeEntryBlock>();

        Vector3 _ExitVel = Vector3.zero;
        Vector3 _dir = (entryBlock.GetExitDirectionVector() / entryBlock.GetExitDirectionVector().magnitude);
        _ExitVel = _dir *exitPower;
        return _ExitVel;
    }
}
