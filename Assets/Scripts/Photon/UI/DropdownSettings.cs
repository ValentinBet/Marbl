using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DropdownSettings : MonoBehaviour
{
    public Dropdown myDropdown;

    public UnityEvent OnChangeValue5;
    public UnityEvent OnChangeValue15;

    public int defaultValue;

    private void Start()
    {
        ResetValue();
    }

    public void SetValue(int value)
    {
        myDropdown.value = value;
        RoomSettings.Instance.SaveSettings();
    }

    public void ResetValue()
    {
        myDropdown.value = defaultValue;
    }

    public void ChangeDrop()
    {
        if(myDropdown.value == 0)
        {
            OnChangeValue5.Invoke();
        }
        else
        {
            OnChangeValue15.Invoke();
        }
    }

}
