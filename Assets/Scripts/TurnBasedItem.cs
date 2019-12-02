using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedItem : MonoBehaviour
{
    [SerializeField]
    protected int myTID = 0;
    private bool selectable = false;
    
    public virtual void NewTurn(int _turnId)
    {
        if(_turnId == myTID)
        {
            selectable = true;
        } else
        {
            selectable = false;
        }
    }

    public bool IsSelectable()
    {
        return selectable;
    }

    public int GetID()
    {
        return myTID;
    }
}
