using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashHandler : MonoBehaviour
{
    public bool SplashShown = false;
    public bool SplashShowing = false;
    public bool SplashFading = false;
    public float SplashDuration;
    public GameObject Splash_Image;
    public LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Splash handler ready");
        if (!SplashShown)
        {
            ShowSplash(SplashDuration);
            SplashShown = true;
        }
    }

    private void OnDisable()
    {
        Debug.Log("Splash handler stopped");
    }

    public bool SplashCompleted()
    {
        return SplashShown;
    }

    public IEnumerator Fade()
    {
        SplashFading = true;
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            Splash_Image.GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    public void StartFading()
    {
        StartCoroutine(Fade());
    }

    public void ShowSplash(float duration)
    {
        SplashShowing = true;
        SplashFading = false;
        Debug.Log("Showing splash screen..");
        Color c = Splash_Image.GetComponent<Image>().color;
        c.a = 1;
        Splash_Image.GetComponent<Image>().color = c;
        Splash_Image.SetActive(true);
        Invoke(nameof(HideSplash), SplashDuration);
        Invoke(nameof(StartFading), SplashDuration-1);
    }

    public void HideSplash() { 
        Splash_Image.SetActive(false);
        Debug.Log("Splash complete"); 
        levelManager.SplashComplete();
    }
}
