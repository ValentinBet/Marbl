using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueManager : MonoBehaviour
{
    private static HueManager _instance;
    public static HueManager Instance { get { return _instance; } }

    private Transform hueNeutralsBalls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    private void Start()
    {
        hueNeutralsBalls = GameModeManager.Instance.listNeutralPos;

        InitNeutralBalls();
    }

    private void InitNeutralBalls()
    {

    }

    public void ActiveThisMode(bool value)
    {
        if (value)
        {
            return;
        }
        else
        {
            enabled = false;
        }
    }
}
