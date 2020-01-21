using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeElement : MonoBehaviour
{
    public Image Icon;
    public Text labelMode;
    public Text Description;
    public Image imgOutline;

    public bool Deathmatch = false;
    public bool KingOfTheHill = false;
    public bool Hue = false;

    public bool isCustom = false;

    public string fileName;

    public void OnClick()
    {
        RoomScripts.Instance.customMode = false;
        RoomScripts.Instance.customModeSave = isCustom;
        RoomScripts.Instance.fileModeName = fileName;

        RoomScripts.Instance.SetMode(imgOutline);

        RoomSettings.Instance.SetMode(fileName, isCustom);
    }
}
