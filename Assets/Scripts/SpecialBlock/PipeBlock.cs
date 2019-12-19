using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEntrySave
{
    public GameObject entryGo;
    public GameObject exitGo;
    public Vector3 currentPos;
    public bool isMovingToExit;
    public float tempTime;
}

public class PipeBlock : MonoBehaviour
{
    public float timeToTeleport;
    public float exitPower = 10;
    public List<GameObject> OrderedPipeEntryList = new List<GameObject>();
    private Vector3 ballEntryVelocity;

    public Dictionary<GameObject, PipeEntrySave> ignoredBall = new Dictionary<GameObject, PipeEntrySave>();



    public void InitPipeTP(int entry, GameObject ball)
    {
        if (ignoredBall.ContainsKey(ball))
        {
            return;
        }

        PipeEntrySave _pes = new PipeEntrySave();

        switch (entry)
        {
            case 0:
                _pes.exitGo = OrderedPipeEntryList[1];                
                break;
            case 1:
                _pes.exitGo = OrderedPipeEntryList[0];
                break;
            default:
                return;
        }
        _pes.entryGo = OrderedPipeEntryList[entry];
        ignoredBall.Add(ball, _pes);
        StartCoroutine(TpBallAtPos(ball));

    }

    IEnumerator TpBallAtPos(GameObject ball)
    {
        Rigidbody _Rb = ball.GetComponent<Rigidbody>();
        BallSettings _Bs = ball.GetComponent<BallSettings>();

        // Collider touché >>
        _Bs.isOnPipe = true;
        ballEntryVelocity = _Rb.velocity;
        ball.SetActive(false);

        //-----
        ignoredBall[ball].tempTime = 0f;
        ignoredBall[ball].isMovingToExit = true;
        ignoredBall[ball].currentPos = ball.transform.position;
        yield return new WaitForSeconds(timeToTeleport);

        ignoredBall[ball].isMovingToExit = false;
        //TP >>
        ball.SetActive(true);
        _Bs.isOnPipe = false;
        _Rb.velocity = GetExitVelocity(ball);
        ball = null;
    }

    private void Update()
    {
        MoveBallToPosition();
    }

    private void MoveBallToPosition()
    {
        if (ignoredBall.Count > 0)
        {
            foreach (KeyValuePair<GameObject, PipeEntrySave> keyValue in ignoredBall)
            {
                if (keyValue.Value.isMovingToExit)
                {
                    keyValue.Value.tempTime += Time.deltaTime / timeToTeleport;
                    keyValue.Key.transform.position = Vector3.Lerp(keyValue.Value.currentPos, keyValue.Value.exitGo.transform.position, keyValue.Value.tempTime);
                }
            }
        }
    }

    private Vector3 GetExitVelocity(GameObject ball)
    {
        PipeEntryBlock entryBlock = ignoredBall[ball].exitGo.GetComponent<PipeEntryBlock>();

        Vector3 _ExitVel = Vector3.zero;
        Vector3 _dir = (entryBlock.GetExitDirectionVector() / entryBlock.GetExitDirectionVector().magnitude);
        _ExitVel = _dir * exitPower;
        return _ExitVel;
    }
}
