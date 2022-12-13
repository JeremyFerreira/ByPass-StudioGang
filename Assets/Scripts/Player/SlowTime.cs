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

    Input input;
    [SerializeField] EventSO pauseEvent;

    [SerializeField] AudioComponent audioSlowTimeStart;
    [SerializeField] AudioComponent audioSlowTimeStop;
    [SerializeField] GlitchEffect glitchEffect;
    [SerializeField] ValueSo slowTimeSliderData;
    [SerializeField] EventSO slowTimeStartEvent;
    [SerializeField] EventSO slowTimeStopEvent;
    bool InPause = false;

    private void OnEnable()
    {
        input = InputManager.Input;
        if(input != null)
        {
            input.InGame.SlowTime.performed += context => StartSlowTime();
            input.InGame.SlowTime.canceled += context => StopSlowTime();

            pauseEvent.OnLaunchEvent += Pause;
        }
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.InGame.SlowTime.performed -= context => StartSlowTime();
            input.InGame.SlowTime.canceled -= context => StopSlowTime();
            pauseEvent.OnLaunchEvent -= Pause;
        }
    }

    public void StartSlowTime()
    {
        if (!InPause)
        {
            isSlowTime = true;
            audioSlowTimeStart.PlayAudioCue();
            slowTimeStartEvent.OnLaunchEvent.Invoke();
            glitchEffect.enabled = true;
            Time.timeScale = 0.5f;
        }
    }
    public void StopSlowTime()
    {
        if(isSlowTime)
        {
            isSlowTime = false;
            audioSlowTimeStop.PlayAudioCue();
            slowTimeStopEvent.OnLaunchEvent.Invoke();
            glitchEffect.enabled = false;
            Time.timeScale = 1;
        }
    }

    public void Pause()
    {
        InPause = !InPause;
    }
}
