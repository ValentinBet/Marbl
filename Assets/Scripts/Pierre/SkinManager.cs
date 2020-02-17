using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;

    [Header("Preview")]
    [SerializeField] MeshRenderer marble;
    [SerializeField] ParticleSystem changeFeedback;
    Material currentMaterial;
    Material oldMaterial;
    ColorType colorType;

    [Header("PlayerInfo")]
    [SerializeField] Sc_Skininfos playerSkinInfos;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        oldMaterial = playerSkinInfos.playerSkin;
        marble.material = playerSkinInfos.playerSkin;
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
    }

    public void SetRed()
    {
        marble.material.SetColor("_MarbleColor", new Color(1, 0, 0, 1));
        colorType = ColorType.Red;
    }
    public void SetBlue()
    {
        marble.material.SetColor("_MarbleColor", new  Color(0, 0.72f, 1, 1));
        colorType = ColorType.Blue;
    }
    public void SetGreen()
    {
        marble.material.SetColor("_MarbleColor", new Color(0, 1, 0, 1));
        colorType = ColorType.Green;
    }
    public void SetYellow()
    {
        marble.material.SetColor("_MarbleColor", new Color(1, .88f, 0, 1));
        colorType = ColorType.Yellow;
    }

    public void SetNewMaterial()
    {
        oldMaterial = currentMaterial;
        playerSkinInfos.playerSkin = currentMaterial;
    }

    public void Cancel()
    {
        playerSkinInfos.playerSkin = oldMaterial;
        UpdatePreview(oldMaterial);
        changeFeedback.Play();
    }

    public enum ColorType
    {
        Red, Yellow,Blue,Green
    }
}
