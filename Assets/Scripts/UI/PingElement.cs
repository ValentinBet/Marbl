using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingElement : MonoBehaviour
{
    public List<SpriteRenderer> Socles = new List<SpriteRenderer>();

    public List<SpriteRenderer> BlackCircles = new List<SpriteRenderer>();

    public void SetColor(Color newColor)
    {
        foreach (SpriteRenderer socle in Socles)
        {
            socle.color = newColor;
        }

        /*
        Gradient grd = Trail.colorGradient;
        GradientColorKey[] ck = grd.colorKeys;
        ck[0].color = newColor;
        ck[1].color = newColor;
        grd.SetKeys(ck, grd.alphaKeys);
        Trail.colorGradient= grd;*/
    }


    public void SetAlphaCircles(float alpha)
    {
        foreach (SpriteRenderer socle in Socles)
        {
            socle.color = new Color(socle.color.r, socle.color.g, socle.color.b, alpha);
        }
    }
}

