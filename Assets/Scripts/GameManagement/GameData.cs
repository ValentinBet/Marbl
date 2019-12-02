using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
	public static int playerAmount = 0;
//    public static int[] previousOrder = 0; used for order;
    public static int previousMode = 0;
    public static int[] playersScore;
    public static readonly int scoreToWin = 15;
    public static Color[] playerColors = new Color[4] { new Color(1.0f,0.364f,0.364f),
                                                        new Color(0.3647059f,0.4282354f,1.0f),
                                                        new Color(0.3647059f,1.0f,0.4494119f),
                                                        new Color(1.0f,0.8941177f,0.3647059f) };
    public static ModeData actualMode;
    public static ModifierProfile actualProfile;
    //Upgrade pour un dictionnaire ?
}
