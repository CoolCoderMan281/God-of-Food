using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuHandler : MonoBehaviour
{
    public GameManager manager;
    public AudioHandler audioHandler;
    public BasketController basketController;
    public void Start()
    {
        audioHandler = GameObject.Find("AudioOwner").GetComponent<AudioHandler>();
        basketController = GameObject.Find("Basket").GetComponent <BasketController>();
    }

    public void Click(string Name)
    {
        Debug.Log("Main Menu " + Name + " selected");
        if (Name == "Endless")
        {
            manager.StartEndless();
            manager.MainMenu = false;
            manager.paused = false;
        }
        else if (Name == "Story")
        {
            manager.StartStory();
            manager.MainMenu = false;
            manager.paused = false;
        }
        else if (Name == "ExitIntro")
        {
            manager.Intro = false;
            manager.Levels_IntroOver();
        }
        else if (Name == "Credits")
        {
            Debug.Log("Not implemented!");
        }
        else if (Name == "Settings")
        {
            manager.SettingsMenu.SetActive(true);
            manager.paused = true;
        }
        else if (Name == "Back")
        {
            manager.SettingsMenu.SetActive(false);
            manager.LevelSelectMenu.SetActive(false);
            manager.paused = false;
        }
        else if (Name == "Quit")
        {
            audioHandler.StopMusic();
            manager.CallCommand("quit");
            manager.paused = false;
        }
        else if (Name == "MainMenu")
        {
            manager.paused = false;
            manager.CallCommand("mainmenu");
            audioHandler.StopMusic();
        }
        else if (Name == "Resume")
        {
            manager.CallCommand("resume");
        }
        else if (Name == "LevelSelect")
        {
            manager.LevelSelectMenu.SetActive(true);
        }
        else if (Name == "Tutorial")
        {
            Debug.LogWarning("No tutorial yet hehe!");
        }
        else if (Name == "Level1")
        {
            manager.LevelSelectMenu.SetActive(false);
            manager.StartStory();
            manager.MainMenu = false;
            manager.paused = false;
            audioHandler.PlayAudio(audioHandler.starvingHarvest);
        }
        else if (Name == "Level2")
        {
            Debug.LogWarning("No Level2 yet hehe!");
        }
        else if (Name == "Level3")
        {
            Debug.LogWarning("No Level3 yet hehe!");
        }
        else if (Name == "ToggleMusic")
        {
            audioHandler.PlayMusic = !audioHandler.PlayMusic;
            if (!audioHandler.PlayMusic)
            {
                audioHandler.currentMusic.volume = 0;
            } else
            {
                audioHandler.currentMusic.volume = audioHandler.Volume;
            }
            Debug.Log("PlayMusic: " + audioHandler.PlayMusic);
        }
        else if (Name == "ToggleSFX")
        {
            audioHandler.PlaySFX = !audioHandler.PlaySFX;
            Debug.Log("PlaySFX: "+audioHandler.PlaySFX);
        }
        else if (Name == "ToggleBasketSmooth")
        {
            basketController.Slow = !basketController.Slow;
            Debug.Log("BasketSmoothing: "+basketController.Slow);
        }
        else
        {
            Debug.Log(Name + " is not even acknowledged.. :|");
        }
        audioHandler.PlayAudio(audioHandler.next);
    }

    public void Slider(string Name)
    {
        if (Name == "Volume")
        {
            audioHandler.SetVolume(gameObject.GetComponent<UnityEngine.UI.Slider>().value);
        }
    }
}
