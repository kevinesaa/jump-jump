using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class CameraIntialState : MonoBehaviour
{
    public float targetWidth;
    public float targetHeight;
    public int pixelsToUnits;

    public float minOhographicSize;

    private float targetRatio;
    private float currentRatio;

    // Use this for initialization
    void Start()
    {
        targetRatio = targetWidth / targetHeight;
        currentRatio = (float)Screen.width / (float)Screen.height;

        if (currentRatio >= targetRatio)
        {
            Camera.main.orthographicSize = targetHeight / 4 / pixelsToUnits;

        }
        else
        {

            float difference = targetRatio / currentRatio;
            Camera.main.orthographicSize = targetHeight / 4 / pixelsToUnits * difference;
        }

        minOhographicSize = Camera.main.orthographicSize;
    }
}
