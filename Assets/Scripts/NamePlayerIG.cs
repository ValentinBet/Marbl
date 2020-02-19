using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

public class NamePlayerIG : MonoBehaviour
{
    public Text PlayerName;
    public Text PlayerNameShadow;

    public void SetPlayer(string _playerName, Team _team)
    {
        PlayerName.text = _playerName;
        PlayerNameShadow.text = _playerName;

        PlayerName.color = MarblGame.GetColor((int)_team);
    }
}
