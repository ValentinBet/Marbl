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
        public const string PlayerPersistantScore = "persistantScore";
    }

    public static class ScoreExtension
    {
        public static void SetPlayerScore(this Player player, int newScore)
        {
            Hashtable score = new Hashtable(); 
            score[PUNScoreExtension.PlayerScoreProp] = newScore;

            player.SetCustomProperties(score); 
        }

        public static void AddPlayerScore(this Player player, int scoreToAddToCurrent)
        {
            int current = player.GetPlayerScore();
            current = current + scoreToAddToCurrent;

            Hashtable score = new Hashtable();  
            score[PUNScoreExtension.PlayerScoreProp] = current;

            player.SetCustomProperties(score); 
        }

        public static int GetPlayerScore(this Player player)
        {
            object score;

            if (player.CustomProperties.TryGetValue(PUNScoreExtension.PlayerScoreProp, out score))
            {
                return (int)score;
            }
            return 0;
        }

        public static void SetPlayerPersistantScore(this Player player, int newScore)
        {
            Hashtable score = new Hashtable();
            score[PUNScoreExtension.PlayerPersistantScore] = newScore;

            player.SetCustomProperties(score);
        }

        public static void AddPlayerPersistantScore(this Player player, int scoreToAddToCurrent)
        {
            int current = player.GetPlayerPersistantScore();
            current = current + scoreToAddToCurrent;

            Hashtable score = new Hashtable();
            score[PUNScoreExtension.PlayerPersistantScore] = current;

            player.SetCustomProperties(score);
        }

        public static int GetPlayerPersistantScore(this Player player)
        {
            object score;

            if (player.CustomProperties.TryGetValue(PUNScoreExtension.PlayerPersistantScore, out score))
            {
                return (int)score;
            }
            return 0;
        }


    }

}