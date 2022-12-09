using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    bool isSlowTime;
    public bool IsSlowTime()
        { return isSlowTime; }
    Input input;
    private void Awake()
    {
        input = new Input();
    }

    private void OnEnable()
    {
        input.Enable();
        input.InGame.SlowTime.performed += context => StartSlowTime();
        input.InGame.SlowTime.canceled += context => StopSlowTime();
    }
    private void OnDisable()
    {
        input.Disable();
        input.InGame.SlowTime.performed -= context => StartSlowTime();
        input.InGame.SlowTime.canceled -= context => StopSlowTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartSlowTime()
    {
        Time.timeScale = 0.5f;
        isSlowTime = true;
    }
    public void StopSlowTime()
    {
        Time.timeScale = 1f;
        isSlowTime = false;
    }
}
