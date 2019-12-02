using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{
    private static int lastMode;
    //    public int maxmode; //Valeur de test
    private static int testInt = 4;
    public ModeData modetotry;
    public ModifierProfile modifs;

    public void setint(int newint)
    {
        testInt = newint;
    }

    public void StartGame(int players)
    {
        GameData.playerAmount = players;
        GameData.playersScore = new int[players];
        GameData.actualMode = modetotry;
        modifs.FormDictionary();
        GameData.actualProfile = modifs;
        SceneManager.LoadSceneAsync(testInt);
    }

    public static void NextMode()
    {
        /*int nextmode = Random.Range(1, maxmode);
        if(nextmode == lastMode)
        {
            if (lastMode == maxmode)
            {
                nextmode--;
            }
        }*/
        SceneManager.LoadSceneAsync(testInt);
    }
    public void LaunchSettings()
    {
        SceneManager.LoadSceneAsync("Settings", LoadSceneMode.Additive);
    }
    public static void BackToMenu()
    {
        SceneManager.LoadSceneAsync("_MainMenu");
    }

    public void ManualBackToMenu()
    {
        SceneManager.LoadSceneAsync("_MainMenu");
    }

    public void StartMulti()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}
