using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevel : MonoBehaviour
{
    [SerializeField] InputSO inputSO;
    [SerializeField] EventSO restartEvent;
    [SerializeField] EventSO deathEvent;
    [SerializeField] EventSO startRun;
    bool _isStartRun;
    bool _isAlive;
    public bool hasRestarted;
    // Start is called before the first frame update
    void OnEnable()
    {
        inputSO.OnRestartPressed += RestartTheLevel;
        deathEvent.OnLaunchEvent += Dead;
        startRun.OnLaunchEvent += StartRun;
    }
    private void Start()
    {
        _isAlive = true;
        _isStartRun = false;
        hasRestarted = false;
    }
    void OnDisable()
    {
        inputSO.OnRestartPressed -= RestartTheLevel;
        deathEvent.OnLaunchEvent -= Dead;
        startRun.OnLaunchEvent -= StartRun;
    }
    public void RestartTheLevel()
    {
        if(_isAlive && _isStartRun)
        {
            restartEvent.OnLaunchEvent.Invoke();
        }
        hasRestarted = true;
    }
    private void Dead()
    {
        _isAlive = false;
    }
    private void StartRun()
    {
        _isStartRun = true;
    }

}
