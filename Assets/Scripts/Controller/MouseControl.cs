using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MarblGame;

public class MouseControl : MonoBehaviour
{

#pragma warning disable IDE0044 // Add readonly modifier

    [SerializeField]
    private PlayerControl player;

    [Header("Raycast Variables")]
    [Space(5)]

    private Ray ray;
    private RaycastHit hit;

    [Header("Mouse Drag Variables")]
    [Space(5)]

    private bool isMouseHold;

    private Vector3 initDragPosition;
    private Vector3 lastDragPosition;
    private Vector3 lineRendererOriginPosition;
    private Vector3 lineRendererEndPosition;
    private Vector3 direction;

    private float dragForce;
    public static float dragPublic;

    [SerializeField]
    private float cancelForce = 0.08f;

    [SerializeField]
    private float dragLineDepth = 10;
    [SerializeField]
    private float dragForceMaxValue = 7;
    [SerializeField]
    private LineRenderer actualBallLineRenderer;

    [SerializeField]
    private TrajectoryRenderer curveRenderer;

    public Image dragForceBar;

    private bool turnLock = false;

    public CameraAverage cameraScript;

    private GameObject lastSelected;

    public static MouseControl singleton;

    public float[] possibleAngles;
    private int angleIndex = 0;
    private float mouseScrollDelta;
    public static float elevation;
    public float sensibility;

    public bool enableAngleShot = false;


#pragma warning restore IDE0044 // Add readonly modifier

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
    }

    void Update()
    {

        DetectClick();
        MouseDrag();
        GetElevation();
    }

    private void DetectClick()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    ClickOnPlayer(hit.collider.gameObject);
                }
            }
        }
    }

    private void ClickOnPlayer()
    {
        if (hit.collider.gameObject.GetComponent<TurnBasedItem>().IsSelectable() && hit.collider.gameObject != lastSelected)
        {
            lastSelected = hit.collider.gameObject;
            player.NewBallSelected(hit.collider.gameObject);
            cameraScript.SetCameraMode(CameraMode.Targeted, hit.collider.GetComponent<TurnBasedItem>().GetID(), hit.collider.GetComponent<DirtyID>().myID);
        }
        if (!enableAngleShot)
        {
            actualBallLineRenderer = player.actualSelectedBall.GetComponent<LineRenderer>();
        }
        else
        {
            //curveRenderer.setNewTarget(hit.collider.gameObject);
        }
    }

    public void ClickOnPlayer(GameObject target)
    {
        if (target != null)
        {
            if (target.GetComponent<TurnBasedItem>().IsSelectable() && target != lastSelected)
            {
                lastSelected = target;
                player.NewBallSelected(target);
                cameraScript.SetCameraMode(CameraMode.Targeted, target.GetComponent<TurnBasedItem>().GetID(), target.GetComponent<DirtyID>().myID);
            }
            if (!enableAngleShot)
            {
                actualBallLineRenderer = player.actualSelectedBall.GetComponent<LineRenderer>();
            }
            else
            {
                //curveRenderer.setNewTarget(target);
            }
        }
        else
        {
            lastSelected = null;
            player.ResetBallSelect();
            actualBallLineRenderer = null;
        }
    }

    public static void ManualTarget(GameObject _newTarget)
    {
        singleton.ClickOnPlayer(_newTarget);
    }

    public static void NewTurn()
    {
        singleton.NewTurnSequence();
    }

    private void NewTurnSequence()
    {        
        player.ResetBallSelect();
        actualBallLineRenderer = null;
        turnLock = false;

        UIManager.Instance.DisplayInfoTurn();
    }

    // Mouse elevation
    private void GetElevation()
    {
        mouseScrollDelta += Input.GetAxis("Mouse ScrollWheel");
        if(mouseScrollDelta > sensibility)
        {
            angleIndex = (angleIndex+1)%possibleAngles.Length;
            mouseScrollDelta = 0;
            //elevation = Mathf.Clamp(elevation + Input.GetAxis("Mouse ScrollWheel") * 10 * sensibility, 0.0f, 45.0f);
        }
        if (mouseScrollDelta < - sensibility)
        {
            angleIndex--;
            if (angleIndex < 0)
                angleIndex = possibleAngles.Length - 1;
            mouseScrollDelta = 0;
        }
        elevation = possibleAngles[angleIndex];
    }


    // --->> MOUSE DRAG  --->> //

    private void MouseDrag()
    {
        if (player.actualSelectedBall != null && !turnLock)
        {
            if (Input.GetMouseButton(1))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (isMouseHold) // HOLD
                {
                    lastDragPosition = GetMouseCameraPoint();
                    dragForce = Vector3.Distance(lastDragPosition, initDragPosition); // generate dragForce --> la limite est dragForceMaxValue;
                    dragForce = Mathf.Clamp(dragForce, 0, dragForceMaxValue);
                    dragPublic = dragForce;
                    direction = Camera.main.transform.forward;
                    DisplayDragForce();
                    DisplayAimLaser();
                }
                if (!isMouseHold) // SI PREMIER CLICK
                {
                    initDragPosition = GetMouseCameraPoint();
                    isMouseHold = true;
                }
            }
            else if (Input.GetMouseButtonUp(1) && isMouseHold)  // SI DERNIER CLICK
            {
                if (dragForce > dragForceMaxValue * cancelForce)
                {
                    if (actualBallLineRenderer != null)
                        actualBallLineRenderer.enabled = false;
                    isMouseHold = false;
                    player.actualSelectedBall.GetComponent<BallMovements>().MoveBall(direction, elevation, dragForce);
                    turnLock = true;
                    TurnManager.NextTurn();
                    dragForce = 0;
                    elevation = 0;
                }
                else
                {
                    if (actualBallLineRenderer != null)
                        actualBallLineRenderer.enabled = false;
                    isMouseHold = false;
                    dragForce = 0;
                    DisplayDragForce();
                }
            }
        }
    }
    private Vector3 GetMouseCameraPoint()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * dragLineDepth;
    }

    private void DisplayAimLaser()
    {
        if (!enableAngleShot)
        {
            lineRendererOriginPosition = new Vector3(player.actualSelectedBall.transform.position.x, 0.5f, player.actualSelectedBall.transform.position.z);
            lineRendererEndPosition = (new Vector3(Camera.main.transform.forward.x * dragForce, 0, Camera.main.transform.forward.z * dragForce) + lineRendererOriginPosition);
            actualBallLineRenderer.SetPositions(new Vector3[] { lineRendererOriginPosition, lineRendererEndPosition });
            if (!actualBallLineRenderer.enabled)
            {
                actualBallLineRenderer.enabled = true;
            }
        }
        else
        {
            curveRenderer.force = dragForce;
        }
    }

    private void DisplayDragForce()
    {
        dragForceBar.fillAmount = ((dragForce * 100) / dragForceMaxValue) / 100; // Génère un pourcentage 
    }

    // <<--- MOUSE DRAG  <<--- //

}
