using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public GameManager manager;
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
            Debug.Log("Not implemented!");
        }
        else if (Name == "Quit")
        {
            manager.CallCommand("quit");
            manager.paused = false;
        }
        else if (Name == "MainMenu")
        {
            manager.CallCommand("resume");
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
}
