using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static readonly FullScreenMode[] Windowmodes = new FullScreenMode[4] { FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.MaximizedWindow, FullScreenMode.Windowed };

    [Header("General")]
    public GameObject videoPanel;
    public GameObject audioPanel;
    public GameObject controlsPanel;
    public Animator thisSceneAnim;

    [Header("Video")]
    public Dropdown windowedDropdown;
    public Dropdown resDropdown;
    public Dropdown qualityDropdown;
    public List<Vector2Int> ResolutionsList = new List<Vector2Int>();

    private bool resExist = false;
    private int resPlace;

    [Header("Audio")]
    public Slider generalVolumeSlider;
    public InputField generalVolumeInputField;

    [Header("Controls")]

    public Slider mouseSensitivitySlider;
    public InputField mouseSensitivityInputField;
    public List<KeyButtonParameters> keyButtonList = new List<KeyButtonParameters>();

    private bool isChangingKey;
    private KeyButtonParameters buttonParameters;
    private KeyCode lastKeyPressed;

    [Header("Settings data")]
    public SettingsList settingsList;

    private SettingsSaves settingsSaves = new SettingsSaves();
    private string filename;

    private InputManager inputManager;

    private void Start()
    {
        filename = Application.persistentDataPath + "Settings" + ".json";
        inputManager = InputManager.Instance;
        InitSettingsList();

        InitResDropdown();
        InitQualityDropdown();

        InitVisuals();
    }

    private void Update()
    {
        if (isChangingKey)
        {
            ChangeKey(buttonParameters);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitSettingsScene();
        }

    }

    void OnGUI()
    {
        if (isChangingKey)
        {
            // A changer
            if (Input.GetKeyDown(KeyCode.Mouse0))
                lastKeyPressed = KeyCode.Mouse0;
            if (Input.GetKeyDown(KeyCode.Mouse1))
                lastKeyPressed = KeyCode.Mouse1;
            if (Input.GetKeyDown(KeyCode.Mouse2))
                lastKeyPressed = KeyCode.Mouse2;
            if (Input.GetKeyDown(KeyCode.LeftShift))
                lastKeyPressed = KeyCode.LeftShift;
            if (Input.GetKeyDown(KeyCode.RightShift))
                lastKeyPressed = KeyCode.RightShift;

            Event e = Event.current;

            if (e.isKey)
            {
                lastKeyPressed = e.keyCode;
            }
        }
    }

    // GENERAL >>
    public void DisplayVideoPanel()
    {
        DisableAllPanel();
        videoPanel.SetActive(true);
    }

    public void DisplayAudioPanel()
    {
        DisableAllPanel();
        audioPanel.SetActive(true);
    }

    public void DisplayControlsPanel()
    {
        DisableAllPanel();
        controlsPanel.SetActive(true);
    }

    private void DisableAllPanel()
    {
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void InitVisuals()
    {
        InitVideoVisuals();
        InitAudioVisuals();
        InitControlsVisuals();
    }

    private void InitSettingsList()
    {
        settingsList.settings.Quality = QualitySettings.GetQualityLevel();
        settingsList.settings.Windowmode = (int)Screen.fullScreenMode;
    }

    private void InitVideoVisuals()
    {
        windowedDropdown.value = settingsList.settings.Windowmode;
        resDropdown.value = resPlace;
        qualityDropdown.value = settingsList.settings.Quality;
    }


    public void RevertChanges()
    {
        InitVisuals();
        MakeChanges();
    }

    public void ApplyChanges()
    {
        // Save settings >>
        settingsList.settings.Windowmode = windowedDropdown.value;

        resPlace = resDropdown.value;
        settingsList.settings.Resolution = resPlace;

        settingsList.settings.Quality = qualityDropdown.value;

        settingsList.settings.GeneralVolume = generalVolumeSlider.value;
        settingsList.settings.MouseSensitivity = mouseSensitivitySlider.value;
        // <<

        inputManager.Inputs.inputs.MouseSensitivity = mouseSensitivitySlider.value;
        inputManager.Inputs.inputs.GeneralVolume = generalVolumeSlider.value / 100;

        foreach (KeyButtonParameters keyButtonParameters in keyButtonList)
        {
            ApplyKeyChange(keyButtonParameters);
        }
        SaveAsJson();
        MakeChanges();
        InitSettingsList();
    }

    private void MakeChanges()
    {
        Screen.SetResolution(ResolutionsList[resDropdown.value].x, ResolutionsList[resDropdown.value].y, Windowmodes[settingsList.settings.Windowmode]);
        QualitySettings.SetQualityLevel(settingsList.settings.Quality);
        AudioListener.volume = inputManager.Inputs.inputs.GeneralVolume;
    }
    public void SaveAsJson()
    {
        SettingsSaves settingsSaves = new SettingsSaves();
        settingsSaves.GeneralVolume = generalVolumeSlider.value / 100;
        string json = JsonUtility.ToJson(settingsSaves);
        File.WriteAllText(filename, json);
    }

    public void QuitSettingsScene()
    {
        thisSceneAnim.Play("Settings Close");
    }
    public void QuitScene()
    {
        SceneManager.UnloadSceneAsync("Settings");
    }

    // <<

    // VIDEO SETTINGS >>

    private void InitResDropdown()
    {
        for (int x = 0; x < ResolutionsList.Count; x++)
        {
            resDropdown.options.Add(new Dropdown.OptionData() { text = ResolutionsList[x].x + " * " + ResolutionsList[x].y });

            if (ResolutionsList[x].x == Screen.width && ResolutionsList[x].y == Screen.height)
            {
                resExist = true;
                resPlace = x;
            }
        }

        if (!resExist)
        {
            resDropdown.options.Add(new Dropdown.OptionData() { text = Screen.width + " * " + Screen.height });
            ResolutionsList.Add(new Vector2Int(Screen.width, Screen.height));
            resPlace = resDropdown.options.Count;
        }

        resDropdown.RefreshShownValue();
    }

    private void InitQualityDropdown()
    {
        for (int x = 0; x < QualitySettings.names.Length; x++)
        {
            qualityDropdown.options.Add(new Dropdown.OptionData() { text = QualitySettings.names[x] });
        }

        qualityDropdown.RefreshShownValue();
    }

    // <<

    // AUDIO SETTINGS >>

    public void InitAudioVisuals()
    {
        using (StreamReader r = new StreamReader(filename))
        {
            var dataAsJson = r.ReadToEnd();
            settingsSaves = JsonUtility.FromJson<SettingsSaves>(dataAsJson);
        }

        generalVolumeInputField.text = (settingsSaves.GeneralVolume * 100).ToString();
        generalVolumeSlider.value = settingsSaves.GeneralVolume * 100;
    }

    public void OnGeneralVolumeSliderUpdate()
    {
        generalVolumeInputField.text = (generalVolumeSlider.value).ToString();
    }

    public void OnGeneralVolumeInputFieldUpdated()
    {
        if (float.Parse(generalVolumeInputField.text) > 100)
        {
            generalVolumeInputField.text = "100";
        }
        else if (float.Parse(generalVolumeInputField.text) < 0)
        {
            generalVolumeInputField.text = "0";
        }

        generalVolumeSlider.value = float.Parse(generalVolumeInputField.text);
    }

    // <<

    // CONTROLS SETTINGS >>

    public void InitControlsVisuals()
    {
        mouseSensitivityInputField.text = inputManager.Inputs.inputs.MouseSensitivity.ToString();
        mouseSensitivitySlider.value = inputManager.Inputs.inputs.MouseSensitivity;

        InitKeyVisuals();
    }

    public void OnSensitivitySliderUpdate()
    {
        mouseSensitivityInputField.text = (mouseSensitivitySlider.value).ToString();

    }

    public void OnSensitivityInputFieldUpdated()
    {
        if (float.Parse(mouseSensitivityInputField.text) > 10)
        {
            mouseSensitivityInputField.text = "10";
        }
        else if (float.Parse(mouseSensitivityInputField.text) < 0)
        {
            mouseSensitivityInputField.text = "0";
        }
        mouseSensitivitySlider.value = float.Parse(mouseSensitivityInputField.text);
    }

    public void InitKeyVisuals()
    {
        string _temp = "";

        foreach (KeyButtonParameters keyButtonParameters in keyButtonList)
        {

            switch (keyButtonParameters.keyName)
            {
                case "MainButton1":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.MainButton1);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.MainButton1;
                    break;
                case "MainButton2":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.MainButton2);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.MainButton2;
                    break;
                case "Leaderboard":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.Learderboard);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.Learderboard;
                    break;
                case "Ping":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.Ping);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.Ping;
                    break;
                case "Chat":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.Chat);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.Chat;
                    break;
                case "FollowCam":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.FollowCam);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.FollowCam;
                    break;
                case "TopCam":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.TopCam);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.TopCam;
                    break;
                case "SpecCam":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.SpecCam);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.SpecCam;
                    break;

                //NOT USED
                case "Forward":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.CameraForward);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.CameraForward;
                    break;
                case "Backward":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.CameraBackward);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.CameraBackward;
                    break;
                case "Left":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.CameraLeft);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.CameraLeft;
                    break;
                case "Right":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.CameraRight);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.CameraRight;
                    break;
                case "Speed":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.Inputs.inputs.CameraSpeed);
                    keyButtonParameters.Key = inputManager.Inputs.inputs.CameraSpeed;
                    break;

                default:
                    Debug.Log("Erreur");
                    break;
            }
            keyButtonParameters.Button.GetComponentInChildren<Text>().text = _temp;
        }

    }

    public void ChangeKey(KeyButtonParameters keyButtonParameters)
    {
        isChangingKey = true;
        buttonParameters = keyButtonParameters;
        buttonParameters.Button.GetComponent<Button>().interactable = false;
        buttonParameters.Button.GetComponentInChildren<Text>().text = "Press key";

        if (lastKeyPressed == KeyCode.Escape)
        {
            isChangingKey = false;
            buttonParameters.Button.GetComponent<Button>().interactable = true;
            lastKeyPressed = KeyCode.None;
            buttonParameters.Button.GetComponentInChildren<Text>().text = buttonParameters.Key.ToString();
        }
        if (lastKeyPressed != KeyCode.None && lastKeyPressed != KeyCode.Escape)
        {
            isChangingKey = false;
            buttonParameters.Button.GetComponentInChildren<Text>().text = inputManager.GetSimplifiedKeyAsString(lastKeyPressed);
            buttonParameters.Key = lastKeyPressed;

            lastKeyPressed = KeyCode.None;
            buttonParameters.Button.GetComponent<Button>().interactable = true;
        }
    }
    public void SetKeyDefault()
    {
        string _temp = "";

        foreach (KeyButtonParameters keyButtonParameters in keyButtonList)
        {
            switch (keyButtonParameters.keyName)
            {
                case "MainButton1":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.MainButton1);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.MainButton1;
                    break;
                case "MainButton2":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.MainButton2);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.MainButton2;
                    break;
                case "Leaderboard":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.Learderboard);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.Learderboard;
                    break;
                case "Ping":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.Ping);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.Ping;
                    break;
                case "Chat":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.Chat);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.Chat;
                    break;
                case "FollowCam":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.FollowCam);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.FollowCam;
                    break;
                case "TopCam":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.TopCam);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.TopCam;
                    break;
                case "SpecCam":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.SpecCam);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.SpecCam;
                    break;

                //NOT USED
                case "Forward":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.CameraForward);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.CameraForward;
                    break;
                case "Backward":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.CameraBackward);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.CameraBackward;
                    break;
                case "Left":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.CameraLeft);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.CameraLeft;
                    break;
                case "Right":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.CameraRight);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.CameraRight;
                    break;
                case "Speed":
                    _temp = inputManager.GetSimplifiedKeyAsString(inputManager.DefaultInputs.inputs.CameraSpeed);
                    keyButtonParameters.Key = inputManager.DefaultInputs.inputs.CameraSpeed;
                    break;

                default:
                    Debug.Log("Erreur");
                    break;
            }
            keyButtonParameters.Button.GetComponentInChildren<Text>().text = _temp;
        }
    }
    public void ApplyKeyChange(KeyButtonParameters keyButtonParameters)
    {
        switch (keyButtonParameters.keyName)
        {
            case "MainButton1":
                inputManager.Inputs.inputs.MainButton1 = keyButtonParameters.Key;
                break;
            case "MainButton2":
                inputManager.Inputs.inputs.MainButton2 = keyButtonParameters.Key;
                break;
            case "Leaderboard":
                inputManager.Inputs.inputs.Learderboard = keyButtonParameters.Key;
                break;
            case "Ping":
                inputManager.Inputs.inputs.Ping = keyButtonParameters.Key;
                break;
            case "Chat":
                inputManager.Inputs.inputs.Chat = keyButtonParameters.Key;
                break;
            case "FollowCam":
                inputManager.Inputs.inputs.FollowCam = keyButtonParameters.Key;
                break;
            case "TopCam":
                inputManager.Inputs.inputs.TopCam = keyButtonParameters.Key;
                break;
            case "SpecCam":
                inputManager.Inputs.inputs.SpecCam = keyButtonParameters.Key;
                break;

            //NOT USED
            case "Forward":
                inputManager.Inputs.inputs.CameraForward = keyButtonParameters.Key;
                break;
            case "Backward":
                inputManager.Inputs.inputs.CameraBackward = keyButtonParameters.Key;
                break;
            case "Left":
                inputManager.Inputs.inputs.CameraLeft = keyButtonParameters.Key;
                break;
            case "Right":
                inputManager.Inputs.inputs.CameraRight = keyButtonParameters.Key;
                break;
            case "Speed":
                inputManager.Inputs.inputs.CameraSpeed = keyButtonParameters.Key;
                break;

            default:
                Debug.Log("Erreur");
                break;
        }
    }
    // <<
}


