using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    public class PUNScoreExtension : MonoBehaviour
    {
        public const string PlayerScoreProp = "score";
    }

    public static class ScoreExtension
    {
        public static void SetPlayerScore(this Player player, int newScore)
        {
            Hashtable score = new Hashtable(); 
            score[PunPlayerScores.PlayerScoreProp] = newScore;

            player.SetCustomProperties(score); 
        }

        public static void AddPlayerScore(this Player player, int scoreToAddToCurrent)
        {
            int current = player.GetPlayerScore();
            current = current + scoreToAddToCurrent;

            Hashtable score = new Hashtable();  
            score[PunPlayerScores.PlayerScoreProp] = current;

            player.SetCustomProperties(score); 
        }

        public static int GetPlayerScore(this Player player)
        {
            object score;

            if (player.CustomProperties.TryGetValue(PunPlayerScores.PlayerScoreProp, out score))
            {
                return (int)score;
            }
            return 0;
        }
    }

}