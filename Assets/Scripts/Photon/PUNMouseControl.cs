using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun.UtilityScripts;

public class PUNMouseControl : MonoBehaviourPunCallbacks
{
    public UnityAction<GameObject> OnBallClicked;
    public float dragForce;
    public float elevation;

    public UnityAction OnShooted;
    public GameObject actualSelectedBall = null;
    public BallSettings actualSelectedBallSettings;
    public bool haveShoot = false;
    public float scrollSensitivity = 0.1f;
    public float[] possibleAngles;
    public LayerMask layerClickBall;
    public float timeBeforeShoot = 1.5f;
    [SerializeField]
    private LineRenderer actualBallLineRenderer;

    [Header("Raycast Variables")]
    [Space(5)]

    private Ray ray;
    private RaycastHit hit;
    private PhotonView photonView;
    private GameObject lastSelected;
    private LocalPlayerManager player;
    private GameModeManager gameModeManager;
    private Camera mainCamera;
    private Vector3 lineRendererOriginPosition;
    private Vector3 lineRendererEndPosition;
    private Vector3 direction;
    private Image dragForceBar;

    [SerializeField]
    private float cancelForce = 0.08f;
    [SerializeField]
    private float dragForceMaxValue = 7;
    [SerializeField]
    private float mouseSensitivity = 1;
    [SerializeField]
    private float camScrollSensitivity = 0.1f;

    [SerializeField]
    private Vector2 minimalValues = new Vector2(0.5f, 1.8f);
    [SerializeField]
    private Vector2 maximalValues = new Vector2(9.0f, 8.0f);

    private Vector2 cameraValues;
    private CameraPlayer CP;
    public int angleIndex = 0;
    private float mouseScrollDelta;
    private float camCoord=0.11f;
    private float camAnimCoord=0.11f;
    public bool isHoldingShoot;

    private bool canShoot = true;
    private bool isStoppingShoot = false;
    private float dragPowerPrctg;

    public Gradient lineGradient;


    private void Start()
    {
        player = GetComponent<LocalPlayerManager>();
        gameModeManager = GameModeManager.Instance;
        photonView = GetComponent<PhotonView>();
        mainCamera = Camera.main;
        dragForceBar = UIManager.Instance.dragForceBar;
        CP = GetComponent<CameraPlayer>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        DetectClick();

        if (player.GetCanShoot() && !haveShoot)
        {
            MouseDrag();
        }
        if (isHoldingShoot == false)
        {
            ElevateCamera();
        }
    }

