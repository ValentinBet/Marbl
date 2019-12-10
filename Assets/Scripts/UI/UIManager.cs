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
    public GameObject PingCamButton;
    bool pingStatut = false;

    public TimerInfo timer;
    public Text round;

    public Action<CameraMode> OnTopCam;
    public Action<CameraMode> OnTeamCam;
    public Action<CameraMode> OnFreeCam;

    public GameObject LoadingPanel;

    public bool isShooting = false;

    private bool isEscapeMenuDisplayed = false;

    Dictionary<GameObject, GameObject> listOfPing = new Dictionary<GameObject, GameObject>();
    public GameObject pingPrefab;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisplayEscapeMenu();
        }
        if (isShooting)
        {
            SetCamButtonState(false);
        }
        else
        {
            SetCamButtonState(true);
        }

        if (pingStatut)
        {
            FollowMarbl();
        }
    }

    public void DisplayEscapeMenu()
    {
        isEscapeMenuDisplayed = (isEscapeMenuDisplayed == false) ? true : false;
        escapeMenu.SetActive(isEscapeMenuDisplayed);
        generalMenu.SetActive(!isEscapeMenuDisplayed);

    }

    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        // SceneManager.LoadScene(quitScene);
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
        ResetButton();
        OnFreeCam(CameraMode.Free);
        FreeCamButton.GetComponent<Image>().color = Color.white;
        FreeCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);

    }

    public void SetTeamCam()
    {
        ResetButton();
        OnTeamCam(CameraMode.TeamCentered);
        TeamCamButton.GetComponent<Image>().color = Color.white;
        TeamCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);
    }

    public void SetTopCam()
    {
        ResetButton();
        OnTopCam(CameraMode.MapCentered);
        TopCamButton.GetComponent<Image>().color = Color.white;
        TopCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);
    }

    public void ResetButton()
    {
        FreeCamButton.GetComponent<Image>().color = new Color(0.109f, 0.109f, 0.109f);
        FreeCamButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        TeamCamButton.GetComponent<Image>().color = new Color(0.109f, 0.109f, 0.109f);
        TeamCamButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        TopCamButton.GetComponent<Image>().color = new Color(0.109f, 0.109f, 0.109f);
        TopCamButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
    }

    public void SetPingMap()
    {
        if (pingStatut)
        {
            DisablePing();
        }
        else
        {
            EnablePing();
        }
        pingStatut = !pingStatut;
    }

    public void DisablePing()
    {
        PingCamButton.GetComponent<Image>().color = new Color(0.109f, 0.109f, 0.109f); ;
        PingCamButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in _Balls)
        {
            ball.GetComponent<MarbleIndicator>().enabled = false;
        }

        foreach(KeyValuePair<GameObject, GameObject> element in listOfPing)
        {
            Destroy(element.Key);
        }

        listOfPing.Clear();
    }

    public void EnablePing()
    {
        listOfPing.Clear();

        PingCamButton.GetComponent<Image>().color = Color.white;
        PingCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);

        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in _Balls)
        {
            GameObject newPing = Instantiate(pingPrefab);
            newPing.transform.position = ball.transform.position;
            newPing.transform.position += new Vector3(0, -0.4f, 0);
            newPing.GetComponent<PingElement>().SetColor( MarblGame.GetColor((int) ball.GetComponent<BallSettings>().myteam));

            if (ball.GetComponent<BallSettings>().myteam == DeathMatchManager.Instance.localPlayerTeam)
            {
                ball.GetComponent<MarbleIndicator>().enabled = true;
            }
            else
            {
                newPing.GetComponent<PingElement>().Hide();
            }

            listOfPing.Add(newPing, ball);
        }
    }

    void FollowMarbl()
    {
        foreach(KeyValuePair < GameObject, GameObject > element in listOfPing)
        {
            element.Key.transform.position = element.Value.transform.position;
            element.Key.transform.position += new Vector3(0, -0.4f, 0);
        }
    }
}
