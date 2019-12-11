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


    public void SetElements(Team team, string point, string marbl)
    {
        myTeam = team;

        /*
        switch (myTeam)
        {
            case 0:
                teamText.text = "Team red";
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;
        }

        //teamText.color = MarblGame.GetColor(myTeam);
        */
    }
}
