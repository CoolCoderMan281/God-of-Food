using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public Vector3 OriginalPosition;
    public GameObject Target;
    public Vector3 ZoomedOutPosition;
    public Coroutine Coroutine;
    private Vector3 startingPosition;
    public Vector3 TargetPosition;
    public GameManager manager;
    public bool ZoomedOut;
    public bool Zooming;
    void Start()
    {
        OriginalPosition = transform.position;
        ZoomedOutPosition = Target.transform.position;
        if (TargetPosition == null)
        {
            TargetPosition = OriginalPosition;
        }
        StartCoroutine(ReachTarget());
    }

    public IEnumerator SmoothZoomOut()
    {
        ZoomedOut = true;
        Debug.Log("Zooming out");
        for (float i = 0; i <= 1; i -= Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startingPosition, ZoomedOutPosition, System.Math.Abs(i));
            yield return null;
        }
        Debug.Log("Done zooming out");
        //transform.position = ZoomedOutPosition;
    }

    public IEnumerator SmoothZoomIn()
    {
        ZoomedOut = false;
        Debug.Log("Zooming in");
        for (float i = 0; i <= 1; i -= Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startingPosition, OriginalPosition, System.Math.Abs(i));
            yield return null;
        }
        Debug.Log("Done zooming in");
        //transform.position = OriginalPosition;
    }

    public IEnumerator ReachTarget()
    {
        Debug.Log("ReachTarget coroutine started");
        while (true)
        {
            Vector3 startingPosition = transform.position;
            if (startingPosition != TargetPosition)
            {
                Debug.Log(startingPosition + " | " + TargetPosition);
                for (float i = 0; i >= -1; i -= Time.deltaTime)
                {
                    Debug.Log(i);
                    transform.position = Vector3.Lerp(startingPosition, TargetPosition, System.Math.Abs(i));
                    yield return null;
                }
                if (transform.position != TargetPosition)
                {
                    Debug.LogWarning("Did not actually reach target! Harsh correction");
                    transform.position = TargetPosition;
                    manager.Zoom_Complete();
                }
                else
                {
                    Debug.Log("Reached target!");
                    manager.Zoom_Complete();
                }
            } else
            {
                yield return new WaitForSeconds(.5f);
            }
        }
        Debug.LogWarning("This coroutine shouldn't have ended!");
    }

    public void TerminateZooms()
    {
        Debug.LogWarning("Disabled method!");
        return;
        StopAllCoroutines();
    }

    public void Zoom()
    {
        Debug.LogWarning("Disabled method!");
        return;
        if (!ZoomedOut)
        {
            ZoomOut();
        } else
        {
            ZoomIn();
        }
    }

    public void ZoomOut()
    {
        Debug.LogWarning("Disabled method!");
        return;
        if (!ZoomedOut)
        {
            startingPosition = transform.position;
            Coroutine = StartCoroutine(SmoothZoomOut());
            Invoke(nameof(TerminateZooms), 1f);
        }
    }
    
    public void ZoomIn()
    {
        Debug.LogWarning("Disabled method!");
        return;
        if (ZoomedOut)
        {
            startingPosition = transform.position;
            Coroutine = StartCoroutine(SmoothZoomIn());
            Invoke(nameof(TerminateZooms), 1f);
        }
    }

    public void ForceLocation(Vector3 newPosition)
    {
        Debug.LogWarning("Disabled method!");
        return;
        transform.position = newPosition;
    }
}
