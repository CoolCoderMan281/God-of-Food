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
    public GameObject ExitText_Img;
    public GameObject ExitText_Btn;
    public GameObject ExitComic_Img;
    public GameObject ExitComic_Btn;
    public string LastSelectedLevel = "BRUH";
    public void Start()
    {
        audioHandler = GameObject.Find("AudioOwner").GetComponent<AudioHandler>();
        basketController = GameObject.Find("Basket").GetComponent <BasketController>();
    }

    public void Click(string Name)
    {
        Debug.Log("Main Menu " + Name + " selected");
        if (Name.StartsWith("Level") && !Name.EndsWith("Select"))
        {
            LastSelectedLevel = Name;
            Debug.Log("LastSelectedLevel = " + LastSelectedLevel);
        }
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
            manager.CreditsMenu.SetActive(true);
            manager.paused = true;
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
            manager.CreditsMenu.SetActive(false);
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
            manager.LevelSelectMenu.SetActive(false);
            manager.StartTutorial();
            manager.MainMenu = false;
            manager.paused = false;
            audioHandler.PlayAudio(audioHandler.sugarDrunk);
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
            manager.LevelSelectMenu.SetActive(false);
            manager.StartStory2();
            manager.MainMenu = false;
            manager.paused = false;
            audioHandler.PlayAudio(audioHandler.beatsNJams);
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
        else if (Name == "Restart")
        {
            Debug.Log("Restart! "+LastSelectedLevel);
            Click("Level1");
            if (LastSelectedLevel.StartsWith("Level"))
            {
                Click(LastSelectedLevel);
            } else
            {
                Debug.Log("Nothing to restart ?? :/");
            }
        } else if (Name == "ExitText")
        {
            ExitText_Img.SetActive(false);
            ExitText_Btn.SetActive(false);
        } else if (Name == "ExitComic")
        {
            ExitComic_Img.SetActive(false);
            ExitComic_Btn.SetActive(false);
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
