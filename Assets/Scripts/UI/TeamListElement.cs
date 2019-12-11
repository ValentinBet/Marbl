using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

public class TeamListElement : MonoBehaviour
{
    public Team myTeam;

    public Text teamText;
    public Text pointText;
    public Text marblNumber;


    public void SetElements(Team team, int point, int marbl)
    {
        myTeam = team;

        teamText.text = MarblGame.GetTeamString((int) myTeam);
        teamText.color = MarblGame.GetColor((int) myTeam);

        pointText.text = point.ToString() + " pts.";

        marblNumber.text = marbl.ToString();
    }
}
