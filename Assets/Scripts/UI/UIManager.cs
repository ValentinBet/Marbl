using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using static MarblGame;
using static Photon.Pun.UtilityScripts.PunTeams;

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

    public Text pingText;

    public bool pingStatut = false;

    public TimerInfo timer;
    public TextMeshProUGUI round;

    public Action<CameraMode> OnTopCam;
    public Action<CameraMode> OnTeamCam;
    public Action<CameraMode> OnFreeCam;

    public GameObject FreeCamTooltip;

    public GameObject LoadingPanel;
    public GameObject chat;
    public bl_ChatUI chatUI;
    public InfoTurnSettings infoTurnSettings;

    public bool isShooting = false;

    private bool isEscapeMenuDisplayed = false;

    Dictionary<BallSettings, PingElement> listOfPing = new Dictionary<BallSettings, PingElement>();

    public GameObject pingPrefab;
    public GameObject PingChoice;
    public GameObject currentClickedBall;

    private GameObject actualCommand;
    public GameObject commandNoButtons;
    public GameObject commandAim;
    public GameObject commandFocus;
    public GameObject commandShoot;

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

        LoadingPanel.SetActive(true);
    }

    private void Update()
    {
        if (pingText != null)
        {
            pingText.text = "Ping : " + PhotonNetwork.GetPing() + "ms";
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisplayEscapeMenu();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DisplayChat();
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
        SceneManager.LoadScene(quitScene);
    }

    public override void OnLeftRoom()
    {
        //SceneManager.LoadScene(quitScene);
    }

    public void DisplaySettings()
    {
        SceneManager.LoadSceneAsync(settingsScene, LoadSceneMode.Additive);
    }

    public void SetCamButtonState(bool value)
    {
        TopCamButton.SetActive(value);
        if (value == false)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandShoot;
            actualCommand.SetActive(true);
        }
        //TeamCamButton.SetActive(value);
        FreeCamButton.SetActive(value);
    }

    public void SetFreeCam()
    {
        ResetButton();
        if (actualCommand != commandFocus)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandFocus;
            actualCommand.SetActive(true);
        }
        OnFreeCam(CameraMode.Free);
        FreeCamButton.GetComponent<Image>().color = Color.white;
        FreeCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);

    }

    public void SetTeamCam()
    {
        ResetButton();
        if (actualCommand != commandFocus)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandFocus;
            actualCommand.SetActive(true);
        }
        OnTeamCam(CameraMode.TeamCentered);
        TeamCamButton.GetComponent<Image>().color = Color.white;
        TeamCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);
    }

    public void SetTopCam()
    {
        ResetButton();
        if (actualCommand != commandFocus)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandFocus;
            actualCommand.SetActive(true);
        }
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

        foreach (KeyValuePair<BallSettings, PingElement> element in listOfPing)
        {
            Destroy(element.Value.gameObject);
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
            PingElement myElement = newPing.GetComponent<PingElement>();
            BallSettings myBallSettings = ball.GetComponent<BallSettings>();

            myElement.SetColor(MarblGame.GetColor((int)myBallSettings.myteam));

            if (myBallSettings.myteam == GameModeManager.Instance.localPlayerTeam)
            {
                myBallSettings.enabled = true;
            }
            else
            {
                newPing.GetComponent<PingElement>().Hide();
            }

            listOfPing.Add(myBallSettings, myElement);
        }
    }

    public void StartAim()
    {
        if (actualCommand != commandAim)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandAim;
            actualCommand.SetActive(true);
        }
    }

    public void ReleaseAim()
    {
        if (actualCommand != commandNoButtons)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandNoButtons;
            actualCommand.SetActive(true);
        }
    }
    public void OnClickOnBall(GameObject ball)
    {
        if (!pingStatut) { return; }
        /*foreach (KeyValuePair<BallSettings, PingElement> element in listOfPing)
        {
            if (element.Key == null) { continue; }

            if (element.Key.myteam == GameModeManager.Instance.localPlayerTeam)
            {
                element.Value.Trail.enabled = true;
            }
        }*/

        if (actualCommand != commandNoButtons)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandNoButtons;
            actualCommand.SetActive(true);
        }

        currentClickedBall = ball;

        foreach (KeyValuePair<BallSettings, PingElement> element in listOfPing)
        {
            if (element.Key == null) { continue; }

            if (element.Key.gameObject == ball)
            {
                element.Value.Trail.enabled = false;
            }
        }
    }

    public void OnEndTurn()
    {
        if (!pingStatut) { return; }
        if (actualCommand != null)
            actualCommand.SetActive(false);
        foreach (KeyValuePair<BallSettings, PingElement> element in listOfPing)
        {
            if (element.Key == null) { continue; }

            if (element.Key.myteam == GameModeManager.Instance.localPlayerTeam)
            {
                element.Value.Trail.enabled = true;
            }
        }
    }

    void FollowMarbl()
    {
        if (listOfPing.Count == 0)
        {
            EnablePing();
        }

        List<BallSettings> deleteBall = new List<BallSettings>();

        foreach (KeyValuePair<BallSettings, PingElement> element in listOfPing)
        {
            if (element.Key == null)
            {
                element.Value.gameObject.SetActive(false);
                deleteBall.Add(element.Key);
                continue;
            }

            element.Value.transform.position = element.Key.transform.position;
            element.Value.transform.position += new Vector3(0, -0.4f, 0);
            element.Value.SetColor(MarblGame.GetColor((int)element.Key.myteam));
        }

        foreach (BallSettings element in deleteBall)
        {
            listOfPing.Remove(element);
        }
    }

    public void DisplayFreeCamTooltip(bool value)
    {

        if (FreeCamTooltip != null)
        {
            FreeCamTooltip.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.CameraSpeed.ToString();
            FreeCamTooltip.SetActive(value);
        }
    }

    public void DisplayInfoTurn(string playerName, int playerTeam)
    {
        if (PhotonNetwork.LocalPlayer.GetPlayerTurnState())
        {
            infoTurnSettings.text.text = "Your turn";
        }
        else
        {
            infoTurnSettings.text.text = playerName + "'s Turn";
        }

        infoTurnSettings.MainBackground.GetComponent<Image>().color = MarblGame.GetColor(playerTeam);
        infoTurnSettings.BackgroundGo.GetComponent<Animator>().SetTrigger("Display");
    }

    public void DisableBecon()
    {
        foreach (KeyValuePair<BallSettings, PingElement> element in listOfPing)
        {
            if (element.Key != null)
            {
                element.Value.Trail.enabled = false;
                continue;
            }
        }
    }

    public void DisplayChat()
    {
        chatUI.OnChatDisplay();

        if (!chat.activeInHierarchy)
        {
            chat.SetActive(true);
        } else
        {
            chat.SetActive(false);
        }
    }
}
