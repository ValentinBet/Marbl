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

   void UpdatePreview(Material updatedMaterial)
   {
        marble.material = updatedMaterial;
   }
}
