using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickSettings : MonoBehaviour
{
    public SettingsManager SM;

    public void CloseAnim()
    {
        SM.QuitScene();
    }
}
