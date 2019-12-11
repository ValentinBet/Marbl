using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public class MarblGame
{
    public const string PLAYER_READY = "IsPlayerReady";

    public static Color GetColor(int colorChoice)
    {
        switch (colorChoice)
        {
            case 0: return Color.red;
            case 1: return Color.green;
            case 2: return new Color(0f, 0.4384f, 1);
            case 3: return Color.yellow;
            case 4: return Color.white;
            case 5: return Color.grey;
            case 6: return Color.magenta;
            case 7: return Color.cyan;
        }
        return Color.black; 
    }

    public static Team GetTeam(int teamChoice)
    {
        switch (teamChoice)
        {
            case 0: return Team.red;
            case 1: return Team.green;
            case 2: return Team.blue;
            case 3: return Team.yellow;
            case 4: return Team.neutral;
        }
        return Team.red;
    }

    public static string GetTeamString(int teamChoice)
    {
        switch (teamChoice)
        {
            case 0: return "Team red";
            case 1: return "Team green";
            case 2: return "Team blue";
            case 3: return "Team yellow";
            case 4: return "Team neutral";
        }
        return "Team red";
    }

    public enum CameraMode
    {
        Void,
        Targeted,
        TeamCentered,
        MapCentered,
        Free
    }
}