using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [HideInInspector]
    public GameObject actualSelectedBall = null;
    [SerializeField]
    private GameObject[] TeamBalls;
    [SerializeField]
    private List<GameObject> listBalls = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < TeamBalls.Length; i++)
        {
            foreach (Transform c in TeamBalls[i].transform)
            {
                listBalls.Add(c.gameObject);
            }
        }
    }

    public void NewBallSelected(GameObject ball)
    {
        actualSelectedBall = ball;
        ControlOutline(ball);
    }

    public void ResetBallSelect()
    {
        if (actualSelectedBall != null)
        {
            actualSelectedBall.GetComponent<BallControl>().SetOutline(false);
        }
        actualSelectedBall = null;
    }

    private void ControlOutline(GameObject ball)
    {
        BallControl _ballControl = ball.GetComponent<BallControl>();
        _ballControl.SetOutline(true);

        listBalls.Remove(null);
        foreach (GameObject b in listBalls)
        {
            if (b == null)
            {
                continue;
            }
            if (b != ball)
            {
                b.GetComponent<BallControl>().SetOutline(false);
            }
        }
        
    }
}