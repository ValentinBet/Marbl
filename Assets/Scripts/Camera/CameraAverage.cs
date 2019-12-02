using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static MarblGame;

public class CameraAverage : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform mapPivot;
    [SerializeField] private float rotationSensitivity = 2.5f;
    private CinemachineVirtualCamera[] cameras;
	private List<List<Transform>> marblesTransforms; // [Player][MarbleID]
	private static CameraMode actualMode = CameraMode.Void;

    private Transform targetedTransform; //FollowedMarbletransform;
    private int actualMarble = 0;
    private int actualPlayer = 0;

    public static CameraAverage singleton;

    #endregion

    #region Initialisation
    void Awake()
	{
        if (singleton == null)
        {
            singleton = this;
        }
        InitializeCameras();
        InitializeMarbles(); //Modifications to apply

        SetCameraMode(CameraMode.MapCentered, 0, 0);
    }

    private void InitializeCameras()
    {
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("CCamera");
        cameras = new CinemachineVirtualCamera[tempList.Length];
        for (int i = 0; i < tempList.Length; i++)
        {
            cameras[i] = tempList[i].GetComponent<CinemachineVirtualCamera>();
        }
    }
    private void InitializeMarbles()
    {
        marblesTransforms = new List<List<Transform>>();
        for (int i = 0; i < GameData.playerAmount; i++)
        {
            marblesTransforms.Add(null);
        }
        //Add Marbles, Handled by MarbleSpawner
    }
    public void AssignPlayerMarbles(List<Transform> _marbles, int _playerID)
    {
        if (_playerID < GameData.playerAmount)
        {
            marblesTransforms[_playerID] = _marbles;
        }
        else
        {
            Debug.LogError("_playerID too high, not enough players in \"GameData.playerAmount\"");
        }
    }

    #endregion

    void Update()
	{
		if (actualMode == CameraMode.Targeted)
		{
			ballModeCalculation();
		}
	}

    #region CameraManipulation

    public void SetCameraMode(CameraMode _newMode, int _player, int _marble)
    {
        transform.rotation = Quaternion.identity;
        if (_newMode != actualMode)
        {
            if ((_newMode == CameraMode.MapCentered && actualMode == CameraMode.TeamCentered) || (_newMode == CameraMode.TeamCentered && actualMode == CameraMode.MapCentered))
            {
                Debug.Log("No Switch");

            }
        }
        switch (_newMode)
        {
            case CameraMode.MapCentered:
                MouseControl.ManualTarget(null);
                transform.position = mapPivot.position;
                actualMode = CameraMode.MapCentered;
                cameras[0].Priority = 0;
                cameras[1].Priority = 100;
                cameras[2].Priority = 0;
                break;
            case CameraMode.TeamCentered:
                MouseControl.ManualTarget(null);
                Vector3 _averagePos = Vector3.zero;
                for (int i = 0; i < marblesTransforms[_player].Count; i++)
                {
                    _averagePos += marblesTransforms[_player][i].position;
                }
                _averagePos /= (float)marblesTransforms[_player].Count;
                transform.position = new Vector3(_averagePos.x, 0, _averagePos.z);
                actualMode = CameraMode.TeamCentered;
                cameras[0].Priority = 0;
                cameras[1].Priority = 0;
                cameras[2].Priority = 100;
                break;
            case CameraMode.Targeted:
                cameras[2].Priority = 8;
                targetedTransform = marblesTransforms[_player][_marble];
                actualMode = CameraMode.Targeted;
                cameras[0].Priority = 100;
                cameras[1].Priority = 0;
                cameras[2].Priority = 0;
                break;
        }
    }

    //For UI mainly;
    public void ManualSet(CameraMode _newMode)
	{
        SetCameraMode(_newMode, 0, 0);
	}
    //AutoSet To Map View
    public void ManualSet()
    {
        SetCameraMode(CameraMode.MapCentered, 0, 0);
    }

    private void ballModeCalculation()
    {
        transform.position = targetedTransform.position;

        if (Input.GetMouseButton(0))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X") * rotationSensitivity, 0));
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            rotationSensitivity += 1;
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            rotationSensitivity -= 1;
        }
    }

    #endregion

    #region MarbleManipulation
    
    public void NextMarble()
    {
        actualMarble = (actualMarble + 1) % marblesTransforms[actualPlayer].Count;
        //not that clean, max calls < 7 is okay ...
        while (marblesTransforms[actualPlayer][actualMarble% marblesTransforms[actualPlayer].Count] == null)
        {
            actualMarble = (actualMarble + 1) % marblesTransforms[actualPlayer].Count;
        }
        MouseControl.ManualTarget(marblesTransforms[actualPlayer][actualMarble].gameObject);
    }

    public void NextPlayer()
    {
        actualMarble = 0;
        actualPlayer = (actualPlayer + 1) % GameData.playerAmount;
        SetCameraMode(CameraMode.MapCentered, 0, 0);
    }

    public static void NextPlayerStatic()
    {
        singleton.NextPlayer();
    }

    public static void RemoveMarble(int player, int marble)
    {
        singleton.marblesTransforms[player][marble] = null;
    }

    #endregion
}
