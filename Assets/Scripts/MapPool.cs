using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapPool.asset", menuName = "Tools/MapPool", order = 100)]
public class MapPool : ScriptableObject
{
    public List<string> DeathmatchPool = new List<string>();
    public List<string> HuePool = new List<string>();
    public List<string> KothPool = new List<string>();
}