using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMode : MonoBehaviour
{
    public GameObject DeathMatch;
    public GameObject KingOfTheHill;
    public GameObject Hue;
    public GameObject Coins;

    public GameObject DMObj;
    public GameObject HillObj;
    public GameObject HueObj;
    public GameObject CoinsObj;

    bool DMBool = true;
    bool HillBool = false;
    bool HueBool = false;
    bool CoinsBool = false;

    public void SetDeathMatch(GameObject image)
    {
        DMBool = !DMBool;
        DMObj.SetActive(DMBool);
        image.SetActive(DMBool);

        image.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
        image.transform.parent.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);

        if (DMBool)
        {
            image.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
            image.transform.parent.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
        }
        else
        {
            image.GetComponent<Image>().color = Color.white;
            image.transform.parent.GetComponent<Image>().color = Color.white;
        }

        RoomSettings.Instance.deathmatch = DMBool;
    }

    public void SetHill(GameObject image)
    {
        HillBool = !HillBool;
        HillObj.SetActive(HillBool);
        image.SetActive(HillBool);

        image.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
        image.transform.parent.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);

        if (HillBool)
        {
            image.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
            image.transform.parent.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
        }
        else
        {
            image.GetComponent<Image>().color = Color.white;
            image.transform.parent.GetComponent<Image>().color = Color.white;
        }
        RoomSettings.Instance.hill = HillBool;
    }

    public void SetHue(GameObject image)
    {
        HueBool = !HueBool;
        HueObj.SetActive(HueBool);
        image.SetActive(HueBool);

        if (HueBool)
        {
            image.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
            image.transform.parent.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
        }
        else
        {
            image.GetComponent<Image>().color = Color.white;
            image.transform.parent.GetComponent<Image>().color = Color.white;
        }
        RoomSettings.Instance.hue = HueBool;
    }

    public void SetCoins(GameObject image)
    {
        CoinsBool = !CoinsBool;
        CoinsObj.SetActive(CoinsBool);
        image.SetActive(CoinsBool);

        if (CoinsBool)
        {
            image.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
            image.transform.parent.GetComponent<Image>().color = new Color(1, 0.1986281f, 0);
        }
        else
        {
            image.GetComponent<Image>().color = Color.white;
            image.transform.parent.GetComponent<Image>().color = Color.white;
        }
        RoomSettings.Instance.coins = CoinsBool;
    }
}
