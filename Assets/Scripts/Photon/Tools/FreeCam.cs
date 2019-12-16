﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float mainSpeed = 100.0f; //regular speed
    public float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    public float maxShift = 1000.0f; //Maximum speed when holdin gshift
    public float camSens = 3.0f; //How sensitive it with mouse
    public bool rotateOnlyIfMousedown = true;
    public bool movementStaysFlat = true;
    public bool onFreeCam = false;

    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    float lastX = 0.0f;

    void Update()
    {
        if (!onFreeCam)
        {

            UIManager.Instance.DisplayFreeCamTooltip(false);

            return;
        }

        UIManager.Instance.DisplayFreeCamTooltip(true);

        if (Input.GetMouseButtonDown(1))
        {
            lastMouse = Input.mousePosition; // $CTK reset when we begin
        }

        if (!rotateOnlyIfMousedown ||
            (rotateOnlyIfMousedown && Input.GetMouseButton(1)) )
        {
            //lastMouse = Input.mousePosition - lastMouse;
            //lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(Mathf.Clamp(lastX+ -Input.GetAxis("Mouse Y") * camSens, -90.0f, 90.0f), transform.eulerAngles.y + Input.GetAxis("Mouse X") * camSens, 0);
            lastX = lastMouse.x;
            transform.eulerAngles = lastMouse;
            //lastMouse = Input.mousePosition;
            //Mouse  camera angle done.  
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

        //Keyboard commands
        Vector3 p = GetBaseInput();
        if (Input.GetKey(InputManager.Instance.Inputs.inputs.CameraSpeed))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (movementStaysFlat && !(rotateOnlyIfMousedown && Input.GetMouseButton(1)))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(InputManager.Instance.Inputs.inputs.CameraForward))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(InputManager.Instance.Inputs.inputs.CameraBackward))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(InputManager.Instance.Inputs.inputs.CameraLeft))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(InputManager.Instance.Inputs.inputs.CameraRight))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
