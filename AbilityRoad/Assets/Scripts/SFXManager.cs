using System.Collections;
using System.Collections.Generic;
using EHTool;
using UnityEngine;

public class SFXManager : MonoSingleton<SFXManager>
{
    private AudioSource audioSource;

    // 여러 SFX 클립을 관리하기 위한 Dictionary
    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;
    }

    public SoundEffect[] soundEffects;
    private Dictionary<string, AudioClip> sfxDictionary;

    protected override void OnCreate()
    {
        audioSource = GetComponent<AudioSource>();
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (SoundEffect sfx in soundEffects)
        {
            sfxDictionary.Add(sfx.name, sfx.clip);
        }
    }
    
    public void PlaySFX(string name)
    {
        if (sfxDictionary.TryGetValue(name, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' not found!");
        }
    }

}
