using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputsList.asset", menuName = "Tools/Inputs List", order = 100)]
public class InputsList : ScriptableObject
{
    public Inputs inputs;
    public bool init;
}

[System.Serializable]
public struct Inputs
{
    public KeyCode MainButton1;
    public KeyCode MainButton2;
    public KeyCode Learderboard;
    public KeyCode CameraForward;
    public KeyCode CameraBackward;
    public KeyCode CameraLeft;
    public KeyCode CameraRight;

    public float MouseSensitivity;
}