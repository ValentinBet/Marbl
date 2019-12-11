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
        GradientColorKey[] ck = Trail.colorGradient.colorKeys;
        ck[1].color = newColor;
        Trail.colorGradient.SetKeys(ck, Trail.colorGradient.alphaKeys);
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
