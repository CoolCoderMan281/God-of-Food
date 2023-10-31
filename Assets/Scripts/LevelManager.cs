using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Level> levels = new List<Level>();
    public Level SelectedLevel;
    private Level defaultLevel;
    private GameManager manager;
    public CameraController cameraController;
    [Header("Audio")]
    public AudioHandler audioHandler;
    [Header("Intro & Background")]
    public GameObject ExitIntro;
    public GameObject NoBackground;
    public GameObject Background1;
    public GameObject Background2;
    public GameObject VictoryBackground;
    public GameObject LevelIntro_Placeholder;
    public GameObject Level1_Intro;
    public GameObject Level2_Intro;
    public GameObject Tutorial_Intro;
    // Start is called before the first frame update
    void Start()
    {
        // Define game manager
        manager = gameObject.GetComponent<GameManager>();
        LevelIntro_Placeholder.SetActive(false);
        NoBackground.SetActive(true);
        Background1.SetActive(false);
        Background2.SetActive(false);
        VictoryBackground.SetActive(false);
        ExitIntro.SetActive(false);
        Level1_Intro.SetActive(false);
        Level2_Intro.SetActive(false);
        Tutorial_Intro.SetActive(false);
        Level MainMenu = new Level("MainMenu", "Its the main menu", -1, GameManager.SectionStyle.IDLE, GameManager.SpawningBehavior.STANDARD,0f,background:NoBackground,setPoints:0);
        // Campaign 1
        Level SurvivalMode = new Level("SurvivalMode", "Survival mode style", 1, GameManager.SectionStyle.SURVIVE,
                                        GameManager.SpawningBehavior.HARSH_STANDARD,requiredPoints: 20, duration: 30, spawnbuffer:0.1f,background:Background2,zoom:true, levelrelation: 1);
        Level NormalMode = new Level("NormalMode", "Normal style example", 0, GameManager.SectionStyle.NORMAL,
                                        GameManager.SpawningBehavior.STANDARD,requiredPoints: 20, spawnbuffer:0.3f,
                                        background:Background2,zoom:false,levelrelation:1,intro:Level1_Intro);
        // Campaign 2
        Level SurvivalMode2 = new Level("SurvivalMode2", "Survival mode style2", 3, GameManager.SectionStyle.SURVIVE,
                                        GameManager.SpawningBehavior.HARSH_STANDARD, requiredPoints: 20, duration: 30, spawnbuffer: 0.1f, background: Background1, zoom: true, levelrelation: 2);
        Level NormalMode2 = new Level("NormalMode2", "Normal style example2", 2, GameManager.SectionStyle.NORMAL,
                                        GameManager.SpawningBehavior.STANDARD, requiredPoints: 20, spawnbuffer: 0.3f,
                                        background: Background1, zoom: false, levelrelation: 2, intro: Level2_Intro);
        // Tutorial
        Level TutorialNormal = new Level("TutorialNormal", "Normal style example3", 4, GameManager.SectionStyle.NORMAL,
                                        GameManager.SpawningBehavior.STANDARD, requiredPoints: 20, spawnbuffer: 0.3f,
                                        background: Background1, zoom: false, intro: Tutorial_Intro);
        Level TutorialSurvive = new Level("TutorialSurvive", "Survival mode style3", 5, GameManager.SectionStyle.SURVIVE,
                                        GameManager.SpawningBehavior.HARSH_STANDARD, requiredPoints: 20, duration: 30, spawnbuffer: 0.1f, background: Background1, zoom: true);
        // [VICTORY LIMBO]
        Level Victory = new Level("Victory", "Victory level, limbo kind of", 102, GameManager.SectionStyle.IDLE, GameManager.SpawningBehavior.BINGUS_DESTROYER_OF_FUN, 0.1f,
                                    background: NoBackground, intro:VictoryBackground);
        // [ENDLESS] Create the endless mode
        Level EndlessMode = new Level("Endless", "Endless mode", 101, GameManager.SectionStyle.ENDLESS, GameManager.SpawningBehavior.STANDARD, 0.5f,background:NoBackground);
        // Change next and back
        SurvivalMode.Next = Victory;
        SurvivalMode.Back = NormalMode;
        NormalMode.Next = SurvivalMode;
        Victory.Next = MainMenu;
        NormalMode2.Next = SurvivalMode2;
        SurvivalMode2.Next = Victory;
        SurvivalMode2.Back = NormalMode2;
        TutorialNormal.Next = TutorialSurvive;
        TutorialSurvive.Next = Victory;
        TutorialSurvive.Back = TutorialNormal;
        // Add levels to levels list
        levels.Add(MainMenu);
        levels.Add(NormalMode);
        levels.Add(SurvivalMode);
        levels.Add(EndlessMode);
        levels.Add(Victory);
        levels.Add(SurvivalMode2);
        levels.Add(NormalMode2);
        levels.Add(TutorialNormal);
        levels.Add(TutorialSurvive);
        // End of start
        if (levels.Count() >= 1)
        {
            defaultLevel = levels[0];
        } else
        {
            Debug.LogError("No levels were registered!");
        }
        Debug.Log("LevelManager ready!");
    }

    public void SplashComplete()
    {
        Invoke(nameof(FirstLevel), 0.5f);
    }

    public void FirstLevel()
    {
        //StartLevel(defaultLevel);
        Debug.LogWarning("ignored default level. This is bug tracking");
    }
    public void StartLevel(Level lvl)
    {
        SelectedLevel = lvl;
        if (SelectedLevel.ID == 102)
        {
            audioHandler.StopMusic();
            audioHandler.PlayAudio(audioHandler.winSound);
        }
        NoBackground.SetActive(false);
        if (SelectedLevel.Background != null)
        {
            SelectedLevel.Background.SetActive(true);
        } else
        {
            SelectedLevel.Background = NoBackground;
            SelectedLevel.Background.SetActive(true);
        }
        manager.Levels_Start(SelectedLevel);
    }
    public void EndLevel(Level next=null) // Called by GameManager
    {
        if (SelectedLevel != null)
        {
            SelectedLevel.Background.SetActive(false);
            Debug.Log("Ending level #" + SelectedLevel.ID);
        }
        if (next != null)
        {
            StartLevel(next);
        } else
        {
            StartLevel(defaultLevel);
        }
    }
    public void RequestLevelSwitch(Level lvl) // Called by GameManager
    {
        Debug.Log("Switching to level #" + lvl.ID);
        EndLevel(lvl);
    }

    public void RequestLevelSwitch(int ID)
    {
        foreach(Level _lvl in levels)
        {
            if (_lvl.ID == ID)
            {
                Debug.Log("Switching to level #" + ID);
                EndLevel(_lvl);
                return;
            }
        }
        Debug.Log("Didn't find a level with the id of " + ID);
    }

    public void OnDisable()
    {
        Debug.Log("LevelManager stopped!");
    }

    public void EndIntro()
    {
        if (SelectedLevel.Intro != null)
        {
            SelectedLevel.Intro.SetActive(false);
            if (SelectedLevel.Intro != VictoryBackground)
            {
                SelectedLevel.Intro = null;
            }
        }
        Debug.Log("No intro was displayed.. Why clear it");
        ExitIntro.SetActive(false);
        if (SelectedLevel.ID == 102) // Victory screen, Goto main menu
        {
            manager.CallCommand("mainmenu");
        }
    }
    public void ShowIntro()
    {
        if (SelectedLevel.Intro != null)
        {
            SelectedLevel.Intro.SetActive(true);
            Invoke(nameof(EnableExitIntro), 5f);
        }
    }

    public void EnableExitIntro()
    {
        ExitIntro.SetActive(true);
    }
}

public class Level
{
    public string Name;
    public string Description;
    public int ID;
    public GameManager.SectionStyle Style;
    public GameManager.SpawningBehavior Behavior;
    public float SpawnBuffer;
    public Level Back;
    public Level Next;
    public int SetPoints;
    public int RequiredPoints;
    public int Duration;
    public GameObject Intro;
    public GameObject Background;
    public int LevelRelation;
    //public GameObject Boss;
    public bool Zoom;
    public Level(string name, string desc, int id, GameManager.SectionStyle style, GameManager.SpawningBehavior behavior,
        float spawnbuffer, Level back=null, Level next=null, int setPoints=-127, int requiredPoints=-127,int duration=-127,
        GameObject intro=null, GameObject background=null, bool zoom=false, int levelrelation = -127)
    {
        Name = name;
        Description = desc;
        ID = id;
        Style = style;
        Behavior = behavior;
        SpawnBuffer = spawnbuffer;
        Back = back;
        Next = next;
        SetPoints = setPoints;
        RequiredPoints = requiredPoints;
        Duration = duration;
        Intro = intro;
        Background = background;
        Zoom = zoom;
        //Boss = boss;
        LevelRelation = levelrelation;
    }
}