using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;
public class HueManager : MonoBehaviour
{
    private static HueManager _instance;
    public static HueManager Instance { get { return _instance; } }

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

    private void InitNeutralBalls()
    {
        List<Transform> spawnPos = MarblFactory.GetListOfAllChild(GameModeManager.Instance.listNeutralPos);
        spawnPos = MarblFactory.ShuffleList(spawnPos);

        for (int i = 0; i < PhotonNetwork.CurrentRoom.GetHueNutralBall(); i++)
        {
            GameObject _neutralBall = PhotonNetwork.Instantiate("Marbl", spawnPos[0].position, Quaternion.identity);
            _neutralBall.GetComponent<BallSettings>().myteam = MarblGame.GetTeam(4);
            spawnPos.Remove(spawnPos[0]);
        }
    }

    private void EndGame()
    {

    }

    public void ActiveThisMode(bool value)
    {
        if (value)
        {
            InitNeutralBalls();
            return;
        }
        else
        {
            enabled = false;
        }
    }
}
