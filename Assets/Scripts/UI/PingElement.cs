using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingElement : MonoBehaviour
{
    public SpriteRenderer Socle;
    public SpriteRenderer Trail1;
    public SpriteRenderer Trail2;

    public void SetColor(Color newColor)
    {
        Socle.color = newColor;
        Trail1.color = newColor;
        Trail2.color = newColor;
    }

    public void Hide()
    {
        Trail1.enabled = false;
        Trail2.enabled = false;
    }

    public void Show()
    {
        Trail1.enabled = true;
        Trail2.enabled = true;
    }
}
