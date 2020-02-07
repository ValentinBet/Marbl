using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonObj : MonoBehaviour
{
    public TextMeshProUGUI Value;

    public void SetValue(int number)
    {
        Value.text = number.ToString();
    }
}
