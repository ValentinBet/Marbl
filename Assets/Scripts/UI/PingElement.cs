using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingElement : MonoBehaviour
{
    public SpriteRenderer Socle;
    public LineRenderer Trail;

    public void SetColor(Color newColor)
    {
        Socle.color = newColor;
        Gradient grd = Trail.colorGradient;
        GradientColorKey[] ck = grd.colorKeys;
        ck[0].color = newColor;
        ck[1].color = newColor;
        grd.SetKeys(ck, grd.alphaKeys);
        Trail.colorGradient= grd;
    }

    public void Hide()
    {
        Trail.enabled = false;
    }

    public void Show()
    {
        Trail.enabled = true;
    }
}
