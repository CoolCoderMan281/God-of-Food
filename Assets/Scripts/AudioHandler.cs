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
    [Header("Music")]

    [Header("Settings")]
    public float Volume = 1.0f;
    public UnityEngine.UI.Slider VolumeSlider;
    public TMP_Text VolumeLabel;
    public bool PlaySFX = true;
    public bool PlayMusic = true;
    public void PlayAudio(AudioSource audiosource)
    {
        audiosource.volume = Volume;
        if (SFX.Contains(audiosource) && PlaySFX)
        {
            audiosource.Play();
        }
        else if (MUSIC.Contains(audiosource) && PlayMusic)
        {
            if (currentMusic != null) { currentMusic.Stop(); }
            audiosource.loop = true;
            audiosource.Play();
            currentMusic = audiosource;
        }
    }

    public void StopAudio(AudioSource audiosource) { audiosource.Stop(); }
    public void StopMusic() { if (currentMusic != null) { currentMusic.Stop(); }}

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
    }
}
