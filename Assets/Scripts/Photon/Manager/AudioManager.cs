using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource myAudioSource;
    public AudioClip ballDeath;
    public AudioClip menuSong;

    public AudioSource backSong;
    public AudioSource playingSong;

    
    bool playBack = true;
    bool playPlaying = false;

    float timeToTransit = 0.75f;
    float musicVolume;

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

        DontDestroyOnLoad(this);
    }


    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayThisSound(AudioClip _audio,float volume = 0.5f)
    {
        myAudioSource.PlayOneShot(_audio, volume);
    }

    public void PlaySoundAtPoint(AudioClip _audio, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(_audio, position);
    }

    private void Update()
    {
        musicVolume = Mathf.Clamp(InputManager.Instance.Inputs.inputs.GeneralVolume, 0f, 0.6f);

        if (playBack)
        {
            backSong.volume = Mathf.Lerp(backSong.volume, musicVolume, timeToTransit * Time.deltaTime);
        }
        else
        {
            backSong.volume = Mathf.Lerp(backSong.volume, 0, timeToTransit * Time.deltaTime);
        }

        if (playPlaying)
        {
            playingSong.volume = Mathf.Lerp(playingSong.volume, musicVolume, timeToTransit * Time.deltaTime);
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
