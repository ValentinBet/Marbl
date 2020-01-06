using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class HillObj : MonoBehaviour
{

    public List<BallSettings> ballInside = new List<BallSettings>();

    public List<SpriteRenderer> imgWall = new List<SpriteRenderer>();


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Ball")
        {
            ballInside.Add(other.gameObject.GetComponent<BallSettings>());
        }
        CheckColor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Ball")
        {
            ballInside.Remove(other.gameObject.GetComponent<BallSettings>());
        }
        CheckColor();
    }

    void CheckColor()
    {
        Dictionary<Color, int> teamValue = new Dictionary<Color, int>() { { MarblGame.GetColor(0), 0 }, { MarblGame.GetColor(1), 0 }, { MarblGame.GetColor(2), 0 }, { MarblGame.GetColor(3), 0 } };

        if(ballInside.Count == 0)
        {
            SetColor(Color.white);
            return;
        }

        foreach (BallSettings _element in ballInside)
        {
            if(_element.myteam == Team.neutral) { continue; }
            teamValue[MarblGame.GetColor((int)_element.myteam)] += 1;
        }

        if(teamValue.Count == 0)
        {
            SetColor(Color.white);
            return;
        }

        Color keyOfMaxValue = teamValue.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

        foreach (KeyValuePair<Color, int> _element in teamValue)
        {
            if(_element.Key != keyOfMaxValue && _element.Value == teamValue[keyOfMaxValue])
            {
                SetColor(Color.white);
                return;
            }
        }

        SetColor(keyOfMaxValue);
    }


    public void SetColor(Color newColor)
    {
        foreach(SpriteRenderer img in imgWall)
        {
            img.color = newColor;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CheckColor();
    }
}
