using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static int actualTurn = 0;
    static TurnBasedItem[] TBI;
    static TurnManager tm;
    public static bool changingturn = false;

    private void Start()
    {
        if (tm == null)
        {
            tm = this;
        }
        actualTurn = 0;
        TurnSwap(0);
    }

    static void TurnSwap(int _turnID)
    {
        TBI = FindObjectsOfType<TurnBasedItem>();
        foreach ( TurnBasedItem _tbi in TBI)
        {
            _tbi.NewTurn(_turnID);
        }
    }

    public static void NextTurn()
    {
        tm.NextTurnRoutine();
    }

    public static void NextTurImmediate()
    {
        tm.NextTurnNow();
    }

    private void NextTurnRoutine()
    {
        changingturn = true;
        Invoke("NextTurnNow", 10f);
    }

    private void NextTurnNow()
    {
        changingturn = false;
        actualTurn = (actualTurn + 1) % GameData.playerAmount;
        if (!ScoreManager.IsElim(actualTurn))
        {
            TurnSwap(actualTurn);
            MouseControl.NewTurn();
            CameraAverage.NextPlayerStatic();
        } else
        {
            NextTurnNow();
        }
    }

    public static int GetActualTurn()
    {
        return actualTurn;
    }
}
