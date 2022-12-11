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

    [SerializeField] AudioComponent audioSlowTimeStart;
    [SerializeField] AudioComponent audioSlowTimeStop;

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
    private void OnEnable()
    {
        startRun.OnLaunchEvent += EnableInput;
        stopRun.OnLaunchEvent += DisableInput;
    }

    private void OnDisable()
    {
        startRun.OnLaunchEvent -= EnableInput;
        stopRun.OnLaunchEvent -= DisableInput;
        DisableInput();
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
        audioSlowTimeStart.PlayAudioCue();
    }
    public void StopSlowTime()
    {
        Time.timeScale = 1f;
        isSlowTime = false;
        audioSlowTimeStop.PlayAudioCue();
    }
}
