using System.Collections;
using System.Collections.Generic;
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

    private void OnEnable()
    {
        input.OnSlowTimePressed += StartSlowTime;
        input.OnSlowTimeReleased += StopSlowTime;
    }

    private void OnDisable()
    {
        input.OnSlowTimePressed -= StartSlowTime;
        input.OnSlowTimeReleased -= StopSlowTime;
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
        Time.timeScale = 1;
    }
}
