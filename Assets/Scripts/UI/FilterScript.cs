using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterScript : MonoBehaviour
{
    public GameObject DeathMatch;
    public GameObject KingOfTheHill;
    public GameObject hue;
    public GameObject Custom;

    public List<GameObject> DMList;
    public List<GameObject> HillList;
    public List<GameObject> HueList;
    public List<GameObject> CustomList;

    bool DMBool = true;
    bool HillBool = true;
    bool HueBool = true;
    bool CustomBool = false;

    public void SetDeathMatch(GameObject image)
    {
        DMBool = !DMBool;
        ActiveDisable(DMList, DMBool);
        image.SetActive(DMBool);
    }

    public void SetHill(GameObject image)
    {
        HillBool = !HillBool;
        ActiveDisable(HillList, HillBool);
        image.SetActive(HillBool);
    }

    public void SetHue(GameObject image)
    {
        HueBool = !HueBool;
        ActiveDisable(HueList, HueBool);
        image.SetActive(HueBool);
    }

    public void SetCustom(GameObject image)
    {
        CustomBool = !CustomBool;
        ActiveDisable(CustomList, CustomBool);
        image.SetActive(CustomBool);
    }

    void ActiveDisable(List<GameObject> myList, bool value)
    {
        foreach(GameObject element in myList)
        {
            element.SetActive(value);
        }
    }

    public enum filter
    {
        Deathmatch,
        KingOfTheHill,
        Hue,
        Custom
    }
}
