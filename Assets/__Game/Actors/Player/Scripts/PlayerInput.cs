using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class PlayerInput : MonoBehaviour
{
    public bool FireButtonPressed { get; private set; }
    private bool cleanInput = false;

    // Update is called once per frame
    void Update()
    {
        CleanInput();
        ProcessInput();
    }

    private void FixedUpdate()
    {
        cleanInput = true;
    }

    private void CleanInput()
    {
        if (!cleanInput)
            return;
        FireButtonPressed = false;
    }

    private void ProcessInput() 
    {
        FireButtonPressed = FireButtonPressed || Input.GetButtonDown("Fire1");
    }
}
