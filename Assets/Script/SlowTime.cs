using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SlowTime : MonoBehaviour
{
    bool isSlowTime;
    public bool IsSlowTime()
        { return isSlowTime; }
    [SerializeField] SOInputButton slowTimeInput;

    [SerializeField] EventSO startRun;
    [SerializeField] EventSO stopRun;

    private void EnableInput()
    {
        slowTimeInput.OnPressed += StartSlowTime;
        slowTimeInput.OnReleased += StopSlowTime;
    }
    private void DisableInput()
    {
        slowTimeInput.OnPressed -= StartSlowTime;
        slowTimeInput.OnReleased -= StopSlowTime;
    }
    private void Start()
    {
        startRun.OnLaunchEvent += EnableInput;
        stopRun.OnLaunchEvent += DisableInput;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(IsSlowTime());
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