    private void DetectClick()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton1))
        {
            if (Physics.Raycast(ray, out hit, 1000, layerClickBall))
            {
                StopShoot();
                actualSelectedBall = null;
                ClickOnBall(hit.collider.transform.parent.gameObject); // cause we collide with "ClickBall" direct child of the marbl
                UIManager.Instance.ResetButton();
            }
        }
    }

    public void DeselectBall()
    {
        lastSelected = null;
        actualBallLineRenderer = null;
        actualSelectedBall = null;

        StopShoot();
    }

    public void ClickOnBall(GameObject target)
    {

        OnBallClicked?.Invoke(target);

        if (target.CompareTag("Hologram"))
        {
            NewHologramSelected(target);
            print("hlo");
        }
        else
        {        
            if (target.GetComponent<BallSettings>().myteam == PhotonNetwork.LocalPlayer.GetTeam())
            {
                if (target != null)
                {
                    NewBallSelected(target);
                }
                else
                {
                    lastSelected = null;
                    actualBallLineRenderer = null;
                }
            }
        }
    }

    private void NewHologramSelected(GameObject holo)
    {
        UIManager.Instance.OnEndTurn();
        UIManager.Instance.OnClickOnBall(holo);
    }

    public void NewBallSelected(GameObject ball)
    {
        UIManager.Instance.OnEndTurn();
        UIManager.Instance.OnClickOnBall(ball);
        actualSelectedBall = ball;
        actualSelectedBallSettings = actualSelectedBall.GetComponent<BallSettings>();

        if (GetComponent<LineRenderer>() != null)
        {
            actualBallLineRenderer = GetComponent<LineRenderer>();
        }
    }

    // Mouse Elevation
    private void GetElevation()
    {
        mouseScrollDelta -= Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollDelta > scrollSensitivity)
        {
            angleIndex = Mathf.Clamp(angleIndex + 1, 0, possibleAngles.Length - 1);
            mouseScrollDelta = 0;
            //elevation = Mathf.Clamp(elevation + Input.GetAxis("Mouse ScrollWheel") * 10 * scrollSensitivity, 0.0f, 45.0f);
        }
        if (mouseScrollDelta < -scrollSensitivity)
        {
            angleIndex = Mathf.Clamp(angleIndex - 1, 0, possibleAngles.Length - 1);
            mouseScrollDelta = 0;
        }
        elevation = possibleAngles[angleIndex];
    }

    private void ElevateCamera()
    {
        mouseScrollDelta = Input.GetAxis("Mouse ScrollWheel")*camScrollSensitivity;
        camCoord = Mathf.Clamp(camCoord + mouseScrollDelta, 0, 1);
        if (camAnimCoord != camCoord)
            camAnimCoord = camAnimCoord + Mathf.Clamp(camCoord - camAnimCoord, -0.02f, 0.02f);
        cameraValues = Vector2.Lerp(minimalValues, maximalValues, camAnimCoord);
        CP.UpdateElevation(cameraValues);
    }

    // --->> MOUSE DRAG  --->> //
    private void MouseDrag()
    {
        if (actualSelectedBall != null && canShoot && !gameModeManager.isOnForceCam)
        {
            if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton2) && !isHoldingShoot) // Si PREMIER CLICK
            {
                actualSelectedBallSettings.InitChargeFx();
                isHoldingShoot = true;
                UIManager.Instance.isShooting = true;
                dragForce = 0;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (Input.GetKey(InputManager.Instance.Inputs.inputs.MainButton2) && isHoldingShoot) // HOLD
            {
                GetElevation();
                actualBallLineRenderer.transform.position = actualSelectedBall.transform.position;
                dragForce += -Input.GetAxis("Mouse Y") * (InputManager.Instance.Inputs.inputs.MouseSensitivity / 4); // generate dragForce --> la limite est dragForceMaxValue;
                dragForce = Mathf.Clamp(dragForce, 0, dragForceMaxValue);
                direction = mainCamera.transform.forward;
                DisplayLineRenderer();
                DisplayDragForce();
                actualSelectedBallSettings.UpdateChargeFx(dragForce, dragForceMaxValue);

                // Debug.DrawRay(transform.position, transform.position+new Vector3(direction.x * Mathf.Cos(Mathf.Deg2Rad * elevation), ((45 - elevation) / 45.0f + PhotonNetwork.CurrentRoom.GetLaunchPower() * 7.0f) * Mathf.Sin(Mathf.Deg2Rad * elevation) / (PhotonNetwork.CurrentRoom.GetLaunchPower() * 2.0f), direction.z * Mathf.Cos(Mathf.Deg2Rad * elevation)), Color.red,1.0f) ;
            }
            else if (Input.GetKeyUp(InputManager.Instance.Inputs.inputs.MainButton2) && isHoldingShoot)  // SI DERNIER CLICK
            {
                ShootBall();
            }

            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton1)) && isHoldingShoot)
            {
                StopShoot();
            }
        }
    }

    private void ShootBall()
    {
        if (dragForce > dragForceMaxValue * cancelForce)
        {
            actualSelectedBall.GetComponent<PUNBallMovement>().MoveBall(direction, elevation, dragForce);
            OnShooted();
            haveShoot = true;
        }

        StopShoot();
    }

    public void StopShoot()
    {
        if (actualSelectedBallSettings != null)
        {
            actualSelectedBallSettings.chargeFx.SetActive(false);
        }

        isHoldingShoot = false;
        if (actualBallLineRenderer != null)
            actualBallLineRenderer.enabled = false;
        angleIndex = 0;
        elevation = 0;
        dragForce = 0;
        DisplayDragForce();
        UIManager.Instance.isShooting = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void DisplayLineRenderer()
    {
        if (dragForce > dragForceMaxValue * cancelForce)
        {
            actualBallLineRenderer.enabled = true;
            dragPowerPrctg = ((dragForce * 100) / dragForceMaxValue) / 100;
            actualBallLineRenderer.material.color = lineGradient.Evaluate(dragPowerPrctg);
        }
        else
        {
            actualBallLineRenderer.enabled = false;
        }
    }

    private void DisplayDragForce()
    {
        dragForceBar.fillAmount = ((dragForce * 100) / dragForceMaxValue) / 100; // Génère un pourcentage 
    }


    public void DisableShootInTime()
    {
        if (!isStoppingShoot)
        {
            StartCoroutine(WaitForShoot());
        }
    }

    IEnumerator WaitForShoot()
    {
        isStoppingShoot = true;
        canShoot = false;
        yield return new WaitForSeconds(timeBeforeShoot);
        canShoot = true;
        isStoppingShoot = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        StopShoot();
    }

}
