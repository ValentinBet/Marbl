using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="NewGameMode",menuName ="MARBL/GameMode",order =0)]
public class ModeData :ScriptableObject
{
    public bool isIncremental;
    public int incrementalScoreToWin; //Not used in decremental
    public int decrementalStartingScore; //Not used in Incremental
    public string scoreName; //Item to indicate Score "Flags/Marbles/ etc..."
}
