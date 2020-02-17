using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;
    [SerializeField] MeshRenderer marble;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

   void UpdatePreview(Texture2D newText, Color colorOfTheLogo)
   {
        Material updatedMaterial;
        //updatedMaterial.SetColor()
    //    marble.material = updatedMaterial;
   }
}
