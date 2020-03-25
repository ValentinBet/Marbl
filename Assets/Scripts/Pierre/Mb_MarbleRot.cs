using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Mb_MarbleRot : MonoBehaviour
{
    CinemachineOrbitalTransposer cot;
    [SerializeField] CinemachineVirtualCamera CVC;
    // Start is called before the first frame update
    void Start()
    {
        cot = CVC.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
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
