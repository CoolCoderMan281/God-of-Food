using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class AudioHandler : MonoBehaviour
{
    [Header("Sound effects")]
    public AudioSource foodCatch;
    public AudioSource foodMiss;
    public AudioSource next;
    public AudioSource winSound;
    public AudioSource tick;
    public AudioSource moldyCatch;
    public AudioSource moldyMiss;
    [Header("Music")]
    public AudioSource starvingHarvest;
    public AudioSource currentMusic;
    private List<AudioSource> SFX = new List<AudioSource>();
    private List<AudioSource> MUSIC = new List<AudioSource>();
    [Header("Settings")]
    public float Volume = 1.0f;
    public UnityEngine.UI.Slider VolumeSlider;
    public TMP_Text VolumeLabel;
    public bool PlaySFX = true;
    public bool PlayMusic = true;
    private List<AudioSource> ActiveAudio = new List<AudioSource>();
    public void PlayAudio(AudioSource audiosource)
    {
        audiosource.volume = Volume;
        if (SFX.Contains(audiosource) && PlaySFX)
        {
            audiosource.Play();
            ActiveAudio.Add(audiosource);
        }
        else if (MUSIC.Contains(audiosource) && PlayMusic)
        {
            if (currentMusic != null) { currentMusic.Stop(); }
            audiosource.loop = true;
            currentMusic = audiosource;
            StartCoroutine(FadeInAudio(audiosource));
        }
    }

    public void StopAudio(AudioSource audiosource) { audiosource.Stop(); }
    public void StopMusic()
    { 
        if (currentMusic != null) 
        { 
            StartCoroutine(FadeOutAudio(currentMusic));
            currentMusic = null;
        }
    }

    public IEnumerator FadeInAudio(AudioSource source)
    {
        source.Play();
        for (float i = 0; i <= Volume; i += Time.deltaTime)
        {
            source.volume = i;
            yield return null;
        }
    }

    public IEnumerator FadeOutAudio(AudioSource source)
    {
        float startingVolume = source.volume;
        for (float i = startingVolume; i > 0; i -= Time.deltaTime)
        {
            source.volume = i;
            yield return null;
        }
        source.Stop();
    }

    public void Start()
    {
        // Define SFX
        SFX.Add(foodCatch);
        SFX.Add(foodMiss);
        SFX.Add(next);
        SFX.Add(winSound);
        SFX.Add(tick);
        SFX.Add(moldyCatch);
        SFX.Add(moldyMiss);
        // Define music
        MUSIC.Add(starvingHarvest);
        // Do volume
        SetVolume(1);
        VolumeSlider.value = Volume;
        PlaySFX = true;
        PlayMusic = true;
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
        VolumeLabel.text = "Volume: " + (int)(Volume * 100) + "%";
        foreach (AudioSource audioSource in ActiveAudio)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }
        if (currentMusic != null && currentMusic.isPlaying && currentMusic.volume != 0)
        {
            currentMusic.volume = volume;
        }
    }
}
