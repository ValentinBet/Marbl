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

    [Header("PlayerInfo")]
    [SerializeField] Sc_Skininfos playerSkinInfos;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);


        marble.material = playerSkinInfos.playerSkin;
    }

   public void UpdatePreview(Material newMaterial)
   {
        Material prepMaterial = new Material(newMaterial);

        currentMaterial = newMaterial;
        marble.material = newMaterial;
        changeFeedback.Play();
   }

    public void SetRed()
    {
        marble.material.SetColor("_MarbleColor",new Color(255,0,0,255));
    }
    public void SetBlue()
    {
        marble.material.SetColor("_MarbleColor", new Color(0, 184, 255, 255));
    }
    public void SetGreen()
    {
        marble.material.SetColor("_MarbleColor", new Color(0, 255, 0, 255));
    }
    public void SetYellow()
    {
        marble.material.SetColor("_MarbleColor", new Color(255, 226, 0, 255));
    }

    public void SetNewMaterial()
    {
        playerSkinInfos.playerSkin = currentMaterial;
    }

    public void Cancel()
    {
        playerSkinInfos.playerSkin = oldMaterial;
        UpdatePreview(oldMaterial);
        changeFeedback.Play();
    }
}
