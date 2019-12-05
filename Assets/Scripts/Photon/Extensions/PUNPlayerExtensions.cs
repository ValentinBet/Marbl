using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    public class PUNPlayerExtensions: MonoBehaviour
    {
        public const string turnPlayer = "playerTurn";
    }

    public static class PlayerExtensions
    {
        public static void SetPlayerTurnState(this Player player, bool mapLoaded)
        {
            Hashtable _turnPlayer = new Hashtable();
            _turnPlayer[PUNPlayerExtensions.turnPlayer] = mapLoaded;

            player.SetCustomProperties(_turnPlayer); 
        }


        public static bool GetPlayerTurnState(this Player player)
        {
            object _turnPlayer;

            if (player.CustomProperties.TryGetValue(PUNPlayerExtensions.turnPlayer, out _turnPlayer))
            {
                return (bool)_turnPlayer;
            }
            return false;
        }
    }

}