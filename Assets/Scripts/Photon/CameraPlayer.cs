using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static MarblGame;
using Photon.Pun;

public class CameraPlayer : MonoBehaviour
{
    #region Variables
    public float curveVariation = 1.5f;
    public AudioClip sd_cameraChange;

    private AudioSource audioSource;
    private float orbitalAngle;
    private Quaternion initialRotation;

    [SerializeField]
    private CameraMode actualMode = CameraMode.Top;

    private CameraMode saveMode = CameraMode.Top;

    private Camera myCamera;
    public CinemachineVirtualCamera[] cameras;
    private Transform mapPivot;
    public Transform followBall;
    private Transform targetedTransform; //FollowedMarbletransform;
    private PUNMouseControl myMouseControl;
    private LocalPlayerManager myLocalPlayerManager;
    private bool isScreenShaking = false;

    Transform camSpec;

    #endregion

    private void OnEnable()
    {
        myMouseControl.OnBallClicked += OnClickOnBall;
        UIManager.Instance.OnTopCam += SetCameraMode;
        UIManager.Instance.OnSpecCam += SetCameraMode;
        UIManager.Instance.OnMainCam += SetCameraMode;
        UIManager.Instance.OnSetSavedCam += SetPrefedMode;
    }

    private void OnDisable()
    {
        myMouseControl.OnBallClicked -= OnClickOnBall;
        UIManager.Instance.OnTopCam -= SetCameraMode;
        UIManager.Instance.OnSpecCam -= SetCameraMode;
        UIManager.Instance.OnMainCam -= SetCameraMode;
        UIManager.Instance.OnSetSavedCam -= SetPrefedMode;
    }

    void OnClickOnBall(GameObject ball)
    {
        targetedTransform = ball.transform;
        //initialRotation = ball.transform.rotation;
        //orbitalAngle = 0;
        SetCameraMode(CameraMode.Targeted);
    }

    public void SetFollowBall(Transform newBall)
    {
        followBall = newBall;
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
    }

    private void Start()
    {
        if (GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void InitializeCameras()
    {
        cameras = new CinemachineVirtualCamera[3];

        cameras[0] = CameraManager.Instance.CineMain;
        cameras[1] = CameraManager.Instance.CineTop;
        cameras[2] = CameraManager.Instance.CineSpec;
    }
    #endregion

    void Update()
    {
        ballModeCalculation();

        if (camSpec == null)
        {
            camSpec = CameraManager.Instance.CameraSpec;
            return;
        }

        if (GameModeManager.Instance.localPlayerTurn)
        {
            switch (actualMode)
            {
                case CameraMode.Top:
                    camSpec.position = cameras[1].transform.position;
                    camSpec.rotation = cameras[1].transform.rotation;
                    break;

                default:
                    camSpec.position = cameras[0].transform.position;
                    camSpec.rotation = cameras[0].transform.rotation;
                    break;
            }
        }
        else
        {
            cameras[2].transform.position = camSpec.position;
            cameras[2].transform.rotation = camSpec.rotation;
        }
    }

    public void SetPrefedMode()
    {
        SetCameraMode(saveMode);
    }

    public void SetCameraMode(CameraMode _newMode)
    {
        myCamera.transform.rotation = Quaternion.identity;

        if (actualMode != _newMode)  // SI --> la caméra précédente est différente de la nouvelle caméra
        {
            if (sd_cameraChange != null && audioSource != null)
            {
                audioSource.PlayOneShot(sd_cameraChange);
            }
        }

        actualMode = _newMode;

        int _player = 0;

        switch (actualMode)
        {
            case CameraMode.Top:
                myLocalPlayerManager.SetBlock(true);

                myCamera.transform.position = mapPivot.position;
                cameras[0].Priority = 0;
                cameras[1].Priority = 100;
                cameras[2].Priority = 0;
                saveMode = actualMode;
                break;

            case CameraMode.Targeted:
                myLocalPlayerManager.SetBlock(false);
                cameras[0].Priority = 100;
                cameras[1].Priority = 0;
                cameras[2].Priority = 0;
                saveMode = actualMode;
                break;

            case CameraMode.SpecMode:
                myLocalPlayerManager.SetBlock(false);
                cameras[0].Priority = 0;
                cameras[1].Priority = 0;
                cameras[2].Priority = 100;
                break;
        }
    }

    private void ballModeCalculation()
    {
        if (targetedTransform == null)
        {
            GameObject[] Balls = GameObject.FindGameObjectsWithTag("Ball");
            targetedTransform = Balls[Random.Range(0, Balls.Length)].transform;
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
            //camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            //camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = intensity;
        }
    }

    public CameraMode GetCurrentMode()
    {
        return actualMode;
    }
}
