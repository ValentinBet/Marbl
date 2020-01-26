using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingSettings : MonoBehaviour
{
    public SpriteRenderer mainSprite;
    public SpriteRenderer pingSprite;

    public AudioSource audioSource;

    private void Update()
    {
        if (audioSource != null && InputManager.Instance != null)
        {
            audioSource.volume = InputManager.Instance.Inputs.inputs.GeneralVolume / 2;
        }
    }
}
