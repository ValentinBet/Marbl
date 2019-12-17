using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingChoiceSettings : MonoBehaviour
{
    public Image leftCornerImage;
    public Image rightCornerImage;
    public Image topCornerImage;
    public Image bottomCornerImage;
    public Image centerImage;
    public bool onEnterCenter;
    public Color HighlightedCornerColor;

    public void SetOnEnterCenter(bool value)
    {
        onEnterCenter = value;
    }
}
