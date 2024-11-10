using System;
using System.Collections;
using System.Collections.Generic;
using EHTool;
using UnityEngine;

public class SFXManager : MonoSingleton<SFXManager>
{
    private AudioSource audioSource;

    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;
    }

    [System.Serializable]
    public struct BackgroundMusic
    {
        public string name;
        public AudioClip clip;
    }

    public SoundEffect[] soundEffects;
    public BackgroundMusic[] backgroundMusics;

    private Dictionary<string, AudioClip> sfxDictionary;
    private Dictionary<string, AudioClip> bgmDictionary;

    protected override void OnCreate()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;  // SFX는 기본적으로 loop를 끄고 사용

        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (SoundEffect sfx in soundEffects)
        {
            sfxDictionary.Add(sfx.name, sfx.clip);
        }

        bgmDictionary = new Dictionary<string, AudioClip>();
        foreach (BackgroundMusic bgm in backgroundMusics)
        {
            bgmDictionary.Add(bgm.name, bgm.clip);
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

    public void PlayBGM(string name)
    {
        if (bgmDictionary.TryGetValue(name, out AudioClip clip))
        {
            if (audioSource.clip == clip && audioSource.isPlaying) return; // Avoid replaying the same track
            audioSource.clip = clip;
            audioSource.loop = true; // BGM은 루프 재생
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{name}' not found!");
        }
    }

    public void StopBGM()
    {
        if (audioSource.isPlaying && audioSource.loop) // BGM만 중지
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false; // SFX 재생 시 loop 비활성화
        }
    }

    public void PauseBGM()
    {
        if (audioSource.isPlaying && audioSource.loop)
        {
            audioSource.Pause();
        }
    }

    public void ResumeBGM()
    {
        if (audioSource.clip != null && audioSource.loop && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }

    public void SetBGMVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
}
