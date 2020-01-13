using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource myAudioSource;
    public AudioClip ballDeath;

    public AudioSource backSong;
    public AudioSource playingSong;

    bool playBack = false;
    bool playPlaying = false;

    float currentVolume = 0.2f;

    float timeToTransit = 30;


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

    private void Update()
    {
        if (playBack)
        {
            backSong.volume = Mathf.Lerp(backSong.volume, currentVolume, timeToTransit * Time.deltaTime);
        }
        else
        {
            backSong.volume = Mathf.Lerp(backSong.volume, 0, timeToTransit * Time.deltaTime);
        }

        if (playPlaying)
        {
            playingSong.volume = Mathf.Lerp(playingSong.volume, currentVolume, timeToTransit * Time.deltaTime);
        }
        else
        {
            playingSong.volume = Mathf.Lerp(playingSong.volume, 0, timeToTransit * Time.deltaTime);
        }
    }

    public void SetPlayingSong(bool value)
    {
        playPlaying = value;
    }

    public void SetBackSong(bool value)
    {
        playBack = value;
    }
}
