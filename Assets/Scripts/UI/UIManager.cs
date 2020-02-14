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
using Photon.Realtime;

public class UIManager : MonoBehaviourPunCallbacks
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    public Image dragForceBar;
    public GameObject escapeMenu;
    public GameObject generalMenu;

    public string quitScene;
    public string settingsScene;

    public GameObject MainCamButton;
    public GameObject TopCamButton;
    public GameObject SpecCamButton;


    public Text pingText;

    public bool pingStatut = false;

    public TimerInfo timer;
    public TextMeshProUGUI round;

    public Action<CameraMode> OnTopCam;
    public Action<CameraMode> OnSpecCam;
    public Action<CameraMode> OnMainCam;
    public Action OnSetSavedCam;

    public GameObject FreeCamTooltip;
    public GameObject ChatTooltip;
    public GameObject PingTooltip;

    public GameObject WinPanel;
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
    public GameObject watchingPanel;

    [Header("Ui panel")]
    public GameObject loadingPanel;

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
        if (pingText != null)
        {
            pingText.text = "Ping : " + PhotonNetwork.GetPing() + "ms";
        }

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

        SpecCamButton.SetActive(!GameModeManager.Instance.localPlayerTurn);
    }

    public void DisplayEscapeMenu()
    {
        isEscapeMenuDisplayed = (isEscapeMenuDisplayed == false) ? true : false;
        escapeMenu.SetActive(isEscapeMenuDisplayed);
        generalMenu.SetActive(!isEscapeMenuDisplayed);

    }

    public void QuitGame()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnLeftRoom()
    {
        StartCoroutine(LoadScene());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ChatManager.Instance.OnChatMessage("<color=" + MarblGame.GetColorUI((int) otherPlayer.GetTeam()) + ">" + otherPlayer.NickName + "</color> has been disconnected !");
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(quitScene, LoadSceneMode.Single);
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
    }

    public void SetSavedCam()
    {
        OnSetSavedCam();
    }

    public void SetSpecCam()
    {
        ResetButton();

        SpecCamButton.GetComponent<Image>().color = Color.white;
        SpecCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);


        if (actualCommand != commandFocus)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandFocus;
            actualCommand.SetActive(true);
        }

        OnSpecCam(CameraMode.SpecMode);
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
        OnTopCam(CameraMode.Top);
        TopCamButton.GetComponent<Image>().color = Color.white;
        TopCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);
    }

    public void SetMainCam()
    {
        ResetButton();

        MainCamButton.GetComponent<Image>().color = Color.white;
        MainCamButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.109f, 0.109f, 0.109f);


        if (actualCommand != commandFocus)
        {
            if (actualCommand != null)
                actualCommand.SetActive(false);
            actualCommand = commandFocus;
            actualCommand.SetActive(true);
        }

        OnMainCam(CameraMode.Targeted);
    }

    public void ResetButton()
    {
        TopCamButton.GetComponent<Image>().color = new Color(0.109f, 0.109f, 0.109f);
        TopCamButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        SpecCamButton.GetComponent<Image>().color = new Color(0.109f, 0.109f, 0.109f);
        SpecCamButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        MainCamButton.GetComponent<Image>().color = new Color(0.109f, 0.109f, 0.109f);
        MainCamButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
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

        GameObject[] _Balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in _Balls)
        {
            GameObject newPing = Instantiate(pingPrefab);
            newPing.transform.position = ball.transform.position;
            newPing.transform.position += new Vector3(0, -0.09f, 0);
            PingElement myElement = newPing.GetComponent<PingElement>();
            BallSettings myBallSettings = ball.GetComponent<BallSettings>();

            myElement.SetColor(MarblGame.GetColor((int)myBallSettings.myteam));

            if (myBallSettings.myteam == GameModeManager.Instance.localPlayerTeam)
            {
                myBallSettings.enabled = true;
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
    }

    public void OnEndTurn()
    {
        if (!pingStatut) { return; }

        if (actualCommand != null)
        {
            actualCommand.SetActive(false);
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
            element.Value.transform.position += new Vector3(0, -0.09f, 0);
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
    public void DisplayChatTooltip(bool value)
    {
        if (ChatTooltip != null)
        {
            ChatTooltip.GetComponentInChildren<Text>().text = "Enter"; // to assign
            ChatTooltip.SetActive(value);
        }
    }
    public void DisplayPingTooltip(bool value)
    {
        if (PingTooltip != null)
        {
            PingTooltip.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.Ping.ToString();
            PingTooltip.SetActive(value);
        }
    }

    public void DisplayInfoTurn(string playerName, int playerTeam)
    {
        string result = "<color=" + MarblGame.GetColorUI(playerTeam) + ">";

        if (PhotonNetwork.LocalPlayer.GetPlayerTurnState())
        {
            watchingPanel.SetActive(false);
            infoTurnSettings.text.text = "Your turn to play";
            DisplayChatTooltip(false);
            DisplayPingTooltip(false);
            result += "Your turn ...";
            infoTurnSettings.MainBackground.GetComponent<Image>().color = MarblGame.GetColor(playerTeam);
            infoTurnSettings.BackgroundGo.GetComponent<Animator>().SetTrigger("Display");
        }
        else
        {
            SetWatchingPanel(playerName, playerTeam);
            watchingPanel.SetActive(true);
            DisplayChatTooltip(true);
            DisplayPingTooltip(true);
            result += playerName + " playing";
        }
    }

    public void EndGame(Team winner)
    {
        WinPanel.SetActive(true);

        if (WinPanel.GetComponent<Image>() != null)
        {
            WinPanel.GetComponent<Image>().color = MarblGame.GetColor((int)winner);
        }

        if (WinPanel.GetComponentInChildren<Text>() != null)
        {
            WinPanel.GetComponentInChildren<Text>().text = winner + " Team WIN";
        }    
    }

    public void SetWatchingPanel(string playerName, int playerTeam)
    {
        WatchingPanel wp = watchingPanel.GetComponent<WatchingPanel>();
        wp.teamText.text = MarblGame.GetTeamString(playerTeam);
        wp.teamText.color = MarblGame.GetColor(playerTeam);
        wp.nicknameText.text = playerName;
        wp.nicknameText.color = MarblGame.GetColor(playerTeam);
        wp.ballNumberText.text = QuickScoreboard.Instance.CountBallOfThisTeam((Team)playerTeam).ToString();
    }
}
