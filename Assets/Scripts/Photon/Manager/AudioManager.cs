using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource myAudioSource;

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayThisSound(AudioClip _audio)
    {
        myAudioSource.PlayOneShot(_audio);
    }

    public void PlaySoundAtPoint(AudioClip _audio, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(_audio, position);
    }
}
