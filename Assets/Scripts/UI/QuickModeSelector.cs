using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickModeSelector : MonoBehaviour
{
    public Image background;
    public Color highlightedColor;
    public string mode;
    private RoomScripts roomScripts;

    void Start()
    {
        roomScripts = RoomScripts.Instance;
    }

    void Update()
    {
        if (mode == "Custom")
        {
            if (roomScripts.isCustom)
            {
                background.color = highlightedColor;
            }
            else
            {
                background.color = Color.white;
            }
        }
        else
        {
            if (roomScripts.GetActualMode() == mode)
            {
                background.color = highlightedColor;
            }
            else
            {
                background.color = Color.white;
            }
        }

        roomScripts.RefreshMaps();
    }
}
