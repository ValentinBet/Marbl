﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PUNMouseControl : MonoBehaviour
{
    public UnityAction<GameObject> OnBallClicked;
    public float dragForce;
    public float orientation;

    public UnityAction OnShooted;
    public GameObject actualSelectedBall = null;
    public bool haveShoot = false;
    public float scrollSensivity = 0.1f;
    public float[] possibleAngles;


    [SerializeField]
    private LineRenderer actualBallLineRenderer;

    [Header("Raycast Variables")]
    [Space(5)]

    private Ray ray;
    private RaycastHit hit;
    private PhotonView photonView;
    private GameObject lastSelected;
    private LocalPlayerManager player;
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

    public int angleIndex = 0;
    private float mouseScrollDelta;
    public bool isHoldingShoot;

    private void Start()
    {
        player = GetComponent<LocalPlayerManager>();
        photonView = GetComponent<PhotonView>();
        mainCamera = Camera.main;
        dragForceBar = UIManager.Instance.dragForceBar;
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
            GetOrientation();
            MouseDrag();
        }
    }

    private void DetectClick()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.collider.CompareTag("Ball"))
                    {
                        StopShoot();
                        ClickOnBall(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    public void ClickOnBall(GameObject target)
    {
        if (player.teamBalls.Contains(target))
        {
            if (target != null)
            {
                NewBallSelected(target);
            }
            else
            {
                lastSelected = null;
                ResetBallSelect();
                actualBallLineRenderer = null;
            }
        }
    }

    public void NewBallSelected(GameObject ball)
    {
        OnBallClicked?.Invoke(ball);

        actualSelectedBall = ball;
        ControlOutline(ball);

        if (GetComponent<LineRenderer>() != null)
        {
            actualBallLineRenderer = GetComponent<LineRenderer>();
        }
    }
    public void ResetBallSelect()
    {
        if (actualSelectedBall != null)
        {
            actualSelectedBall.GetComponent<BallControl>().SetOutline(false);
        }
        actualSelectedBall = null;
    }

    private void ControlOutline(GameObject ball)
    {
        if (ball.GetComponent<PUNBallControl>() != null)
        {
            PUNBallControl _ballControl = ball.GetComponent<PUNBallControl>();
            _ballControl.SetOutline(true);

            foreach (GameObject b in player.teamBalls)
            {
                if (b == null)
                {
                    continue;
                }

                if (b != ball)
                {
                    b.GetComponent<PUNBallControl>().SetOutline(false);
                }
            }
        }
    }

    // Mouse Orientation
    private void GetOrientation()
    {
        mouseScrollDelta += Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollDelta > scrollSensivity)
        {
            angleIndex = Mathf.Clamp(angleIndex+1, 0, possibleAngles.Length-1);
            mouseScrollDelta = 0;
            //orientation = Mathf.Clamp(orientation + Input.GetAxis("Mouse ScrollWheel") * 10 * scrollSensivity, 0.0f, 45.0f);
        }
        if (mouseScrollDelta < -scrollSensivity)
        {
            angleIndex = Mathf.Clamp(angleIndex-1,0,possibleAngles.Length-1);
            mouseScrollDelta = 0;
        }
        orientation = possibleAngles[angleIndex];
    }

    // --->> MOUSE DRAG  --->> //
    private void MouseDrag()
    {
        if (actualSelectedBall != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject()  )
            {
                if (Input.GetKeyDown(InputManager.Instance.Inputs.inputs.MainButton2) && !isHoldingShoot) // Si PREMIER CLICK
                {
                    isHoldingShoot = true;
                    UIManager.Instance.isShooting = true;
                    actualBallLineRenderer.transform.position = actualSelectedBall.transform.position;
                    dragForce = 0;
                    Cursor.visible = false;
                }

                if (Input.GetKey(InputManager.Instance.Inputs.inputs.MainButton2) && isHoldingShoot) // HOLD
                {
                    dragForce += -Input.GetAxis("Mouse Y") * (InputManager.Instance.Inputs.inputs.MouseSensitivity / 4); // generate dragForce --> la limite est dragForceMaxValue;
                    dragForce = Mathf.Clamp(dragForce, 0, dragForceMaxValue);
                    direction = mainCamera.transform.forward;
                    DisplayLineRenderer();
                    DisplayDragForce();
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
    }

    private void ShootBall()
    {

        if (dragForce > dragForceMaxValue * cancelForce)
        {
            actualSelectedBall.GetComponent<PUNBallMovement>().MoveBall(direction, orientation, dragForce);
            OnShooted();
            haveShoot = true;
            StopShoot();
        }
        else
        {
            StopShoot();
        }
    }

    private void StopShoot()
    {
        isHoldingShoot = false;
        Cursor.visible = true;
        if (actualBallLineRenderer != null)
            actualBallLineRenderer.enabled = false;

        orientation = 0;
        dragForce = 0;
        DisplayDragForce();

        UIManager.Instance.isShooting = false;

    }

    private void DisplayLineRenderer()
    {
        if (dragForce > dragForceMaxValue * cancelForce)
        {
            actualBallLineRenderer.enabled = true;
        } else
        {
            actualBallLineRenderer.enabled = false;
        }
    }

    private void DisplayDragForce()
    {
        dragForceBar.fillAmount = ((dragForce * 100) / dragForceMaxValue) / 100; // Génère un pourcentage 
    }
}
