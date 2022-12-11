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
    [SerializeField] float timeTotal = 3;
    [SerializeField] float timeLeft;
    [SerializeField] Slider slider;
    [SerializeField] SOInputButton slowTimeInput;

    [SerializeField] EventSO startRun;
    [SerializeField] EventSO stopRun;
    [SerializeField] EventSO pauseEvent;

    [SerializeField] AudioComponent audioSlowTimeStart;
    [SerializeField] AudioComponent audioSlowTimeStop;
    [SerializeField] GlitchEffect glitchEffect;

    bool InPause = false;

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
        pauseEvent.OnLaunchEvent += Pause;
    }

    private void OnDisable()
    {
        startRun.OnLaunchEvent -= EnableInput;
        stopRun.OnLaunchEvent -= DisableInput;
        pauseEvent.OnLaunchEvent -= Pause;
        DisableInput();
    }
    private void Start()
    {
        timeLeft = timeTotal;
    }
    // Update is called once per frame
    void Update()
    {
        if (!InPause)
        {
        slider.value = 1-((timeTotal - timeLeft) / timeTotal);
        Debug.Log(1 - ((timeTotal - timeLeft) / timeTotal));
            if (isSlowTime)
            {
                slider.gameObject.SetActive(true);
                glitchEffect.enabled = true;
                Time.timeScale = 0.5f;
                timeLeft -= Time.unscaledDeltaTime;
                if (timeLeft <= 0)
                {
                    StopSlowTime();
                }
            }
            else
            {
                glitchEffect.enabled = false;
                slider.gameObject.SetActive(false);
                Time.timeScale = 1f;
                if (timeLeft < timeTotal)
                {
                    timeLeft += Time.unscaledDeltaTime * 0.5f;
                }
                else
                {
                    timeLeft = timeTotal;
                }
            }
        }
    }
    public void StartSlowTime()
    {
        
        isSlowTime = true;
        audioSlowTimeStart.PlayAudioCue();
        
    }
    public void StopSlowTime()
    {
        if(isSlowTime)
        {
            isSlowTime = false;
            audioSlowTimeStop.PlayAudioCue();
        }
    }

    public void Pause()
    {
        InPause = !InPause;
    }
}
