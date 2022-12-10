using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] EventSO _eventStartLevel;
    [SerializeField] EventSO _eventStartRun;
    [SerializeField] TimeSO _countDownData;
    [SerializeField] float timeOfCountDown;
    [SerializeField] float[] writeinCountDown;

    int count;

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
        yield return new WaitForSecondsRealtime(timeOfCountDown / writeinCountDown.Length);
        _countDownData.ChangeTimer(writeinCountDown[count]);
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
