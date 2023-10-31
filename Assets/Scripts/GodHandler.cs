using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodHandler : MonoBehaviour
{
    [Header("Healthy one")]
    public GameObject Idle1;
    public GameObject Idle2;
    public GameObject Switch1;
    public GameObject Switch2;
    [Header("Internal")]
    public GameManager manager;
    public CameraController camController;
    public GameObject lastShown;
    // OH MY ITS CRUNCH TIME :OOOOO
    void Start()
    {
        Idle1.SetActive(false);
        Idle2.SetActive(false);
        Switch1.SetActive(false);
        Switch2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // I'm an always nester jk i just dont have any time to optimize gotta crank out this stuff
        if (manager.Level != null)
        {
            if (manager.Level.LevelRelation == 1)
            {
                if (lastShown == null)
                {
                    lastShown = Idle1;
                }
                lastShown.SetActive(false);
                if (camController.Zooming && !manager.Intro)
                {
                    if (lastShown == Idle1)
                    {
                        lastShown = Switch2;
                    }
                    else if (lastShown == Idle2)
                    {
                        lastShown = Switch1;
                    }
                    lastShown.SetActive(true);
                }
                else
                {
                    if (manager.Level.Style == GameManager.SectionStyle.NORMAL)
                    {
                        lastShown = Idle1;
                    }
                    else
                    {
                        lastShown = Idle2;
                    }
                    lastShown.SetActive(true);
                }
            }
        }
    }
}
