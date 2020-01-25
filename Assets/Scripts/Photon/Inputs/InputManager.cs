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



}
