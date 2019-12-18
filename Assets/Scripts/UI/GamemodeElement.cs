using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeElement : MonoBehaviour
{
    public Image Icon;
    public Text labelMode;
    public Text Description;
    public int indexMode;

    [SerializeField]
    public Tag myTags;
}

public class Tag
{
    public bool Deathmatch = false;
    public bool KingOfTheHill = false;
    public bool Hue = false;
    public bool Coins = false;
    public bool Potato = false;
}
