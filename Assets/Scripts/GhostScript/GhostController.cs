using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private DataGhost _dataGhost;

    private EventSO _eventStartRun;
    private EventSO _pauseEvent;

    private TimeSO _timerData;

    private bool _pause;
    private bool _startReproduce;

    private int _indexOfPath;
    private float _duration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(_startReproduce)
        {
            ReproducePath();
        }
    }

    public void InitGhostController (DataGhost dataghost, TimeSO dataTime, EventSO eventStartRun, EventSO pauseEvent)
    {
        _dataGhost = dataghost;
        gameObject.transform.position = _dataGhost.TransfomPlayer[0];

        _timerData = dataTime;

        _eventStartRun = eventStartRun;
        _pauseEvent = pauseEvent;

        InitEvents();
    }

    private void InitEvents ()
    {
        _eventStartRun.OnLaunchEvent += StartReproduce;
        _pauseEvent.OnLaunchEvent += PauseReproduce;
    }

    private void StartReproduce ()
    {
        _startReproduce = true;
    }

    private void StopReproduce ()
    {
        _startReproduce = false;
    }

    private void PauseReproduce ()
    {
        if(_pause)
        {
            _pause = false;
            StartReproduce();
        }
        else
        {
            _pause = true;
            StopReproduce();
        }
    }

    private void ReproducePath ()
    {
        if (_indexOfPath < _dataGhost.TransfomPlayer.Count)
        {

            if (_indexOfPath + 1 < _dataGhost.TransfomPlayer.Count)
            {

                if (_dataGhost.TimeTransforme[_indexOfPath + 1] <= _timerData.TotalTime)
                {
                    _indexOfPath++;

                    if (_indexOfPath + 1 < _dataGhost.TransfomPlayer.Count)
                    {
                        _duration = _dataGhost.TimeTransforme[_indexOfPath + 1] - _dataGhost.TimeTransforme[_indexOfPath];
                    }
                    else
                    {
                        if (_indexOfPath + 1 >= _dataGhost.TransfomPlayer.Count)
                        {
                            Debug.Log("Fantome Arriver " + gameObject.name);
                            _startReproduce = false;
                        }
                    }
                }
                if (_indexOfPath + 1 < _dataGhost.TransfomPlayer.Count)
                {
                    float timePass = _timerData.TotalTime - _dataGhost.TimeTransforme[_indexOfPath];
                    if (timePass < 0)
                    {
                        timePass = 0;
                    }
                    float lerpPercent = timePass / _duration;

                    gameObject.transform.position = Vector3.Lerp(_dataGhost.TransfomPlayer[_indexOfPath], _dataGhost.TransfomPlayer[_indexOfPath + 1], lerpPercent);
                }
            }
        }
    }
}
