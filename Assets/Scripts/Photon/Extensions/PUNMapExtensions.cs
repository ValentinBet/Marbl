using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    public class PUNMapExtensions: MonoBehaviour
    {
        public const string maploaded = "maploaded";
    }

    public static class MapExtensions
    {
        public static void SetPlayerMapState(this Player player, bool mapLoaded)
        {
            Hashtable _mapLoaded = new Hashtable();
            _mapLoaded[PUNMapExtensions.maploaded] = mapLoaded;

            player.SetCustomProperties(_mapLoaded); 
        }


        public static bool GetPlayerMapState(this Player player)
        {
            object _mapLoaded;

            if (player.CustomProperties.TryGetValue(PUNMapExtensions.maploaded, out _mapLoaded))
            {
                return (bool)_mapLoaded;
            }
            return false;
        }
    }

}