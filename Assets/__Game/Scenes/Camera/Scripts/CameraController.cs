using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetToFollow;
    [Range(0, 1f)]
    public float sensibility = 0.8f;

    public float maxCameraSize = 100f;
    public float cameraChangeSpeed = 30f;
    

    private CameraIntialState cameraIntialState;
    private Vector3 relativeDistance;
    private Camera mCamera;
    private Vector3 initialPosition;


    void Awake()
    {
        initialPosition = transform.position;
        relativeDistance = transform.position - targetToFollow.transform.position;
        mCamera = Camera.main;
        cameraIntialState = GetComponent<CameraIntialState>();
    }


    private void LateUpdate()
    {
        float currentDistance = Vector3.Distance(initialPosition, transform.position);

        currentDistance += cameraIntialState.minOhographicSize;
        float size = Mathf.MoveTowards(mCamera.orthographicSize, currentDistance, cameraChangeSpeed*Time.deltaTime);
        
        size = Mathf.Clamp(size, cameraIntialState.minOhographicSize, maxCameraSize);
        
        mCamera.orthographicSize = size;
        
        Vector3 target = targetToFollow.transform.position + relativeDistance;
        transform.position = Vector3.MoveTowards(transform.position, target, sensibility);
    }
}
