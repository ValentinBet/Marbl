﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MenuAnim : MonoBehaviour
{
    public CinemachineVirtualCamera CVC;
    private CinemachineOrbitalTransposer cot;
    public float orbitalRotation;
    public Transform rotateMarbl;
    public float additionalRotation;
    private float timer = 0.0f;
    private float timerB = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        cot = CVC.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        timerB += Time.deltaTime;
        if (timer > 1)
            timer = timer - 1;
        if (timerB > 10.0f)
            timerB = timerB - 10;
        rotateMarbl.transform.localRotation = Quaternion.Euler(0, 0, -timer * 360.0f - additionalRotation);
        cot.m_Heading.m_Bias = -180.0f + timerB * 36.0f;
        if(Input.anyKeyDown)
        {
            GetComponent<Animator>().SetTrigger("Clicked");
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            cot.m_XAxis.m_MaxSpeed = 300.0f;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            cot.m_XAxis.m_MaxSpeed = 0.0f;
        }
    }
}
