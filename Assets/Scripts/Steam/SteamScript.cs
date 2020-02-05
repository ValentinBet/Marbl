using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamScript : MonoBehaviour
{
    //protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    //private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;

    //private void Awake()
    //{
    //    try
    //    {
    //        if (SteamAPI.RestartAppIfNecessary((AppId)480))
    //        {
    //            Application.Quit();
    //            return;
    //        }
    //    }
    //    catch (System.DllNotFoundException e)
    //    {
    //        Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

    //        Application.Quit();
    //        return;
    //    }
    //}

    //private void OnEnable()
    //{
    //    if (SteamManager.Initialized)
    //    {
    //        m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

    //        m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
    //    }
    //}

    //private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    //{
    //    if (pCallback.m_bActive != 0)
    //    {
    //        Debug.Log("Steam Overlay has been activated");
    //    }
    //    else
    //    {
    //        Debug.Log("Steam Overlay has been closed");
    //    }
    //}

    //private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
    //{
    //    if (pCallback.m_bSuccess != 1 || bIOFailure)
    //    {
    //        Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
    //    }
    //    else
    //    {
    //        Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
    //    }
    //}
}
