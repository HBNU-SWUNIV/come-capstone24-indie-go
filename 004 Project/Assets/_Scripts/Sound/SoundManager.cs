using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private Dictionary<string, AudioClip> soundClips;

    public void LoadAllSounds()
    {
        soundClips = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");

        foreach (AudioClip clip in clips)
        {
            soundClips[clip.name] = clip;
        }

        Debug.Log($"Loaded {soundClips.Count} sounds into SoundManager.");
    }

    public AudioClip GetSound(string soundName)
    {
        if (soundClips.TryGetValue(soundName, out AudioClip clip))
        {
            return clip;
        }

        Debug.LogWarning($"Sound '{soundName}' not found in SoundManager.");
        return null;
    }
}
