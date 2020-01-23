using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMatGround : MonoBehaviour
{
    public Material newMat;

    // Start is called before the first frame update
    void Start()
    {
        Renderer myRenderer = GetComponent<Renderer>();

        List<Material> allMat = new List<Material>();

        allMat.AddRange(myRenderer.materials);

        allMat.Add(newMat);

        myRenderer.materials = allMat.ToArray();
    }
}
