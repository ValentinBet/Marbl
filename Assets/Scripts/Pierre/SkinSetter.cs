using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinSetter : MonoBehaviour
{
    public static SkinSetter instance;

    [Header("Preview")]
    [SerializeField] MeshRenderer marble;
    [SerializeField] ParticleSystem changeFeedback;
    Material currentMaterial;
    Material oldMaterial;
    ColorType colorType;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        int skinIndex = 0;
        
        if (PlayerPrefs.HasKey("Skin") == true)
        {
            skinIndex = PlayerPrefs.GetInt("Skin");
        }

        currentMaterial = Mb_SkinManager.Instance.playerSkinScriptable.allskins[skinIndex];
   
        oldMaterial = Mb_SkinManager.Instance.playerSkinScriptable.allskins[skinIndex];

        UpdatePreview(oldMaterial);
        SetRed();
    }

    public void UpdatePreview(Material newMaterial)
    {
        currentMaterial = newMaterial;
        marble.material = newMaterial;
        switch (colorType)
        {
            case ColorType.Red:
                SetRed();
                break;
            case ColorType.Blue:
                SetBlue();
                break;
            case ColorType.Green:
                SetGreen();
                break;
            case ColorType.Yellow:
                SetYellow();
                break;
        }
        changeFeedback.Play();

        PlayerPrefs.SetInt("Skin", GetIndexOfTheSkin(currentMaterial));
    }

    public void SetRed()
    {
        marble.material.SetColor("_Color", new Color(1, 0, 0, 1));
        colorType = ColorType.Red;
    }
    public void SetBlue()
    {
        marble.material.SetColor("_Color", new  Color(0, 0.72f, 1, 1));
        colorType = ColorType.Blue;
    }
    public void SetGreen()
    {
        marble.material.SetColor("_Color", new Color(0, 1, 0, 1));
        colorType = ColorType.Green;
    }
    public void SetYellow()
    {
        marble.material.SetColor("_Color", new Color(1, .88f, 0, 1));
        colorType = ColorType.Yellow;
    }

    public void SetNewMaterial()
    {
        oldMaterial = currentMaterial;
        Mb_SkinManager.Instance.playerSkinScriptable.skinIndex = GetIndexOfTheSkin(currentMaterial);
    }

    public void Cancel()
    {
        Mb_SkinManager.Instance.playerSkinScriptable.skinIndex = GetIndexOfTheSkin(oldMaterial);
        UpdatePreview(oldMaterial);
        changeFeedback.Play();
    }

    int GetIndexOfTheSkin(Material skin)
    {
        for(int i = 0; i < Mb_SkinManager.Instance.playerSkinScriptable.allskins.Length; i++)
        {
            if (Mb_SkinManager.Instance.playerSkinScriptable.allskins[i] == skin)
                return i;
        }

        return 0;
    }

    public enum ColorType
    {
        Red, Yellow,Blue,Green
    }

    public void CloseMenu()
    {
        SceneManager.LoadScene("_MainMenu", LoadSceneMode.Single);
    }
}
