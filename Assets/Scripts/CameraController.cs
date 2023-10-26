using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 OriginalPosition;
    public GameObject Target;
    public Vector3 ZoomedOutPosition;
    public Coroutine Coroutine;
    public bool ZoomedOut;
    public bool Zooming;
    void Start()
    {
        OriginalPosition = transform.position;
        ZoomedOutPosition = Target.transform.position;
        //Invoke(nameof(ZoomOut), 5f);
        //Invoke(nameof(ZoomOut), 8f);
        //Invoke(nameof(ZoomIn), 11f);
        //Invoke(nameof(ZoomOut), 14f);
    }

    public IEnumerator SmoothZoomOut()
    {
        ZoomedOut = true;
        Debug.Log("Zooming out");
        for (float i = 0; i <= 1; i -= Time.deltaTime)
        {
            Debug.Log(i);
            transform.position = Vector3.Lerp(OriginalPosition, ZoomedOutPosition, System.Math.Abs(i));
            yield return null;
        }
        Debug.Log("Done zooming out");
        yield break;
    }

    public IEnumerator SmoothZoomIn()
    {
        ZoomedOut = false;
        Debug.Log("Zooming in");
        for (float i = 0; i <= 1; i -= Time.deltaTime)
        {
            Debug.Log(i);
            transform.position = Vector3.Lerp(ZoomedOutPosition, OriginalPosition, System.Math.Abs(i));
            yield return null;
        }
        Debug.Log("Done zooming in");
        yield break;
    }

    public void TerminateZooms()
    {
        StopCoroutine(Coroutine);
    }

    public void Zoom()
    {
        if (!ZoomedOut)
        {
            Coroutine = StartCoroutine(SmoothZoomOut());
            Invoke(nameof(TerminateZooms), 1f);
        } else
        {
            Coroutine = StartCoroutine(SmoothZoomIn());
            Invoke(nameof(TerminateZooms), 1f);
        }
    }

    public void ZoomOut()
    {
        if (!ZoomedOut)
        {
            Coroutine = StartCoroutine(SmoothZoomOut());
            Invoke(nameof(TerminateZooms), 1f);
        }
    }
    
    public void ZoomIn()
    {
        if (ZoomedOut)
        {
            Coroutine = StartCoroutine(SmoothZoomIn());
            Invoke(nameof(TerminateZooms), 1f);
        }
    }
}
