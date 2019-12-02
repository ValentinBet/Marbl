using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreTurnBased : TurnBasedItem
{
    private Text text;
    
    public override void NewTurn(int _turnId)
    {
        base.NewTurn(_turnId);
        if (text == null)
        {
            text = GetComponent<Text>();
        }
        text.color = GameData.playerColors[_turnId];
        text.text = ScoreManager.GetScore(_turnId) + " " + GameData.actualMode.scoreName;
    }
}
