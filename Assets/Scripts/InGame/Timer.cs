using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] EventSO _startRunEvent;
    [SerializeField] EventSO _PauseEvent;
    [SerializeField] EventSO _reachFinishLineEvent;
    [SerializeField] TimeSO _timer;
    [SerializeField] bool _timerIsLaunch;

    private void Start()
    {
        _timerIsLaunch = false;
    }
    private void OnEnable()
    {
        _startRunEvent.OnLaunchEvent += LaunchRun;
        _reachFinishLineEvent.OnLaunchEvent += StopTimer;
        _PauseEvent.OnLaunchEvent += Pause;
    }
    private void OnDisable()
    {
        _startRunEvent.OnLaunchEvent -= LaunchRun;
        _reachFinishLineEvent.OnLaunchEvent -= StopTimer;
        _PauseEvent.OnLaunchEvent -= Pause;
    }

    private void Pause()
    {
        if (!_timerIsLaunch)
        {
            _timerIsLaunch = true;
        }
        else
        {
            _timerIsLaunch = false;
        }

    }
    private void LaunchRun()
    {
        _timer.ChangeTimer(0f);
        _timerIsLaunch = true;
    }
    private void StopTimer()
    {
        _timerIsLaunch = false;
    }

    private void Update()
    {
        if (_timerIsLaunch)
        {
            _timer.ChangeTimer(_timer.TotalTime + Time.unscaledDeltaTime);
        }
    }
}
