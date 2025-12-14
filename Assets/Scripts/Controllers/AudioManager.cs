using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There are two AudioManager!");
        }
        instance = this;
    }

    public void PlayMusic(string name, float volume = 1f)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s != null) musicSource.clip = s.clip;
        musicSource.volume = volume;
        musicSource.Play();
    }


    public void PlaySfx(string name, float volume = 1f)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s != null) sfxSource.clip = s.clip;
        sfxSource.volume = volume;
        sfxSource.Play();
    }

    public void StopSfx()
    {
        sfxSource.Stop();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}
