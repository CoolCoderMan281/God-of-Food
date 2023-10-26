using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControle : MonoBehaviour
{
    public Vector3 OriginalPosition;
    public GameObject Target;
    public Vector3 ZoomedOutPosition;
    public bool ZoomedOut;
    void Start()
    {
        OriginalPosition = transform.position;
        ZoomedOutPosition = Target.transform.position;
        Invoke(nameof(Zoom), 10f);
        Invoke(nameof(Zoom), 20f);
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
    }

    public void Zoom()
    {
        if (!ZoomedOut)
        {
            StartCoroutine(SmoothZoomOut());
        } else
        {
            StartCoroutine(SmoothZoomIn());
        }
    }

}
