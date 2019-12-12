using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static MarblGame;

public class CameraPlayer : MonoBehaviour
{
    #region Variables
    public float curveVariation = 1.5f;
    private float orbitalAngle;
    private Quaternion initialRotation;
    private Transform mapPivot;
    Camera myCamera;
    private CinemachineVirtualCamera[] cameras;
    private static CameraMode actualMode = CameraMode.Void;
    Transform followBall;

    Transform targetedTransform; //FollowedMarbletransform;

    PUNMouseControl myMouseControl;
    LocalPlayerManager myLocalPlayerManager;

    bool isScreenShaking = false;

    #endregion

    private void OnEnable()
    {
        myMouseControl.OnBallClicked += OnClickOnBall;
        UIManager.Instance.OnTopCam += SetCameraMode;
        UIManager.Instance.OnTeamCam += SetCameraMode;
        UIManager.Instance.OnFreeCam += SetCameraMode;
    }

    private void OnDisable()
    {
        myMouseControl.OnBallClicked -= OnClickOnBall;
        UIManager.Instance.OnTopCam -= SetCameraMode;
        UIManager.Instance.OnTeamCam -= SetCameraMode;
        UIManager.Instance.OnFreeCam -= SetCameraMode;
    }

    void OnClickOnBall(GameObject ball)
    {
        targetedTransform = ball.transform;
        //initialRotation = ball.transform.rotation;
        //orbitalAngle = 0;
        SetCameraMode(CameraMode.Targeted);
    }

    #region Initialisation
    void Awake()
    {
        myLocalPlayerManager = GetComponent<LocalPlayerManager>();
        myCamera = Camera.main;
        myMouseControl = GetComponent<PUNMouseControl>();
        followBall = GameObject.Find("Cam Follow Point").transform;

        mapPivot = GameObject.Find("MapPivot").transform;
        InitializeCameras();

        InitCam();
    }

    public void InitCam()
    {
        SetCameraMode(CameraMode.MapCentered);
    }

    private void InitializeCameras()
    {
        cameras = new CinemachineVirtualCamera[4];

        cameras[0] = CameraManager.Instance.CineMain;
        cameras[1] = CameraManager.Instance.CineTop;
        cameras[2] = CameraManager.Instance.CineTeam;
        cameras[3] = CameraManager.Instance.CineFree;
    }
    #endregion

    void Update()
    {
        if (actualMode == CameraMode.Targeted)
        {
            ballModeCalculation();
        }

        if (actualMode == CameraMode.Free)
        {
            CameraManager.Instance.CameraFree.GetComponent<FreeCam>().onFreeCam = true;
        }
        else
        {
            CameraManager.Instance.CameraFree.GetComponent<FreeCam>().onFreeCam = false;
        }
    }

    #region CameraManipulation

    public void SetCameraMode(CameraMode _newMode)
    {
        myCamera.transform.rotation = Quaternion.identity;

        actualMode = _newMode;
        int _player = 0;

        switch (actualMode)
        {
            case CameraMode.MapCentered:
                myLocalPlayerManager.SetBlock(true);

                myCamera.transform.position = mapPivot.position;
                cameras[0].Priority = 0;
                cameras[1].Priority = 100;
                cameras[2].Priority = 0;
                cameras[3].Priority = 0;
                break;

            case CameraMode.TeamCentered:
                myLocalPlayerManager.SetBlock(true);
                Vector3 _averagePos = Vector3.zero;

                myLocalPlayerManager.GetMyBalls();

                foreach (GameObject ball in myLocalPlayerManager.teamBalls)
                {
                    _averagePos += ball.transform.position;
                }

                _averagePos /= myLocalPlayerManager.teamBalls.Count;
                myCamera.transform.position = new Vector3(_averagePos.x, 0, _averagePos.z);
                cameras[0].Priority = 0;
                cameras[1].Priority = 0;
                cameras[2].Priority = 100;
                cameras[3].Priority = 0;
                break;

            case CameraMode.Targeted:
                myLocalPlayerManager.SetBlock(false);
                cameras[2].Priority = 8;
                cameras[0].Priority = 100;
                cameras[1].Priority = 0;
                cameras[2].Priority = 0;
                cameras[3].Priority = 0;
                break;

            case CameraMode.Free:
                myLocalPlayerManager.SetBlock(true);
                cameras[0].Priority = 0;
                cameras[1].Priority = 0;
                cameras[2].Priority = 0;
                cameras[3].Priority = 100;
                break;
        }
    }

    //For UI mainly;
    public void ManualSet(CameraMode _newMode)
    {
        SetCameraMode(_newMode);
    }

    //AutoSet To Map View
    public void ManualSet()
    {
        SetCameraMode(CameraMode.MapCentered);
    }

    private void ballModeCalculation()
    {
        if (targetedTransform == null)
        {
            SetCameraMode(CameraMode.MapCentered);
            return;
        }

        followBall.transform.position = targetedTransform.position;

        if (Input.GetKey(InputManager.Instance.Inputs.inputs.MainButton1) || Input.GetKey(InputManager.Instance.Inputs.inputs.MainButton2))
        {
            //orbitalAngle += Input.GetAxis("Mouse X") * 2.5f;
            //followBall.transform.rotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0, orbitalAngle, 0));// + myMouseControl.possibleAngles[myMouseControl.angleIndex] * curveVariation, 0));
            followBall.transform.rotation = Quaternion.Euler(followBall.transform.rotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X") * 2.5f, 0));
        }
    }


    public void InitShakeScreen(float intensity, float duration)
    {
        if (!isScreenShaking)
        {
            StartCoroutine(ProcessShake(intensity, duration));
        }
    }

    private IEnumerator ProcessShake(float intensity, float duration)
    {
        isScreenShaking = true;
        CamerasShake(intensity);
        yield return new WaitForSeconds(duration);
        CamerasShake(0);
        isScreenShaking = false;
    }

    private void CamerasShake(float intensity)
    {
        foreach (CinemachineVirtualCamera camera in cameras)
        {
            camera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            camera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = intensity;
        }
    }

    #endregion
}
