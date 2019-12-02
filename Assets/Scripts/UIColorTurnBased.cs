using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorTurnBased : TurnBasedItem
{
    private Image image;

    public override void NewTurn(int _turnId)
    {
        base.NewTurn(_turnId);
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        image.color = GameData.playerColors[_turnId];
    }

}
