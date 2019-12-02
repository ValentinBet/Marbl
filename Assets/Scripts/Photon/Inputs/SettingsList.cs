using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsList.asset", menuName = "Tools/Settings List", order = 100)]
public class SettingsList : ScriptableObject
{
    public Settings settings;
}

[System.Serializable]
public struct Settings
{
    public int Windowmode;
    public int Resolution;
    public int Quality;
    public float GeneralVolume;

    public float MouseSensitivity;
}
