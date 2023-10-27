using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuHandler : MonoBehaviour
{
    public GameManager manager;
    public AudioHandler audioHandler;
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
            manager.paused = false;
        }
        else if (Name == "Quit")
        {
            manager.CallCommand("quit");
            manager.paused = false;
        }
        else if (Name == "MainMenu")
        {
            manager.paused = false;
            manager.CallCommand("mainmenu");
        }
        else if (Name == "Resume")
        {
            manager.CallCommand("resume");
        }
        else
        {
            Debug.Log(Name + " is not even acknowledged.. :|");
        }
    }

    public void Slider(string Name)
    {
        if (Name == "Volume")
        {
            audioHandler.SetVolume(gameObject.GetComponent<UnityEngine.UI.Slider>().value);
        }
    }
}
