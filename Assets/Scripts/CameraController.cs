using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 OriginalPosition;
    public GameObject Target;
    public Vector3 ZoomedOutPosition;
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
        for (float i = 0; i <= 1; i -= Time.deltaTime)
        {
            transform.position = Vector3.Lerp(OriginalPosition, ZoomedOutPosition, System.Math.Abs(i));
            yield return null;
        }
        Debug.Log("Done zooming out");
    }

    public IEnumerator SmoothZoomIn()
    {
        ZoomedOut = false;
        Debug.Log("Zooming in");
        for (float i = 0; i <= 1; i -= Time.deltaTime)
        {
            transform.position = Vector3.Lerp(ZoomedOutPosition, OriginalPosition, System.Math.Abs(i));
            yield return null;
        }
        Debug.Log("Done zooming in");
    }

    public void Zoom()
    {
        if (!ZoomedOut)
        {
            StopCoroutine(SmoothZoomIn());
            StartCoroutine(SmoothZoomOut());
        } else
        {
            StopCoroutine(SmoothZoomOut());
            StartCoroutine(SmoothZoomIn());
        }
    }

    public void ZoomOut()
    {
        StopCoroutine(SmoothZoomIn());
        StartCoroutine(SmoothZoomOut());
    }
    
    public void ZoomIn()
    {
        StopCoroutine(SmoothZoomOut());
        StartCoroutine(SmoothZoomIn());
    }
}
