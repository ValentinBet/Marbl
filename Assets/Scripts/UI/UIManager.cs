using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MarblGame;

public class UIManager : MonoBehaviourPunCallbacks
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    public Image dragForceBar;
    public GameObject escapeMenu;
    public GameObject generalMenu;

    public string quitScene;
    public string settingsScene;

    public GameObject TopCamButton;
    public GameObject TeamCamButton;
    public GameObject FreeCamButton;

    public TimerInfo timer;
    public Text round;

    public Action<CameraMode> OnTopCam;
    public Action<CameraMode> OnTeamCam;
    public Action<CameraMode> OnFreeCam;

    public bool isShooting = false;

    private bool isEscapeMenuDisplayed = false;

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

    private void Update()
    {
        DisplayEscapeMenu();

        if (isShooting)
        {
            SetCamButtonState(false);
        } else
        {
            SetCamButtonState(true);
        }
    }

    public void DisplayEscapeMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEscapeMenuDisplayed = (isEscapeMenuDisplayed == false) ? true : false;

            escapeMenu.SetActive(isEscapeMenuDisplayed);
            generalMenu.SetActive(!isEscapeMenuDisplayed);
        }
    }

    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(quitScene);
    }

    public void DisplaySettings()
    {
        SceneManager.LoadSceneAsync(settingsScene, LoadSceneMode.Additive);
    }

    public void SetCamButtonState(bool value)
    {
        TopCamButton.SetActive(value);
        TeamCamButton.SetActive(value);
        FreeCamButton.SetActive(value);
    }

    public void SetFreeCam()
    {
        OnFreeCam(CameraMode.Free);
    }

    public void SetTeamCam()
    {
        OnTeamCam(CameraMode.TeamCentered);
    }

    public void SetTopCam()
    {
        OnTopCam(CameraMode.MapCentered);
    }
}
