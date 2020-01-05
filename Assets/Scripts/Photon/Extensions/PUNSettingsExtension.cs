using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    public class PUNSettingsExtension : MonoBehaviour
    {
        public const string mapProp = "map";
        public const string customMap = "mapCustom";
        public const string deathmatchProp = "deathmatch";
        public const string hillProp = "hill";
        public const string coinsProp = "coins";
        public const string potatoProp = "potato";
        public const string hueProp = "hue";
        public const string billardProp = "billard";
        public const string turnLimitProp = "turnLimit";
        public const string roundProp = "round";
        public const string nbrBallProp = "nbrBall";
        public const string spawnModeProp = "spawnMode";
        public const string launchPowerProp = "launchPower";
        public const string impactPowerProp = "impactPower";
        public const string winPointDMProp = "winPointDM";
        public const string elimPointDMProp = "elimPointDM";
        public const string killstreakDMProp = "killstreakDM";
        public const string suicidePointDMProp = "suicidePointDM";
        public const string nbrHillProp = "nbrHill";
        public const string hillMode = "hillMode";
        public const string hillPoint = "hillPoint";
        public const string coinsAmount = "coinsAmount";
        public const string potatoTurnMin = "potatoTurnMin";
        public const string potatoTurnMax = "potatoTurnMax";
        public const string hueNutralBall = "hueNutralBall";
        public const string billardBall = "billardBall";
    }


    public static class SettingsExtension
    {
        public static void SetMap(this Room room, int map)
        {
            Hashtable _map = new Hashtable();
            _map[PUNSettingsExtension.mapProp] = map;
            room.SetCustomProperties(_map);
        }

        public static int GetMap(this Room room)
        {
            object _map;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.mapProp, out _map))
            {
                return (int)_map;
            }
            return 0;
        }

        public static void SetCustomMap(this Room room, bool map)
        {
            Hashtable _mapCust = new Hashtable();
            _mapCust[PUNSettingsExtension.customMap] = map;
            room.SetCustomProperties(_mapCust);
        }

        public static bool GetCustomMap(this Room room)
        {
            object _mapCust;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.customMap, out _mapCust))
            {
                return (bool)_mapCust;
            }
            return false;
        }

        public static void SetDeathmatch(this Room room, bool deathmatch)
        {
            Hashtable _deathmatch = new Hashtable();
            _deathmatch[PUNSettingsExtension.deathmatchProp] = deathmatch;
            room.SetCustomProperties(_deathmatch);
        }

        public static bool GetDeathmatch(this Room room)
        {
            object _deathmatch;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.deathmatchProp, out _deathmatch))
            {
                return (bool)_deathmatch;
            }
            return false;
        }
        public static void SetHill(this Room room, bool hill)
        {
            Hashtable _hill = new Hashtable();
            _hill[PUNSettingsExtension.hillProp] = hill;
            room.SetCustomProperties(_hill);
        }

        public static bool GetHill(this Room room)
        {
            object _hill;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.hillProp, out _hill))
            {
                return (bool)_hill;
            }
            return false;
        }
        public static void SetCoins(this Room room, bool coins)
        {
            Hashtable _coins = new Hashtable();
            _coins[PUNSettingsExtension.coinsProp] = coins;
            room.SetCustomProperties(_coins);
        }

        public static bool GetCoins(this Room room)
        {
            object _coins;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.coinsProp, out _coins))
            {
                return (bool)_coins;
            }
            return false;
        }
       
        public static void SetHue(this Room room, bool hue)
        {
            Hashtable _hue = new Hashtable();
            _hue[PUNSettingsExtension.hueProp] = hue;
            room.SetCustomProperties(_hue);
        }

        public static bool GetHue(this Room room)
        {
            object _hue;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.hueProp, out _hue))
            {
                return (bool)_hue;
            }
            return false;
        }

        public static void SetTurnLimit(this Room room, int turnLimit)
        {
            Hashtable _turnLimit = new Hashtable();
            _turnLimit[PUNSettingsExtension.turnLimitProp] = turnLimit;
            room.SetCustomProperties(_turnLimit);
        }

        public static int GetTurnLimit(this Room room)
        {
            object _turnLimit;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.turnLimitProp, out _turnLimit))
            {
                return (int)_turnLimit;
            }
            return 0;
        }

        public static void SetRoundProp(this Room room, int round)
        {
            Hashtable _round = new Hashtable();
            _round[PUNSettingsExtension.roundProp] = round;
            room.SetCustomProperties(_round);
        }

        public static int GetRoundProp(this Room room)
        {
            object _round;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.roundProp, out _round))
            {
                return (int)_round;
            }
            return 0;
        }
        public static void SetNbrBallProp(this Room room, int NrbBall)
        {
            Hashtable _NrbBall = new Hashtable();
            _NrbBall[PUNSettingsExtension.nbrBallProp] = NrbBall;
            room.SetCustomProperties(_NrbBall);
        }

        public static int GetNbrbBallProp(this Room room)
        {
            object _NrbBall;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.nbrBallProp, out _NrbBall))
            {
                return (int)_NrbBall;
            }
            return 0;
        }
        public static void SetSpawnMode(this Room room, int spawnMode)
        {
            Hashtable _spawnMode = new Hashtable();
            _spawnMode[PUNSettingsExtension.spawnModeProp] = spawnMode;
            room.SetCustomProperties(_spawnMode);
        }

        public static int GetSpawnMode(this Room room)
        {
            object _spawnMode;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.spawnModeProp, out _spawnMode))
            {
                return (int)_spawnMode;
            }
            return 0;
        }
        public static void SetLaunchPower(this Room room, float launchPower)
        {
            Hashtable _launchPower = new Hashtable();
            _launchPower[PUNSettingsExtension.launchPowerProp] = launchPower;
            room.SetCustomProperties(_launchPower);
        }

        public static float GetLaunchPower(this Room room)
        {
            object _launchPower;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.launchPowerProp, out _launchPower))
            {
                return (float)_launchPower;
            }
            return 0;
        }

        public static void SetImpactPower(this Room room, float impactPower)
        {
            Hashtable _impactPower = new Hashtable();
            _impactPower[PUNSettingsExtension.impactPowerProp] = impactPower;
            room.SetCustomProperties(_impactPower);
        }

        public static float GetImpactPower(this Room room)
        {
            object _impactPower;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.impactPowerProp, out _impactPower))
            {
                return (float)_impactPower;
            }
            return 0;
        }
        public static void SetWinPointDM(this Room room, int winPointDM)
        {
            Hashtable _winPointDM = new Hashtable();
            _winPointDM[PUNSettingsExtension.winPointDMProp] = winPointDM;
            room.SetCustomProperties(_winPointDM);
        }

        public static int GetWinPointDM(this Room room)
        {
            object _winPointDM;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.winPointDMProp, out _winPointDM))
            {
                return (int)_winPointDM;
            }
            return 0;
        }
        public static void SetElimPointDM(this Room room, int elimPointDM)
        {
            Hashtable _elimPointDM = new Hashtable();
            _elimPointDM[PUNSettingsExtension.elimPointDMProp] = elimPointDM;
            room.SetCustomProperties(_elimPointDM);
        }

        public static int GetElimPointDM(this Room room)
        {
            object _elimPointDM;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.elimPointDMProp, out _elimPointDM))
            {
                return (int)_elimPointDM;
            }
            return 0;
        }

        public static void SetKillstreakDMProp(this Room room, bool killstreak)
        {
            Hashtable _killstreakDMProp = new Hashtable();
            _killstreakDMProp[PUNSettingsExtension.killstreakDMProp] = killstreak;
            room.SetCustomProperties(_killstreakDMProp);
        }

        public static bool GetKillstreakDMProp(this Room room)
        {
            object _killstreakDMProp;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.killstreakDMProp, out _killstreakDMProp))
            {
                return (bool)_killstreakDMProp;
            }
            return false;
        }

        public static void SetSuicidePointDM(this Room room, int suicidePointDM)
        {
            Hashtable _suicidePointDM = new Hashtable();
            _suicidePointDM[PUNSettingsExtension.suicidePointDMProp] = suicidePointDM;
            room.SetCustomProperties(_suicidePointDM);
        }

        public static int GetSuicidePointDM(this Room room)
        {
            object _suicidePointDM;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.suicidePointDMProp, out _suicidePointDM))
            {
                return (int)_suicidePointDM;
            }
            return 0;
        }
        public static void SetNbrHill(this Room room, int nbrHill)
        {
            Hashtable _nbrHill = new Hashtable();
            _nbrHill[PUNSettingsExtension.nbrHillProp] = nbrHill;
            room.SetCustomProperties(_nbrHill);
        }

        public static int GetNbrHill(this Room room)
        {
            object _nbrHill;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.nbrHillProp, out _nbrHill))
            {
                return (int)_nbrHill;
            }
            return 0;
        }
        public static void SetHillMode(this Room room, int hillMode)
        {
            Hashtable _hillMode = new Hashtable();
            _hillMode[PUNSettingsExtension.hillMode] = hillMode;
            room.SetCustomProperties(_hillMode);
        }

        public static int GetHillMode(this Room room)
        {
            object _hillMode;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.hillMode, out _hillMode))
            {
                return (int)_hillMode;
            }
            return 0;
        }
        public static void SetHillPoint(this Room room, int hillPoint)
        {
            Hashtable _hillPoint = new Hashtable();
            _hillPoint[PUNSettingsExtension.hillPoint] = hillPoint;
            room.SetCustomProperties(_hillPoint);
        }

        public static int GetHillPoint(this Room room)
        {
            object _hillPoint;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.hillPoint, out _hillPoint))
            {
                return (int)_hillPoint;
            }
            return 0;
        }
        public static void SetCoinsAmount(this Room room, int coinsAmount)
        {
            Hashtable _coinsAmount = new Hashtable();
            _coinsAmount[PUNSettingsExtension.coinsAmount] = coinsAmount;
            room.SetCustomProperties(_coinsAmount);
        }

        public static int GetCoinsAmount(this Room room)
        {
            object _coinsAmount;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.coinsAmount, out _coinsAmount))
            {
                return (int)_coinsAmount;
            }
            return 0;
        }
        public static void SetPotatoTurnMin(this Room room, int potatoTurnMin)
        {
            Hashtable _potatoTurnMin = new Hashtable();
            _potatoTurnMin[PUNSettingsExtension.potatoTurnMin] = potatoTurnMin;
            room.SetCustomProperties(_potatoTurnMin);
        }

        public static int GetPotatoTurnMin(this Room room)
        {
            object _potatoTurnMin;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.potatoTurnMin, out _potatoTurnMin))
            {
                return (int)_potatoTurnMin;
            }
            return 0;
        }
        public static void SetPotatoTurnMax(this Room room, int potatoTurnMax)
        {
            Hashtable _potatoTurnMax = new Hashtable();
            _potatoTurnMax[PUNSettingsExtension.potatoTurnMax] = potatoTurnMax;
            room.SetCustomProperties(_potatoTurnMax);
        }

        public static int GetPotatoTurnMax(this Room room)
        {
            object _potatoTurnMax;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.potatoTurnMax, out _potatoTurnMax))
            {
                return (int)_potatoTurnMax;
            }
            return 0;
        }
        public static void SetHueNutralBall(this Room room, int hueNutralBall)
        {
            Hashtable _hueNutralBall = new Hashtable();
            _hueNutralBall[PUNSettingsExtension.hueNutralBall] = hueNutralBall;
            room.SetCustomProperties(_hueNutralBall);
        }

        public static int GetHueNutralBall(this Room room)
        {
            object _hueNutralBall;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.hueNutralBall, out _hueNutralBall))
            {
                return (int)_hueNutralBall;
            }
            return 0;
        }
        public static void SetBillardBall(this Room room, int billardBall)
        {
            Hashtable _billardBall = new Hashtable();
            _billardBall[PUNSettingsExtension.billardBall] = billardBall;
            room.SetCustomProperties(_billardBall);
        }

        public static int GetBillardBall(this Room room)
        {
            object _billardBall;

            if (room.CustomProperties.TryGetValue(PUNSettingsExtension.billardBall, out _billardBall))
            {
                return (int)_billardBall;
            }
            return 0;
        }

    }
}