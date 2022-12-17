using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioComponent))]
public class CountDown : MonoBehaviour
{
    [SerializeField] EventSO _eventStartLevel;
    [SerializeField] EventSO _eventStartRun;
    [SerializeField] TimeSO _countDownData;
    [SerializeField] float timeOfCountDown;
    [SerializeField] float[] writeinCountDown;

    AudioComponent _audioComponent;
    [SerializeField] RumblerDataConstant countDownRumbler;

    int count = 0;
    private void Awake()
    {
        count = 0;
        _audioComponent = GetComponent<AudioComponent>();
    }
    private void OnEnable()
    {
        _eventStartLevel.OnLaunchEvent += StartCountDown;
    }
    private void OnDisable()
    {
        _eventStartLevel.OnLaunchEvent -= StartCountDown;
    }
    public void StartCountDown()
    {
        count = 0;
        StartCoroutine(CoroutineCountDown());
    }

    IEnumerator CoroutineCountDown()
    {
        yield return new WaitForSeconds(timeOfCountDown / writeinCountDown.Length);
        _countDownData.ChangeTimer(writeinCountDown[count]);
        _audioComponent.PlayAudioCue();
        Rumbler.instance.RumbleConstant(countDownRumbler);
        count++;
        if (count < writeinCountDown.Length)
        {
            StartCoroutine(CoroutineCountDown());
        }
        else
        {
            _eventStartRun.OnLaunchEvent?.Invoke();
        }
    }
}
