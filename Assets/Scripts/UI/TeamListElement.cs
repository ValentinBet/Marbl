using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

public class TeamListElement : MonoBehaviour
{
    public Team myTeam;

    public Text teamText;
    public TextMeshProUGUI pointText;
    public Text marblNumber;

    public List<Image> colorBackgroundObj;

    float oldPoint = 0;
    float newPoint = 0;


    public void SetElements(Team team, int point, int marbl)
    {
        myTeam = team;

        teamText.text = MarblGame.GetTeamString((int) myTeam);
        teamText.color = MarblGame.GetColor((int) myTeam);

        newPoint = point;

        marblNumber.text = marbl.ToString();

        foreach(Image element in colorBackgroundObj)
        {
            element.color = MarblGame.GetColor((int)myTeam);
        }
    }

    private void Update()
    {
        oldPoint = Mathf.Lerp(oldPoint, newPoint, 3 * Time.deltaTime);
        pointText.text = Mathf.RoundToInt(oldPoint).ToString() + " pts.";
    }
}
