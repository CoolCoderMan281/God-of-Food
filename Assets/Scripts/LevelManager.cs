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

    public GameObject ExitIntro;
    public GameObject NoBackground;
    public GameObject Background1;
    public GameObject Background2;
    public GameObject LevelIntro_Placeholder;
    // Start is called before the first frame update
    void Start()
    {
        // Define game manager
        manager = gameObject.GetComponent<GameManager>();
        LevelIntro_Placeholder.SetActive(false);
        NoBackground.SetActive(true);
        Background1.SetActive(false);
        Background2.SetActive(false);
        ExitIntro.SetActive(false);
        // [TEST] Create survival level
        Level MainMenu = new Level("MainMenu", "Its the main menu", -1, GameManager.SectionStyle.IDLE, GameManager.SpawningBehavior.STANDARD,0f,background:NoBackground);
        Level SurvivalMode = new Level("SurvivalMode", "Survival mode style", 1, GameManager.SectionStyle.SURVIVE,
                                        GameManager.SpawningBehavior.HARSH_STANDARD,requiredPoints: 20, duration: 64,spawnbuffer:1f,background:Background1,zoom:true);
        // [TEST] Create NormalMode level
        Level NormalMode = new Level("NormalMode", "Normal style example", 0, GameManager.SectionStyle.NORMAL,
                                        GameManager.SpawningBehavior.STANDARD,requiredPoints: 20, spawnbuffer:1f,
                                        background:Background2,zoom:false);
        // [ENDLESS] Create the endless mode
        Level EndlessMode = new Level("Endless", "Endless mode", 101, GameManager.SectionStyle.ENDLESS, GameManager.SpawningBehavior.STANDARD, 0.5f,background:NoBackground);
        // Change next and back
        SurvivalMode.Next = NormalMode;
        SurvivalMode.Back = NormalMode;
        NormalMode.Next = SurvivalMode;
        // Add levels to levels list
        levels.Add(MainMenu);
        levels.Add(NormalMode);
        levels.Add(SurvivalMode);
        levels.Add(EndlessMode);
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
        }
        Debug.Log("No intro was displayed.. Why clear it");
        ExitIntro.SetActive(false);
    }
    public void ShowIntro()
    {
        if (SelectedLevel.Intro != null)
        {
            SelectedLevel.Intro.SetActive(true);
            ExitIntro.SetActive(true);
        }
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
    //public GameObject Boss;
    public bool Zoom;
    public Level(string name, string desc, int id, GameManager.SectionStyle style, GameManager.SpawningBehavior behavior,
        float spawnbuffer, Level back=null, Level next=null, int setPoints=-127, int requiredPoints=-127,int duration=-127,
        GameObject intro=null, GameObject background=null, bool zoom=false)
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
    }
}