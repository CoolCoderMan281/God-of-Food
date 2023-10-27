using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameInProgress = false;
    public bool CanSpawn = false;
    private bool AllowSpawns = false;
    public List<GameObject> falling_options;
    public List<GameObject> spawned;
    public List<GameObject> Indicators;
    private TMP_Text phase_text;
    private TMP_Text score_text;
    private TMP_Text timer_text;
    private GameObject MainMenu_canvas;
    public int Points = 0;
    public int RequiredPoints;
    public Level Level;
    public int Section;
    public int SpawningIncrement;
    public int SpawningRangeStart;
    public int SpawningRangeEnd;
    public SectionStyle Style;
    public SpawningBehavior SBehavior;
    public int Duration;
    private bool timer_isCounting;
    private LevelManager levelManager;
    public SplashHandler SplashHandler;
    public bool paused = false;
    public bool MainMenu = false;
    public bool Intro = false;
    public GameObject Indicator;
    public CameraController camController;
    public float spawnHight;
    public float spawnXRange;
    [Header("Keybinds")]
    public KeyCode pauseKey = KeyCode.Escape;
    [Header("Developer")]
    public bool AllowCheats;
    public KeyCode ConsoleKey = KeyCode.BackQuote;
    private bool ConsoleOpen;
    private string ConsoleInput;
    public List<String> SessionLogs = new List<String>();
    public GameObject Logs;
    public TMP_Text Logs_Text;
    // Start is called before the first frame update
    void Start()
    {
        Application.logMessageReceived += HandleLog;
        Debug.Log("Application is starting..");
        phase_text = GameObject.Find("Phase").GetComponent<TMP_Text>();
        score_text = GameObject.Find("Score").GetComponent<TMP_Text>();
        timer_text = GameObject.Find("Timer").GetComponent<TMP_Text>();
        MainMenu_canvas = GameObject.Find("MainMenu_Content");
        levelManager = gameObject.GetComponent<LevelManager>();
        phase_text.text = "";
        timer_text.text = "";
        spawnHight = 7;
        spawnXRange = 8;
        GameInProgress = false;
        CanSpawn = false;
        AllowSpawns = false;
        SpawningIncrement = 0;
        ConsoleOpen = false;
        camController = gameObject.GetComponent<CameraController>();
        if (AllowCheats)
        {
            Logs.SetActive(false);
            Logs_Text.text = "";
            Debug.Log("Console Key: " + ConsoleKey.ToString());
            Debug.Log("AllowCheats: " + AllowCheats.ToString());
        }
        MainMenu = true;
        Debug.Log("GameManager ready!");
    }

    public enum SectionStyle
    {
        NORMAL, SURVIVE, ENDLESS, ACHIEVE, IDLE
    }

    public enum SpawningBehavior
    {
        STANDARD, CLUSTER, BLANKET
    }

    private void OnGUI()
    {
        if (!ConsoleOpen) { return; }
        Logs.SetActive(ConsoleOpen);
        float y = 5f;
        GUIStyle textStyle = new GUIStyle(GUI.skin.box);
        textStyle.alignment = TextAnchor.MiddleLeft;
        textStyle.fontSize = 32;
        GUI.SetNextControlName("ConsoleInput");
        ConsoleInput = GUI.TextField(new Rect(0, y, Screen.width, Screen.height / 15), ConsoleInput, textStyle);
        Input.eatKeyPressOnTextFieldFocus = false;
        GUI.FocusControl("ConsoleInput");
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return) && ConsoleInput.Length >= 1)
        {
            try
            {
                CallCommand(ConsoleInput);
            }
            catch
            {
                Debug.LogError("Failed to execute command " + ConsoleInput);
            }
            ConsoleInput = "";
        }
    }

    public void CallCommand(string command)
    {
        command = command.ToLower();
        Debug.Log("Console Info: > " + command);
        if (command.StartsWith("level"))
        {
            string[] args = command.Replace("level", "").Split(" ");
            // Example: level name set X
            // Example: level name get
            if (args.Length == 4) // Set
            {
                if (args[1] == "name" && args[2] == "set")
                {
                    Level.Name = args[3].ToString();
                    Debug.Log("Operation completed successfully.");
                }
                if (args[1] == "desc" && args[2] == "set")
                {
                    Level.Description = args[3].ToString();
                    Debug.Log("Operation completed successfully.");
                }
                if (args[1] == "id" && args[2] == "set")
                {
                    Level.ID = int.Parse(args[3]);
                    Debug.Log("Operation completed successfully.");
                }
                if (args[1] == "style" && args[2] == "set")
                {
                    if (args[3] == "normal")
                    {
                        Level.Style = SectionStyle.NORMAL;
                    } else if (args[3] == "survive")
                    {
                        Level.Style = SectionStyle.SURVIVE;
                    } else if (args[3] == "achieve")
                    {
                        Level.Style = SectionStyle.ACHIEVE;
                    } else if (args[3] == "endless")
                    {
                        Level.Style = SectionStyle.ENDLESS;
                    } else if (args[3] == "idle")
                    {
                        Level.Style = SectionStyle.IDLE;
                    } else
                    {
                        Debug.LogWarning("Unknown style");
                    }
                    Debug.Log("Operation completed successfully.");
                }
                if (args[1] == "behavior" && args[2] == "set")
                {
                    if (args[3] == "standard")
                    {
                        Level.Behavior = SpawningBehavior.STANDARD;
                    }
                    else if (args[3] == "cluster")
                    {
                        Level.Behavior = SpawningBehavior.CLUSTER;
                    }
                    else if (args[3] == "blanket")
                    {
                        Level.Behavior = SpawningBehavior.BLANKET;
                    }
                    else
                    {
                        Debug.LogWarning("Unknown behavior");
                    }
                    Debug.Log("Operation completed successfully.");
                }
                if (args[1] == "back" && args[2] == "set")
                {
                    Debug.LogWarning("Not implemented!");
                }
                if (args[1] == "next" && args[2] == "set")
                {
                    Debug.LogWarning("Not implemented!");
                }
                if (args[1] == "setpoints" && args[2] == "set")
                {
                    Level.SetPoints = int.Parse(args[3]);
                    Debug.Log("Operation completed successfully.");
                }
                if (args[1] == "requiredpoints" && args[2] == "set")
                {
                    Level.RequiredPoints = int.Parse(args[3]);
                    Debug.Log("Operation completed successfully.");
                }
                if (args[1] == "duration" && args[2] == "set")
                {
                    Level.Duration = int.Parse(args[3]);
                    Debug.Log("Operation completed successfully.");
                }
                if (args[1] == "buffer" && args[2] == "set")
                {
                    Level.SpawnBuffer = float.Parse(args[3]);
                    Debug.Log("Operation completed successfully.");
                }
                Levels_Start(Level);
            }
            else if (args.Length == 3) // Get
            {
                if (args[1] == "name")
                {
                    if (args[2] == "get")
                    {
                        Debug.Log("Name: " + Level.Name);
                    }
                }
                if (args[1] == "desc")
                {
                    if (args[2] == "get")
                    {
                        Debug.Log("Description: " + Level.Description);
                    }
                }
                if (args[1] == "id" && args[2] == "get")
                {
                    Debug.Log("ID: " + Level.ID.ToString());
                }
                if (args[1] == "style" && args[2] == "get")
                {
                    Debug.Log("Style: " + Level.Style.ToString());
                }
                if (args[1] == "behavior" && args[2] == "get")
                {
                    Debug.Log("Behavior: " + Level.Behavior.ToString());
                }
                if (args[1] == "back" && args[2] == "get")
                {
                    Debug.Log("Back: " + Level.Back.Name.ToString());
                }
                if (args[1] == "next" && args[2] == "get")
                {
                    Debug.Log("Next: " + Level.Next.Name.ToString());
                }
                if (args[1] == "setpoints" && args[2] == "get")
                {
                    Debug.Log("SetPoints: " + Level.SetPoints);
                }
                if (args[1] == "requiredpoints" && args[2] == "get")
                {
                    Debug.Log("RequiredPoints: " + Level.RequiredPoints.ToString());
                }
                if (args[1] == "duration" && args[2] == "get")
                {
                    Debug.Log("Duration: " + Level.Duration.ToString());
                }
                if (args[1] == "buffer" && args[2] == "get")
                {
                    Debug.Log("Buffer: " + Level.SpawnBuffer.ToString());
                }
            }
            else if (args.Length == 2)
            {
                if (args[1] == "reload")
                {
                    Levels_Start(Level);
                }
            }
            else
            {
                Debug.Log("Console Info: Insufficient arguments");
            }
        }
        else if (command.StartsWith("object"))
        {
            CallCommand("clear");
            Debug.Log("Objects cleared to only be referencing the examples..");
            string[] args = command.Replace("object", "").Split(" ");
            GameObject TheObj;
            TheObj = GameObject.Find("OBJECT_"+args[1]);
            if (TheObj == null)
            {
                Debug.Log("Unknown object OBJECT_" + args[1]);
                return;
            }
            Debug.Log("Found object!");
            if (args[2] == "fallspeed" && args[3] == "set")
            {
                TheObj.GetComponent<FallingObject>().FallSpeedMultiplyer = float.Parse(args[4]);
                Debug.Log("Operation completed successfuly");
            }
            else if (args[2] == "fallspeed" && args[3] == "get")
            {
                Debug.Log("Fallspeed set to: " + TheObj.GetComponent<FallingObject>().FallSpeedMultiplyer);
            }
            else if (args[2] == "maxspeed" && args[3] == "set")
            {
                TheObj.GetComponent<FallingObject>().MaxSpeed = float.Parse(args[4]);
                Debug.Log("Operation completed successfuly");
            }
            else if (args[2] == "maxspeed" && args[3] == "get")
            {
                Debug.Log("Maxspeed set to: " + TheObj.GetComponent<FallingObject>().MaxSpeed);
            }
            else if (args[2] == "worth" && args[3] == "set")
            {
                TheObj.GetComponent<FallingObject>().Worth = int.Parse(args[4]);
                Debug.Log("Operation completed successfuly");
            }
            else if (args[2] == "worth" && args[3] == "get")
            {
                Debug.Log("Worth set to: " + TheObj.GetComponent<FallingObject>().Worth);
            }
            else if (args[2] == "punishment" && args[3] == "set")
            {
                TheObj.GetComponent<FallingObject>().Punishment = int.Parse(args[4]);
                Debug.Log("Operation completed successfuly");
            }
            else if (args[2] == "punishment" && args[3] == "get")
            {
                Debug.Log("Punishment set to: " + TheObj.GetComponent<FallingObject>().Punishment);
            } else
            {
                Debug.Log("Unknown argument");
            }
        }
        else if (command == "splash")
        {
            try
            {
                Style = SectionStyle.IDLE;
                SplashHandler.ShowSplash(SplashHandler.SplashDuration);
            } catch
            {
                Debug.LogError("Failed to force splash screen");
            }
        }
        else if (command == "mainmenu")
        {
            MainMenu = true;
            paused = true;
            ExitConsole(true);
        }
        else if (command.StartsWith("setpoints"))
        {
            string[] args = command.Replace("setpoints", "").Split(" "); ;
            Points = int.Parse(args[1]);
            score_text.text = Points.ToString();
            Debug.Log("Set points to "+Points);
        }
        else if (command == "clear")
        {
            int cleared = 0;
            for (var i = 0; i < spawned.Count(); i++)
            {
                Destroy(spawned[i].gameObject);
                cleared++;
            }
            spawned = new List<GameObject>();
            Debug.Log("Cleared " + cleared + " active objects");
        } else if (command == "quit")
        {
            Application.Quit();
        } else if (command == "pause")
        {
            paused = true;
            for (var i = 0; i < spawned.Count(); i++)
            {
                if (spawned[i] != null)
                {
                    spawned[i].SetActive(false);
                }
            }
        } else if (command == "resume")
        {
            paused = false;
            for (var i = 0; i < spawned.Count(); i++)
            {
                if (spawned[i] != null)
                {
                    spawned[i].SetActive(true);
                }
            }
        } else if (command == "exit")
        {
            paused = false;
            ExitConsole();
        } else
        {
            Debug.Log("Unknown command: " + command);
        }
    }
    public void ExitConsole(bool StayPaused=false)
    {
        ConsoleOpen = false;
        Logs.SetActive(false);
        ConsoleInput = "";
        paused = StayPaused;
    }

    // Update is called once per frame
    void Update()
    {
        if (MainMenu)
        {
            paused = true;
            MainMenu_canvas.SetActive(true);
        } else
        {
            MainMenu_canvas.SetActive(false);
        }
        if (Input.GetKeyDown(pauseKey))
        {
            paused = !paused;
            if (paused)
            {
                CallCommand("pause");
            } else
            {
                CallCommand("resume");
            }
            Debug.Log("Paused " + paused);
        }

        // Handle debug input
        if (Input.GetKeyDown(ConsoleKey) && AllowCheats)
        {
            ConsoleOpen = !ConsoleOpen;
            // Get all UI elements with UI_RuntimeEditor tag
            ConsoleInput = "";
            Logs.SetActive(ConsoleOpen);
            if (ConsoleOpen == false)
            {
                CallCommand("resume");
            } else
            {
                CallCommand("pause");
            }
        }

        // Spawn Handler
        if (GameInProgress && !paused) // Don't waste resources, don't process if no game
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            newPosition.y = -2; newPosition.z = 0; newPosition.x += 5;
            Indicator.transform.position = newPosition;
            if (CanSpawn && AllowSpawns) // Don't waste resources, don't process if spawn unavaliable
            {
                try 
                {
                    // Standard behavior
                    if (SBehavior == SpawningBehavior.STANDARD)
                    {
                        CanSpawn = false;
                        if (SpawningIncrement == 5)
                        {
                            SpawningIncrement = 0;
                            CancelInvoke(nameof(AllowSpawning));
                            Invoke(nameof(AllowSpawning), UnityEngine.Random.Range(0.5f, 2.5f));
                        } else
                        {
                            int randomObj = UnityEngine.Random.Range(0, falling_options.Count());
                            GameObject newObj = Instantiate(falling_options[UnityEngine.Random.Range(0, falling_options.Count())]);
                            spawned.Add(newObj);
                            Vector3 targetPos = new Vector3(UnityEngine.Random.Range(-spawnXRange, spawnXRange), spawnHight, 0);
                            newObj.transform.position = targetPos;
                            newObj.GetComponent<FallingObject>().isExample = false;
                            newObj.GetComponent<FallingObject>().SpawnBuffer = Level.SpawnBuffer;
                            Invoke(nameof(AllowSpawning), UnityEngine.Random.Range(0.25f, 0.50f));
                            SpawningIncrement++;
                        }
                    }
                    // Cluster behavior
                    if (SBehavior == SpawningBehavior.CLUSTER)
                    {
                        CanSpawn = false;
                        if (SpawningIncrement == 5)
                        {
                            SpawningIncrement = 0;
                            CancelInvoke(nameof(AllowSpawning));
                            Invoke(nameof(AllowSpawning), UnityEngine.Random.Range(1.50f, 2.50f));
                            return;
                        }
                        if (SpawningIncrement == 2)
                        {
                            if (SpawningRangeEnd <= 0)
                            {
                                SpawningRangeStart = UnityEngine.Random.Range((int)-spawnXRange, (int)spawnXRange);
                                SpawningRangeEnd = SpawningRangeStart + 2;
                            } else
                            {
                                SpawningRangeStart = UnityEngine.Random.Range((int)-spawnXRange, (int)spawnXRange);
                                SpawningRangeEnd = SpawningRangeStart - 2;
                            }
                            CancelInvoke(nameof(AllowSpawning));
                            Invoke(nameof(AllowSpawning),0f);
                        }
                        if (SpawningIncrement == 0)
                        {
                            SpawningRangeStart = UnityEngine.Random.Range(-8, 6);
                            SpawningRangeEnd = SpawningRangeStart + 2;
                            CancelInvoke(nameof(AllowSpawning));
                        }
                        if (SpawningIncrement <= 5)
                        {
                            int randomObj = UnityEngine.Random.Range(0, falling_options.Count());
                            GameObject newObj = Instantiate(falling_options[UnityEngine.Random.Range(0, falling_options.Count())]);
                            spawned.Add(newObj);
                            Vector3 targetPos = new Vector3(UnityEngine.Random.Range(SpawningRangeStart, SpawningRangeEnd), spawnHight, 0);
                            newObj.transform.position = targetPos;
                            newObj.GetComponent<FallingObject>().isExample = false;
                            Invoke(nameof(AllowSpawning), UnityEngine.Random.Range(0.025f, 0.05f));
                            SpawningIncrement++;
                        }
                    }
                    // Blanket behavior
                    if (SBehavior == SpawningBehavior.BLANKET)
                    {
                        CanSpawn = false;
                        for(var i = 0; i < 5; i++)
                        {
                            int randomObj = UnityEngine.Random.Range(0, falling_options.Count());
                            GameObject newObj = Instantiate(falling_options[UnityEngine.Random.Range(0, falling_options.Count())]);
                            spawned.Add(newObj);
                            Vector3 targetPos = new Vector3(UnityEngine.Random.Range(-spawnXRange, spawnXRange), spawnHight, 0);
                            newObj.transform.position = targetPos;
                            newObj.GetComponent<FallingObject>().isExample = false;
                        }
                        Invoke(nameof(AllowSpawning), UnityEngine.Random.Range(1.25f, 2.50f));
                    }
                }
                catch
                {
                    Debug.Log("Spawn failed");
                }
            }
        }
    }
    public bool SectionUpdate(int new_section)
    {
        ResetScoreColor();
        Duration = Level.Duration;
        timer_isCounting = false;
        Section = new_section;
        try
        {
            SectionChangeApply(Section);
            return true;
        } catch
        {
            Debug.Log("Failed to apply section change modifiers");
            return false;
        }
    }
    public void SectionChangeApply(int theSection)
    {
        Debug.Log("Ignored destroying falling objects (Designer request) :/");
        //for(var i = 0; i < spawned.Count(); i++)
        //{
        //    Destroy(spawned[i].gameObject);
        //}
        // Get objects that may require changes
        Section = theSection;
        // Apply changes
        if (Level.Style == SectionStyle.IDLE)
        {
            GameInProgress=false;
            AllowSpawns=false;
            CanSpawn = false;
        }

        if (Style == SectionStyle.SURVIVE || Style == SectionStyle.ACHIEVE)
        {
            SetTimer();
        }
    }
    public void TimerEnd()
    {
        timer_text.text = "";
        Debug.Log("Timer complete");

        // Survive Style
        if (Style == SectionStyle.SURVIVE)
        {
            if (Points >= RequiredPoints)
            {
                // PASS
                Levels_PassedLevel();
            } else
            {
                // FAIL
                Levels_FailedLevel();
            }
        }
        // ACHIEVE Style
        if (Style == SectionStyle.ACHIEVE)
        {
            if (Points >= RequiredPoints)
            {
                // PASS
                Levels_PassedLevel();
            } else
            {
                // FAIL
                Levels_FailedLevel();
            }
        }
    }
    public void SetTimer()
    {
        if (!timer_isCounting)
        {
            timer_isCounting = true;
            Invoke(nameof(Countdown),0);
            Debug.Log("Timer started");
        }
    }
    private void Countdown()
    {
        timer_text.text = Duration.ToString();
        if (!paused)
        {
            Duration--;
        }
        if (Duration > 0)
        {
            Invoke(nameof(Countdown), 1);
        } else
        {
            timer_isCounting = false;
            TimerEnd();
        }
    }
    public void AllowSpawning() { if (AllowSpawns) { CanSpawn = true; } }
    public void ResetPhaseText() { phase_text.text = ""; }
    public void SetScoreColorRed() { Color red = Color.red; red.a = 0.5f; score_text.color = red; }
    public void SetScoreColorGreen() { Color green = Color.green; green.a = 0.5f; score_text.color = green; }
    public void ResetScoreColor() { Color white = Color.white; white.a = 0.5f; score_text.color = white; }
    public void UpdateScore(int worth, GameObject cause=null)
    {
        // Add the points
        Points += worth;

        // Normal Style
        if (Style == SectionStyle.NORMAL)
        {
            if (worth < 0) { SetScoreColorRed(); Invoke(nameof(ResetScoreColor), 0.25f); } // Flash red when reducing points
            if (Points < 0) { Points = 0; } // Prevent negative score since we are nice :)
            if (Points >= RequiredPoints)
            {
                Levels_PassedLevel();
            }
        }
        // Survive Style
        if (Style == SectionStyle.SURVIVE)
        {
            if (Points < RequiredPoints) // FAIL
            {
                Levels_FailedLevel();
            } else if (Points > RequiredPoints)
            {
                SetScoreColorGreen();
            } else
            {
                ResetScoreColor();
            }
        }
        // Endless Style
        if (Style == SectionStyle.ENDLESS)
        {
            if (worth < 0) { SetScoreColorRed(); Invoke(nameof(ResetScoreColor), 0.25f); } // Flash red when reducing points
            if (Points < 0) { Points = 0; } // Prevent negative score since we are nice :)
            Debug.LogWarning("Endless currently has no way to escape to menu!");
        }
        // ACHIEVE Style
        if (Style == SectionStyle.ACHIEVE)
        {
            if (worth < 0) { SetScoreColorRed(); Invoke(nameof(ResetScoreColor), 0.25f); } // Flash red when reducing points
            if (Points < 0) { Points = 0; } // Prevent negative score since we are nice :)
            if (Points < RequiredPoints)
            {
                SetScoreColorRed();
            }
            else
            {
                SetScoreColorGreen();
            }
        }
        if (cause != null)
        {
            if (worth >= 0)
            {
                CancelInvoke(nameof(KillIndicator));
                StartCoroutine(FadeIn_Text(Indicator, 0.1f));
                TMP_Text indicator_text = Indicator.GetComponent<TMP_Text>();
                if (indicator_text.text == "")
                {
                    indicator_text.text = "0";
                }
                int new_score = int.Parse(indicator_text.text);
                new_score += worth;
                if (new_score >= 0)
                {
                    indicator_text.color = Color.green;
                    indicator_text.text = "+" + new_score;
                }
                else
                {
                    indicator_text.color = Color.red;
                    indicator_text.text = "" + new_score;
                }
                StartCoroutine(FadeOut_Text(Indicator, 0.75f,false));
                Invoke(nameof(KillIndicator), 1f);
            } else
            {
                GameObject tmpIndicator = Instantiate(Indicator);
                Indicators.Add(tmpIndicator);
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                newPosition.x = cause.transform.position.x + 5f;
                newPosition.y = -2;
                newPosition.z = 0;
                tmpIndicator.transform.position = newPosition;
                tmpIndicator.GetComponent<TMP_Text>().text = "" + worth;
                tmpIndicator.GetComponent<TMP_Text>().color = Color.red;
                tmpIndicator.SetActive(true);
                StartCoroutine(FadeIn_Text(tmpIndicator, 0.25f));
                Invoke(nameof(DestroyIndicator), .5f);
            }
        }
        // Update the points display
        score_text.text = Points.ToString();
    }

    public void KillIndicator()
    {
        TMP_Text indicator_text = Indicator.GetComponent<TMP_Text>();
        indicator_text.text = "";
    }
    public void DestroyIndicator()
    {
        StartCoroutine(FadeOut_Text(Indicators[0],0.25f));
        Indicators.RemoveAt(0);
    }

    public IEnumerator FadeIn_Text(GameObject target, float time)
    {
        TMP_Text i = target.GetComponent<TMP_Text>();
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / time));
            yield return null;
        }
    }

    public IEnumerator FadeOut_Text(GameObject target, float time, bool destroy=true)
    {
        TMP_Text i = target.GetComponent<TMP_Text>();
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / time));
            yield return null;
        }
        if (destroy)
        {
            Destroy(target);
        }
    }

    public void Levels_Start(Level lvl) // Called by LevelManager
    {
        Debug.Log("Level load request recived for level id: " + lvl.ID);
        CancelInvoke(nameof(AllowSpawning));
        timer_text.text = "";
        Level = lvl;
        SBehavior = Level.Behavior;
        Style = Level.Style;
        camController.TerminateZooms();
        if (Style != SectionStyle.IDLE)
        {
            CanSpawn = true;
        }
        // Update variables to reflect level parameters
        if (Level.SetPoints != -127)
        {
            Points = Level.SetPoints;
        }
        if (Level.RequiredPoints != -127)
        {
            RequiredPoints = Level.RequiredPoints;
        }
        if (Level.Duration != -127)
        {
            Duration = Level.Duration;
        }
        if (Level.Intro != null)
        {
            Intro = true;
            GameInProgress = false;
            levelManager.ShowIntro();
        }
        else
        {
            GameInProgress = true;
            CanSpawn = true;
            AllowSpawns = true;
        }
        if (Level.Zoom)
        {
            camController.ZoomOut();
            spawnHight = 18;
            spawnXRange = 16;
        } else
        {
            camController.ZoomIn();
            spawnHight = 7;
            spawnXRange = 8;
        }
        
        score_text.text = Points.ToString();
        Debug.Log("Set level attributes\nStyle: "+Level.Style+"\nSBehavior: "+Level.Behavior+"\nSetPoints: "+Level.SetPoints+"\nRequiredPoints: "+
                    Level.RequiredPoints+"\nDuration: "+Level.Duration.ToString()+"\nLevel ID: "+Level.ID.ToString()+"\nLevel Name: "+Level.Name);
        SectionUpdate(lvl.ID);
    }
    public void Levels_FailedLevel()
    {
        Debug.Log("Failed level #" + Level.ID);
        if (Level.Back != null)
        {
            levelManager.EndLevel(Level.Back);
        } else
        {
            levelManager.EndLevel();
        }
    }
    public void Levels_PassedLevel()
    {
        Debug.Log("Won level #" + Level.ID);
        if (Level.Next != null)
        {
            levelManager.EndLevel(Level.Next);
        } else
        {
            levelManager.EndLevel();
        }
    }

    public void Levels_IntroOver()
    {
        Debug.Log("Exiting intro");
        Intro = false;
        levelManager.EndIntro();
        GameInProgress = true;
        paused = false;
        AllowSpawns = true;
        CanSpawn = true;
    }

    public void OnDisable()
    {
        Debug.Log("GameManager stopped!");
        Debug.Log("Application is closing..");
        Application.Quit();
    }

    public void UpdateConsole(string log)
    {
        Logs_Text.text = Logs_Text.text + "\n" + log;
    }

    public void StartEndless()
    {
        Debug.Log("Starting endless mode..");
        levelManager.RequestLevelSwitch(101);
    }
    
    public void StartStory()
    {
        Debug.Log("Starting story mode...");
        levelManager.RequestLevelSwitch(0);
    }

    public void HandleLog(string logString, string stacktrace, LogType type)
    {
        /*
         * This log handler will save me a lot of trouble if someone has an issue
         * with the game, I can have them send me this file and it'll tell
         * me what went wrong and I can figure it out
         */
        TextWriter tw = new StreamWriter((Application.dataPath+"/log.txt"), true);
        String fancy_log = ("[" + (DateTime.Now.Date).ToString().Substring(0, 10) + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" +
                    DateTime.Now.Second + "] ") + logString;
        tw.WriteLine(fancy_log);
        tw.Close();
        UpdateConsole(fancy_log);
    }
}

