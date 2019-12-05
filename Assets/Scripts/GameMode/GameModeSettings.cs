using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GameModeSettings
{
    public int map;
    public bool deathmatch;
    public bool hill;
    public bool coins;
    public bool potato;
    public bool hue;
    public bool billard;
    public int turnLimit;
    public int round;

    public int nbrBall;
    public int spawnMode;
    public float launchPower;
    public float impactPower;

    public int winPointDM;
    public int elimPointDM;
    public bool killstreakDM;
    public int suicidePointDM;

    public int nbrHill;
    public int hillMode;
    public int hillPoint;

    public int coinsAmount;

    public int potatoTurnMin;
    public int potatoTurnMax;

    public int hueNutralBall;

    public int billardBall;
}