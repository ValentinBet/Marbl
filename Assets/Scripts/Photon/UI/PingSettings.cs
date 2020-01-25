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
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp(audioSource.volume, 0, 0.55f);
        }
    }
}
