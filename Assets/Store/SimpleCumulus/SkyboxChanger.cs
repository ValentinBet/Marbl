using UnityEngine;
using UnityEngine.UI;

public class SkyboxChanger : MonoBehaviour
{
    public Material[] Skyboxes;

    public void Awake()
    {
        ChangeSkybox();
    }

    public void ChangeSkybox()
    {
        RenderSettings.skybox = Skyboxes[Random.Range(0, Skyboxes.Length)];
        RenderSettings.skybox.SetFloat("_Rotation", 0);
    }
}