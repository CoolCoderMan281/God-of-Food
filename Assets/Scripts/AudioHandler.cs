using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class AudioHandler : MonoBehaviour
{
    public AudioSource foodCatch;
    public AudioSource foodMiss;
    public AudioSource next;
    public AudioSource winSound;

    public float Volume = 1.0f;
    public UnityEngine.UI.Slider VolumeSlider;
    public TMP_Text VolumeLabel;
    public void PlayAudio(AudioSource audiosource)
    {
        audiosource.volume = Volume;
        audiosource.Play();
    }

    public void Start()
    {
        SetVolume(1);
        VolumeSlider.value = Volume;
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
        VolumeLabel.text = "Volume: " + (int)(Volume * 100) + "%";
    }
}
