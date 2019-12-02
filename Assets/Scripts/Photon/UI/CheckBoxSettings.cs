using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckBoxSettings : MonoBehaviour
{
    public Image checkedImg;
    public bool statut = false;

    public bool defaultValue;

    public UnityEvent OnActive;
    public UnityEvent OnDisable;

    private void Start()
    {
        ResetValue();
    }

    public void SetValue(bool value)
    {
        checkedImg.enabled = value;
        statut = value;

        if (statut)
        {
            OnActive.Invoke();
        }
        else
        {
            OnDisable.Invoke();
        }

        RoomSettings.Instance.Refresh();
    }

    public void ResetValue()
    {
        SetValue(defaultValue);
    }

    public void OnClick()
    {
        if (statut)
        {
            SetValue(false);
            OnDisable.Invoke();
        }
        else
        {
            SetValue(true);
            OnActive.Invoke();
        }
        RoomSettings.Instance.Refresh();
    }

    public void StartGame()
    {

    }
}
