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
    }

    public IEnumerator SmoothZoomOut()
    {
        ZoomedOut = true;
        Debug.Log("Zooming out");
        Vector3 startingPosition = transform.position;
        for (float i = 0; i <= 1; i -= Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startingPosition, ZoomedOutPosition, System.Math.Abs(i));
            yield return null;
        }
        Debug.Log("Done zooming out");
        transform.position = ZoomedOutPosition;
    }

    public IEnumerator SmoothZoomIn()
    {
        ZoomedOut = false;
        Debug.Log("Zooming in");
        Vector3 startingPosition = transform.position;
        for (float i = 0; i <= 1; i -= Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startingPosition, OriginalPosition, System.Math.Abs(i));
            yield return null;
        }
        Debug.Log("Done zooming in");
        transform.position = OriginalPosition;
    }

    public void TerminateZooms()
    {
        StopAllCoroutines();
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
