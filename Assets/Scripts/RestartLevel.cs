using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevel : MonoBehaviour
{
    [SerializeField] SOInputButton sOInputButton;
    [SerializeField] EventSO restartEvent;
    [SerializeField] EventSO deathEvent;
    [SerializeField] EventSO startRun;
    bool _isStartRun;
    bool _isAlive;
    // Start is called before the first frame update
    void OnEnable()
    {
        sOInputButton.OnPressed += RestartTheLevel;
        deathEvent.OnLaunchEvent += Dead;
        startRun.OnLaunchEvent += StartRun;
    }
    private void Start()
    {
        _isAlive = true;
        _isStartRun = false;
    }
    void OnDisable()
    {
        sOInputButton.OnPressed -= RestartTheLevel;
        deathEvent.OnLaunchEvent -= Dead;
        startRun.OnLaunchEvent -= StartRun;
    }
    public void RestartTheLevel()
    {
        if(_isAlive && _isStartRun)
        {
            restartEvent.OnLaunchEvent.Invoke();
        }
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
