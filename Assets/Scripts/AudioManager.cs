using UnityEngine;

using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sounds[] sounds;

    public AudioSource Source, BG;

    void Awake()
    {
        
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Play(string sound)
    {
        Sounds s = Array.Find(sounds, item => item.name == sound);

        Source.clip = s.clip;
        Source.volume = s.volum;
        Source.pitch = s.pitch;
        Source.Play();
    }
}