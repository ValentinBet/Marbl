using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSettings : MonoBehaviour
{
    public Slider mySlider;
    public InputField myInputfield;

    public bool HaveInfini = false;

    public float defaultValue;

    private void Start()
    {
        ResetValue();
    }

    public void SetValue(float value)
    {
        myInputfield.text = value.ToString();
        SetSlider();
    }

    public void ResetValue()
    {
        myInputfield.text = defaultValue.ToString();
        SetSlider();
    }

    public void SetText()
    {
        if (mySlider.value == mySlider.maxValue && HaveInfini)
        {
            myInputfield.text = "∞";
        }
        else
        {
            myInputfield.text = Math.Round(mySlider.value, 3).ToString();
        }

        RoomSettings.Instance.SaveSettings();
    }

    public void SetSlider()
    {
        float value;
        if (myInputfield.text.Contains("."))
        {
            myInputfield.text = myInputfield.text.Replace('.', ',');
        }
        if (float.TryParse(myInputfield.text, out value)) 
        {
            if (float.Parse(myInputfield.text) >= mySlider.maxValue && HaveInfini)
            {
                mySlider.value = mySlider.maxValue;
                myInputfield.text = "∞";
            }
            else
            {
                mySlider.value = float.Parse(myInputfield.text);
            }
        }
        else
        {
            myInputfield.text = Math.Round(mySlider.value, 3).ToString();
        }
    }

    public void SetMax(float value)
    {
        mySlider.maxValue = value;
    }

    public void SetMin(float value)
    {
        mySlider.minValue = value;
    }
}
