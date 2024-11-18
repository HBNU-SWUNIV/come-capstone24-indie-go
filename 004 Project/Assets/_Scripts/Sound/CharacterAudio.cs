using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    private AudioHandler audioHandler;
    private Transform soundParent;

    void Start()
    {
        audioHandler = new AudioHandler(gameObject);
    }
    public void PlayOneShotSound(string audioName)
    {
        audioHandler.PlayOneShotSound(audioName);
    }

    public void PlaySound(string audioName)
    {
        audioHandler.PlaySound(audioName);
    }
    public void PlayHitSound(string hitSoundName = "Hit")
    {
        audioHandler.PlayOneShotSound(hitSoundName);
    }

    public void PlayDeathSound(string deathSoundName = "Death")
    {
        PlaySoundAtPosition(deathSoundName, transform.position, GetSoundParent());
    }

    private void PlaySoundAtPosition(string soundName, Vector3 position, Transform parent = null)
    {
        GameObject tempAudio = new GameObject("TempAudio");
        tempAudio.transform.position = position;

        if (parent != null)
        {
            tempAudio.transform.SetParent(parent);
        }

        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        AudioClip clip = GameManager.SoundManager.GetSound(soundName);

        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();

            Destroy(tempAudio, clip.length);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in SoundManager.");
            Destroy(tempAudio); 
        }
    }

    private Transform GetSoundParent()
    {
        if (soundParent == null)
        {
            GameObject parentObj = new GameObject("SoundParent");
            soundParent = parentObj.transform;
        }
        return soundParent;
    }
}