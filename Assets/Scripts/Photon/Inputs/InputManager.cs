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

        VerifySettings();
        DontDestroyOnLoad(this.gameObject);
        InitInputs();
    }

    private void VerifySettings()
    {
        if (!File.Exists(Application.persistentDataPath + "Settings" + ".json"))
        {
            settingsSaves.GeneralVolume = 1;
            string json = JsonUtility.ToJson(settingsSaves);
            File.WriteAllText(Application.persistentDataPath + "Settings" + ".json", json);
        }
    }
    private void InitInputs()
    {
        if (!Inputs.init)
        {
            Inputs.inputs.CameraBackward = DefaultInputs.inputs.CameraBackward;
            Inputs.inputs.CameraForward = DefaultInputs.inputs.CameraForward;
            Inputs.inputs.CameraLeft = DefaultInputs.inputs.CameraLeft;
            Inputs.inputs.CameraRight = DefaultInputs.inputs.CameraRight;
            Inputs.inputs.MouseSensitivity = DefaultInputs.inputs.MouseSensitivity;
            Inputs.inputs.MainButton2 = DefaultInputs.inputs.MainButton2;
            Inputs.inputs.Learderboard = DefaultInputs.inputs.Learderboard;

            Inputs.init = true;
        }
    }

    private void Start()
    {
        using (StreamReader r = new StreamReader(Application.persistentDataPath + "Settings" + ".json"))
        {
            var dataAsJson = r.ReadToEnd();
            settingsSaves = JsonUtility.FromJson<SettingsSaves>(dataAsJson);
        }

        Inputs.inputs.GeneralVolume = settingsSaves.GeneralVolume;
        AudioListener.volume = Inputs.inputs.GeneralVolume;
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
