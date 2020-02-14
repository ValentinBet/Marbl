using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    public class PUNRoomCustomExtension : MonoBehaviour
    {
        public const string forceCam = "forceCam";
    }

    public static class RoomCustomExtension
    {
        public static void SetForceCam(this Room room, bool cam)
        {
            Hashtable _forceCam = new Hashtable();
            _forceCam[PUNRoomCustomExtension.forceCam] = cam;
            room.SetCustomProperties(_forceCam);
        }

        public static bool GetForceCam(this Room room)
        {
            object _cam;

            if (room.CustomProperties.TryGetValue(PUNRoomCustomExtension.forceCam, out _cam))
            {
                return (bool)_cam;
            }
            return false;
        }
    }
}