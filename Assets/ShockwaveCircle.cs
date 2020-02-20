using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class ShockwaveCircle : MonoBehaviour
{
    public Renderer ShockCircle;
    public Renderer ShockCircleAnimated;

    public Material red;
    public Material blue;
    public Material green;
    public Material yellow;
    public Material white;

    Material currentMat;

    // Update is called once per frame
    public void SetColor(Team _team)
    {
        switch (_team)
        {
            case Team.red:
                currentMat = red;
                break;

            case Team.green:
                currentMat = green;
                break;

            case Team.blue:
                currentMat = blue;
                break;

            case Team.yellow:
                currentMat = yellow;
                break;

            case Team.neutral:
                currentMat = white;
                break;
        }

        ShockCircle.material = currentMat;
        ShockCircleAnimated.material = currentMat;
    }
}
