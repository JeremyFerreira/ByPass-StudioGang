using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SlowTime : MonoBehaviour
{
    bool isSlowTime;
    public bool IsSlowTime()
    { return isSlowTime; }

    [SerializeField] InputSO input;

    [SerializeField] AudioComponent audioSlowTimeStart;
    [SerializeField] AudioComponent audioSlowTimeStop;
    [SerializeField] GlitchEffect glitchEffect;
    [SerializeField] ValueSo slowTimeSliderData;
    [SerializeField] EventSO slowTimeStartEvent;
    [SerializeField] EventSO slowTimeStopEvent;

    [SerializeField] EventSO pauseEvent;
    [SerializeField] EventSO resumeEvent;
    bool pause;

    private void OnEnable()
    {
        input.OnSlowTimePressed += StartSlowTime;
        input.OnSlowTimeReleased += StopSlowTime;

        pauseEvent.OnLaunchEvent += Pause;
        resumeEvent.OnLaunchEvent += Resume;

        pause = false;
    }

    private void OnDisable()
    {
        input.OnSlowTimePressed -= StartSlowTime;
        input.OnSlowTimeReleased -= StopSlowTime;

        pauseEvent.OnLaunchEvent -= Pause;
        resumeEvent.OnLaunchEvent -= Resume;
    }

    public void StartSlowTime()
    {
        isSlowTime = true;
        audioSlowTimeStart.PlayAudioCue();
        slowTimeStartEvent.OnLaunchEvent.Invoke();
        glitchEffect.enabled = true;
        Time.timeScale = 0.5f;
    }

    public void StopSlowTime()
    {
        isSlowTime = false;
        audioSlowTimeStop.PlayAudioCue();
        slowTimeStopEvent.OnLaunchEvent.Invoke();
        glitchEffect.enabled = false;
        if(!pause)
        {
            Time.timeScale = 1;
        }
        
    }
    void Pause()
    {
        pause = true;
    }
    void Resume()
    {
        pause = false;
    }
}
