using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkinInfos", menuName ="CreatePlayerSkinInfos")]
public class Sc_Skininfos : ScriptableObject
{
    public int skinIndex;
    public Material[] allskins;
}
