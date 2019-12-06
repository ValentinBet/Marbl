using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map
{
    public string mapName;

    public List<MapObject> mapObjects;

    public Vector3 mapObjectsPosition;
    public Quaternion mapObjectsRotation;
    public Vector3 mapObjectsScale;

    public Vector3 fixedSpawnPosition;
    public Quaternion fixedSpawnRotation;
    public Vector3 fixedSpawnScale;

    public Vector3 randomSpawnPosition;
    public Quaternion randomSpawnRotation;
    public Vector3 randomSpawnScale;

    public Vector3 team1Scale;
    public Vector3 team2Scale;
    public Vector3 team3Scale;
    public Vector3 team4Scale;

}
