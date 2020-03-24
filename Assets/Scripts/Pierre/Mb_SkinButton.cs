using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Mb_SkinButton : MonoBehaviour
{
    [SerializeField] Image selectionedFeedBack;
    [SerializeField] Image SkinPreview;

    public Material associatedMaterial;

    public void UpdateFeedBack(Color colorOfTheOutline)
    {
        selectionedFeedBack.color = colorOfTheOutline;
    }

    public void CleanOutline()
    {
        selectionedFeedBack.color = Color.clear;
    }

    public void SetPlayerSkin()
    {
        SkinSetter.instance.UpdatePreview(associatedMaterial, this);
    }

}
