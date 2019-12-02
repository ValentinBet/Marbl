using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static int[] playerScores;
    private static int[] playerRemainingMarbles;
    private static int[] playerOrder;
    private static int eliminated = 0;
    private static bool[] isPlayerElim;
    private static int[] pointsGiven = new int[4] { 1, 2, 3, 5 };

    [SerializeField]private Animator EndGameAnim;

    static int maxplayer = 0;

    //ToReplace
    static ScoreManager sm;

    void Awake()
    {
        if(sm == null)
        {
            sm = this;
        }
        eliminated = 0;
        playerOrder = new int[GameData.playerAmount];
        playerRemainingMarbles = new int[GameData.playerAmount];
        //ranking for this game.
        playerScores = new int[GameData.playerAmount];
        isPlayerElim = new bool[GameData.playerAmount];
        for (int i = 0; i < GameData.playerAmount; i++)
        {
            playerScores[i] = GameData.actualMode.isIncremental ? 0 : GameData.actualMode.decrementalStartingScore;
            isPlayerElim[i] = false;
        }
    }

    public static void LowerScore(int _playerID)
    {
        playerScores[_playerID]--;
        CheckElim();
    }

    public static int GetScore(int _playerID)
    {
        return playerScores[_playerID];
    }

    //Eliminations only occurs in Decremental GameModes for now. (tweakable later)
    public static bool IsElim(int _playerID)
    {
        if (GameData.actualMode.decrementalStartingScore > playerScores[_playerID])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    static void CheckElim()
    {
        for(int i =0; i < GameData.playerAmount;i++)
        {
            if(playerScores[i] == 0 && isPlayerElim[i] == false)
            {
                playerOrder[eliminated] = i;
                Debug.Log("Player (" + i + ") is eliminated");
                isPlayerElim[i] = true;
                eliminated++;
            }
        }
        Debug.Log("("+eliminated+") currentlyEliminated");
        if (eliminated == GameData.playerAmount - 1)
        {
            EndSquence();
        }
    }

    static void EndSquence()
    {
        for (int i = 0; i < GameData.playerAmount; i++)
        {
            if (isPlayerElim[i] == false)
            {
                playerOrder[eliminated] = i;
            }
        }
        for (int i = 0; i < GameData.playerAmount; i++)
        {
            GameData.playersScore[i] += pointsGiven[i];
            if (GameData.playersScore[i] > maxplayer)
            {
                maxplayer = i;
            }
        }

        sm.EndGameAnim.Play("Podium");
    }

    public void EndingTransition()
    {
        if (GameData.playersScore[maxplayer] < GameData.scoreToWin)
        {
            GameLauncher.NextMode();
        }
        else
        {
            GameLauncher.BackToMenu();
        }
    }
}
