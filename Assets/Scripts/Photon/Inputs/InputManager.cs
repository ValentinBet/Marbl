using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputsList Inputs;
    public InputsList DefaultInputs;

    private static InputManager _instance;

    public static InputManager Instance { get { return _instance; } }

    private SettingsSaves settingsSaves = new SettingsSaves();
    private string SettingsFileName;
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
        SettingsFileName = Application.persistentDataPath + "Settings" + ".json";
        VerifySettings();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        GetSetJsonData();
        AudioListener.volume = Inputs.inputs.GeneralVolume;
    }

    private void VerifySettings()
    {
        if (!File.Exists(SettingsFileName))
        {
            settingsSaves.AppVersion = MarblGame.APP_VERSION;

            settingsSaves.MainButton1 = DefaultInputs.inputs.MainButton1;
            settingsSaves.MainButton2 = DefaultInputs.inputs.MainButton2;
            settingsSaves.Learderboard = DefaultInputs.inputs.Learderboard;
            settingsSaves.Ping = DefaultInputs.inputs.Ping;
            settingsSaves.Chat = DefaultInputs.inputs.Chat;
            settingsSaves.CameraForward = DefaultInputs.inputs.CameraForward;
            settingsSaves.CameraBackward = DefaultInputs.inputs.CameraBackward;
            settingsSaves.CameraLeft = DefaultInputs.inputs.CameraLeft;
            settingsSaves.CameraRight = DefaultInputs.inputs.CameraRight;
            settingsSaves.CameraSpeed = DefaultInputs.inputs.CameraSpeed;
            settingsSaves.FollowCam = DefaultInputs.inputs.FollowCam;
            settingsSaves.TopCam = DefaultInputs.inputs.TopCam;
            settingsSaves.SpecCam = DefaultInputs.inputs.SpecCam;
            settingsSaves.MouseSensitivity = DefaultInputs.inputs.MouseSensitivity;
            settingsSaves.GeneralVolume = DefaultInputs.inputs.GeneralVolume;

            string json = JsonUtility.ToJson(settingsSaves);
            File.WriteAllText(SettingsFileName, json);
        } else
        {
            using (StreamReader r = new StreamReader(SettingsFileName))
            {
                var dataAsJson = r.ReadToEnd();
                settingsSaves = JsonUtility.FromJson<SettingsSaves>(dataAsJson);
            }

            if (settingsSaves.AppVersion != MarblGame.APP_VERSION)
            {
                UpdateJsonSettingsFile();             
            }
        }
    }

    private void UpdateJsonSettingsFile()
    {
        GetSetJsonData();
        File.Delete(SettingsFileName);
        settingsSaves.AppVersion = MarblGame.APP_VERSION;

        settingsSaves.MainButton1 = Inputs.inputs.MainButton1;
        settingsSaves.MainButton2 = Inputs.inputs.MainButton2;
        settingsSaves.Learderboard = Inputs.inputs.Learderboard;
        settingsSaves.Ping = Inputs.inputs.Ping;
        settingsSaves.Chat = Inputs.inputs.Chat;
        settingsSaves.CameraForward = Inputs.inputs.CameraForward;
        settingsSaves.CameraBackward = Inputs.inputs.CameraBackward;
        settingsSaves.CameraLeft = Inputs.inputs.CameraLeft;
        settingsSaves.CameraRight = Inputs.inputs.CameraRight;
        settingsSaves.CameraSpeed = Inputs.inputs.CameraSpeed;
        settingsSaves.FollowCam = Inputs.inputs.FollowCam;
        settingsSaves.TopCam = Inputs.inputs.TopCam;
        settingsSaves.SpecCam = Inputs.inputs.SpecCam;
        settingsSaves.MouseSensitivity = Inputs.inputs.MouseSensitivity;
        settingsSaves.GeneralVolume = Inputs.inputs.GeneralVolume;

        string json = JsonUtility.ToJson(settingsSaves);
        File.WriteAllText(SettingsFileName, json);
    }

    private void GetSetJsonData()
    {
        Inputs.inputs.MainButton1 = settingsSaves.MainButton1;
        Inputs.inputs.MainButton2 = settingsSaves.MainButton2;
        Inputs.inputs.Learderboard = settingsSaves.Learderboard;
        Inputs.inputs.Ping = settingsSaves.Ping;
        Inputs.inputs.Chat = settingsSaves.Chat;
        Inputs.inputs.CameraForward = settingsSaves.CameraForward;
        Inputs.inputs.CameraBackward = settingsSaves.CameraBackward;
        Inputs.inputs.CameraLeft = settingsSaves.CameraLeft;
        Inputs.inputs.CameraRight = settingsSaves.CameraRight;
        Inputs.inputs.CameraSpeed = settingsSaves.CameraSpeed;
        Inputs.inputs.FollowCam = settingsSaves.FollowCam;
        Inputs.inputs.TopCam = settingsSaves.TopCam;
        Inputs.inputs.SpecCam = settingsSaves.SpecCam;
        Inputs.inputs.MouseSensitivity = settingsSaves.MouseSensitivity;
        Inputs.inputs.GeneralVolume = settingsSaves.GeneralVolume;
    }

    public string GetSimplifiedKeyAsString(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Keypad0:
                return "0";
            case KeyCode.Keypad1:
                return "1";
            case KeyCode.Keypad2:
                return "2";
            case KeyCode.Keypad3:
                return "3";
            case KeyCode.Keypad4:
                return "4";
            case KeyCode.Keypad5:
                return "5";
            case KeyCode.Keypad6:
                return "6";
            case KeyCode.Keypad7:
                return "7";
            case KeyCode.Keypad8:
                return "8";
            case KeyCode.Keypad9:
                return "9";
            case KeyCode.Alpha0:
                return "²";
            case KeyCode.Alpha1:
                return "&";
            case KeyCode.Alpha2:
                return "é";
            case KeyCode.Alpha3:
                return "\"";
            case KeyCode.Alpha4:
                return "'";
            case KeyCode.Alpha5:
                return "(";
            case KeyCode.Alpha6:
                return "-";
            case KeyCode.Alpha7:
                return "è";
            case KeyCode.Alpha8:
                return "_";
            case KeyCode.Alpha9:
                return "ç";
            default:
                return key.ToString();
        }
    }

}
