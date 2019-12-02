using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PodiumDirty : MonoBehaviour
{

    [SerializeField] private GameObject[] podiums;
    [SerializeField] private ScoreManager sm;
    Image[] playerBars;

    public void activateRight()
    {
        for (int i = 0; i < podiums.Length;i++)
        {
            if (i + 2 == GameData.playerAmount)
            {
                podiums[i].SetActive(true);
            } else
            {
                podiums[i].SetActive(false);
            }
        }
    }

    public void UpdateScores()
    {
        playerBars = podiums[GameData.playerAmount - 2].transform.GetComponentsInChildren<Image>();
        StartCoroutine("EndGame");
        for (int i = 0; i < GameData.playerAmount; i++)
        {
            playerBars[i].fillAmount = (float)(1 + GameData.playersScore[i]) / (float)(1+GameData.scoreToWin);
        }
    }

    private IEnumerator EndGame()
    {
        float timer = 0.0f;
        while(timer<1.0f)
        {
            yield return null;
            timer += Time.deltaTime;
            for (int i = 0; i < GameData.playerAmount; i++)
            {
                playerBars[i].fillAmount = (float)Mathf.Lerp(1.0f,(1 + GameData.playersScore[i]),timer) / (float)(1 + GameData.scoreToWin);
            }
        }
        for (int i = 0; i < GameData.playerAmount; i++)
        {
            playerBars[i].fillAmount = (float)(1 + GameData.playersScore[i])/ (float)(1 + GameData.scoreToWin);
        }
        yield return new WaitForSeconds(7f);
        sm.EndingTransition();
    }

}
