using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject CameraMain;
    public GameObject CameraTop;
    public GameObject CameraSpec;

    public Transform CamSpecNetwork;

    public CinemachineVirtualCamera CineMain;
    public CinemachineVirtualCamera CineTop;
    public CinemachineVirtualCamera CineSpec;

    private static CameraManager _instance;
    public static CameraManager Instance { get { return _instance; } }

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
}
