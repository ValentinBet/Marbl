using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSettings : MonoBehaviour
{
    public Dropdown dropMap;
    public Image mapImg;

    //Map
    public List<Sprite> allMapImg;


    private void Start()
    {
        mapImg.sprite = allMapImg[dropMap.value];
    }

    public void ChangeMap()
    {
        mapImg.sprite = allMapImg[dropMap.value];
    }

    public void SetMap(int value)
    {
        dropMap.value = value;
        ChangeMap();
    }
}
