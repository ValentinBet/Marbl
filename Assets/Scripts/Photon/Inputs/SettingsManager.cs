using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static readonly FullScreenMode[] Windowmodes = new FullScreenMode[2] { FullScreenMode.ExclusiveFullScreen, FullScreenMode.Windowed };
    public static readonly int[,] Resolutions = new int[,] { { 1920, 1080 }, { 1680, 1050 }, { 1600, 1200 }, { 1600, 900 }, { 1600, 900 },
        { 1440, 900 }, { 1366, 768 }, { 1360, 768 }, { 1280, 1024 }, { 1280, 960 }, { 1280, 800 }, { 1280, 768 }, { 1280, 720 }, { 1152, 864 }, { 1024, 768 } };

    [Header("General")]
    public GameObject videoPanel;
    public GameObject audioPanel;
    public GameObject controlsPanel;

    [Header("Video")]
    public Dropdown windowedDropdown;
    public Dropdown resDropdown;
    public Dropdown qualityDropdown;

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


    private void Start()
    {
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
    }

    private void InitVideoVisuals()
    {
        windowedDropdown.value = settingsList.settings.Windowmode;
        resDropdown.value = settingsList.settings.Resolution;
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
        settingsList.settings.Resolution = resDropdown.value;
        settingsList.settings.Quality = qualityDropdown.value;

        settingsList.settings.GeneralVolume = generalVolumeSlider.value;
        settingsList.settings.MouseSensitivity = mouseSensitivitySlider.value;
        // <<

        foreach (KeyButtonParameters keyButtonParameters in keyButtonList)
        {
            ApplyKeyChange(keyButtonParameters);
        }

        MakeChanges();
        InitSettingsList();
    }

    private void MakeChanges()
    {
        Screen.SetResolution(Resolutions[resDropdown.value, 0], Resolutions[resDropdown.value, 1], Windowmodes[settingsList.settings.Windowmode]);
        QualitySettings.SetQualityLevel(settingsList.settings.Quality);
    }

    public void QuitSettingsScene()
    {
        SceneManager.UnloadSceneAsync("Settings");
    }

    // <<

    // VIDEO SETTINGS >>

    private void InitResDropdown()
    {
        for (int x = 0; x < Resolutions.GetLength(0); x++)
        {
            resDropdown.options.Add(new Dropdown.OptionData() { text = Resolutions[x, 0] + " * " + Resolutions[x, 1] });
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
        generalVolumeInputField.text = (settingsList.settings.GeneralVolume * 100).ToString();
        generalVolumeSlider.value = settingsList.settings.GeneralVolume;
    }

    public void OnGeneralVolumeSliderUpdate()
    {
        generalVolumeInputField.text = (generalVolumeSlider.value * 100).ToString();
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
        generalVolumeSlider.value = float.Parse(generalVolumeInputField.text) / 100;
    }

    // <<

    // CONTROLS SETTINGS >>

    public void InitControlsVisuals()
    {
        mouseSensitivityInputField.text = (settingsList.settings.MouseSensitivity * 10).ToString();
        mouseSensitivitySlider.value = settingsList.settings.MouseSensitivity;

        InitKeyVisuals();
    }

    public void OnSensitivitySliderUpdate()
    {
        mouseSensitivityInputField.text = (mouseSensitivitySlider.value * 10).ToString();
        InputManager.Instance.Inputs.inputs.MouseSensitivity = mouseSensitivitySlider.value;
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
        mouseSensitivitySlider.value = float.Parse(mouseSensitivityInputField.text) / 10;
        InputManager.Instance.Inputs.inputs.MouseSensitivity = mouseSensitivitySlider.value;
    }

    public void InitKeyVisuals()
    {
        foreach (KeyButtonParameters keyButtonParameters in keyButtonList)
        {
            switch (keyButtonParameters.keyName)
            {
                case "MainButton1":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.MainButton1.ToString();
                    keyButtonParameters.Key = InputManager.Instance.Inputs.inputs.MainButton1;
                    break;
                case "MainButton2":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.MainButton2.ToString();
                    keyButtonParameters.Key = InputManager.Instance.Inputs.inputs.MainButton2;
                    break;
                case "Leaderboard":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.Learderboard.ToString();
                    keyButtonParameters.Key = InputManager.Instance.Inputs.inputs.Learderboard;
                    break;
                case "Forward":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.CameraForward.ToString();
                    keyButtonParameters.Key = InputManager.Instance.Inputs.inputs.CameraForward;
                    break;
                case "Backward":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.CameraBackward.ToString();
                    keyButtonParameters.Key = InputManager.Instance.Inputs.inputs.CameraBackward;
                    break;
                case "Left":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.CameraLeft.ToString();
                    keyButtonParameters.Key = InputManager.Instance.Inputs.inputs.CameraLeft;
                    break;
                case "Right":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.Inputs.inputs.CameraRight.ToString();
                    keyButtonParameters.Key = InputManager.Instance.Inputs.inputs.CameraRight;
                    break;
                default:
                    Debug.Log("Erreur");
                    break;
            }
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
            buttonParameters.Button.GetComponentInChildren<Text>().text = lastKeyPressed.ToString();
            buttonParameters.Key = lastKeyPressed;

            lastKeyPressed = KeyCode.None;
            buttonParameters.Button.GetComponent<Button>().interactable = true;
        }
    }
    public void SetKeyDefault()
    {
        foreach (KeyButtonParameters keyButtonParameters in keyButtonList)
        {
            switch (keyButtonParameters.keyName)
            {
                case "MainButton1":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.DefaultInputs.inputs.MainButton1.ToString();
                    keyButtonParameters.Key = InputManager.Instance.DefaultInputs.inputs.MainButton1;
                    break;
                case "MainButton2":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.DefaultInputs.inputs.MainButton2.ToString();
                    keyButtonParameters.Key = InputManager.Instance.DefaultInputs.inputs.MainButton2;
                    break;
                case "Leaderboard":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.DefaultInputs.inputs.Learderboard.ToString();
                    keyButtonParameters.Key = InputManager.Instance.DefaultInputs.inputs.Learderboard;
                    break;
                case "Forward":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.DefaultInputs.inputs.CameraForward.ToString();
                    keyButtonParameters.Key = InputManager.Instance.DefaultInputs.inputs.CameraForward;
                    break;
                case "Backward":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.DefaultInputs.inputs.CameraBackward.ToString();
                    keyButtonParameters.Key = InputManager.Instance.DefaultInputs.inputs.CameraBackward;
                    break;
                case "Left":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.DefaultInputs.inputs.CameraLeft.ToString();
                    keyButtonParameters.Key = InputManager.Instance.DefaultInputs.inputs.CameraLeft;
                    break;
                case "Right":
                    keyButtonParameters.Button.GetComponentInChildren<Text>().text = InputManager.Instance.DefaultInputs.inputs.CameraRight.ToString();
                    keyButtonParameters.Key = InputManager.Instance.DefaultInputs.inputs.CameraRight;
                    break;
                default:
                    Debug.Log("Erreur");
                    break;
            }
        }
    }
    public void ApplyKeyChange(KeyButtonParameters keyButtonParameters)
    {
        switch (keyButtonParameters.keyName)
        {
            case "MainButton1":
                InputManager.Instance.Inputs.inputs.MainButton1 = keyButtonParameters.Key;
                break;
            case "MainButton2":
                InputManager.Instance.Inputs.inputs.MainButton2 = keyButtonParameters.Key;
                break;
            case "Leaderboard":
                InputManager.Instance.Inputs.inputs.Learderboard = keyButtonParameters.Key;
                break;
            case "Forward":
                InputManager.Instance.Inputs.inputs.CameraForward = keyButtonParameters.Key;
                break;
            case "Backward":
                InputManager.Instance.Inputs.inputs.CameraBackward = keyButtonParameters.Key;
                break;
            case "Left":
                InputManager.Instance.Inputs.inputs.CameraLeft = keyButtonParameters.Key;
                break;
            case "Right":
                InputManager.Instance.Inputs.inputs.CameraRight = keyButtonParameters.Key;
                break;
            default:
                Debug.Log("Erreur");
                break;
        }
    }
    // <<
}


