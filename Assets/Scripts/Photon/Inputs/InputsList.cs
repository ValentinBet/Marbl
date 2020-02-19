﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputsList.asset", menuName = "Tools/Inputs List", order = 100)]
public class InputsList : ScriptableObject
{
    public Inputs inputs;
}

[System.Serializable]
public struct Inputs
{
    public KeyCode MainButton1;
    public KeyCode MainButton2;
    public KeyCode Learderboard;
    public KeyCode Ping;
    public KeyCode Chat;
    public KeyCode CameraForward;
    public KeyCode CameraBackward;
    public KeyCode CameraLeft;
    public KeyCode CameraRight;
    public KeyCode CameraSpeed;
    public KeyCode FollowCam;
    public KeyCode TopCam;
    public KeyCode SpecCam;

    public float MouseSensitivity;
    public float GeneralVolume;
}