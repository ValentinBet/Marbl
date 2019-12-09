using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using static Photon.Pun.UtilityScripts.PunTeams;

namespace Photon.Pun.UtilityScripts
{
    public class PUNTeamScoreExtension : MonoBehaviour
    {
        public const string TeamScoreProp = "teamscore";
    }

    public static class TeamScoreExtension
    {
        public static void SetTeamScore(this Room room, Team team, int newScore)
        {
                Hashtable teamscore = new Hashtable();
                teamscore[team.ToString()] = newScore;
                room.SetCustomProperties(teamscore);
        }

        public static void AddTeamScore(this Room room, Team team, int scoreToAddToCurrent)
        {
            int current = room.GetTeamScore(team);

            current = current + scoreToAddToCurrent;

            Hashtable teamscore = new Hashtable();
            teamscore[team.ToString()] = current;

            room.SetCustomProperties(teamscore);
        }
        public static int GetTeamScore(this Room room, Team team)
        {
            object teamscore;

            if (room.CustomProperties.TryGetValue(team.ToString(), out teamscore))
            {
                return (int)teamscore;
            }
            return 0;
        }
    }
}