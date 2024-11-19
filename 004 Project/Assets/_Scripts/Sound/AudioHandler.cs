using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler
{
    private AudioSource audioSource;

    public AudioHandler(GameObject target)
    {
        audioSource = target.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = target.AddComponent<AudioSource>();
        }
    }
    public void PlayOneShotSound(string soundName)
    {
        AudioClip clip = GameManager.SoundManager.GetSound(soundName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = GameManager.SoundManager.GetSound(soundName);
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
