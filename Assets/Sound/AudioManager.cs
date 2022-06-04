using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.AudioSource = gameObject.AddComponent<AudioSource>();
            s.AudioSource.clip = s.AudioClip;
            s.AudioSource.volume = s.volume;
            s.AudioSource.pitch = s.pitch;
            s.AudioSource.loop = s.loop;
            // s.AudioSource.Play();
        }
    }
    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (!s.AudioSource.isPlaying)
        {
            s.AudioSource.PlayOneShot(s.AudioClip);
            print(soundName);
        }
    }public void PlayDelay(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (!s.AudioSource.isPlaying)
        {
            s.AudioSource.PlayDelayed(0.3f);
            print(soundName);
        }
    }
    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        s.AudioSource.Stop();
    }
}
