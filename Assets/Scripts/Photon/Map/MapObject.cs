using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapObject
{
    public string objectName;
    public string prefabName;

    public string rootParent;
    public string teamNumber;

    public bool isRootParent;

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}
