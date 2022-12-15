using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField] bool _haveAlreadySeeCinematique;
    [SerializeField] GameState _gameState;
    [SerializeField] EventSO _eventStartLevel;
    [SerializeField] EventSO _eventStartCinematique;
    [SerializeField] EventSO _eventDeath;
    [SerializeField] EventSO _eventRestart;
    [SerializeField] EventSO _eventPause;
    [SerializeField] EventSO _eventReachLine;
    [SerializeField] EventSO _eventResume;

    static bool created = false;
    bool alreadyLoad = false;
    bool _win = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Resume()
    {
        if (!_win)
        {
            Time.timeScale = 1;
        }
    }
    private void Pause()
    {
        if (!_win)
        {
            Time.timeScale = 0;
        }
    }

    private void OnEnable()
    {
        _eventStartLevel.OnLaunchEvent += StartLevel;
        _eventDeath.OnLaunchEvent += Death;
        _eventRestart.OnLaunchEvent += Restart;
        _eventPause.OnLaunchEvent += Pause;
        _eventReachLine.OnLaunchEvent += Win;
        _eventResume.OnLaunchEvent += Resume;
    }
    private void OnDisable()
    {
        _eventStartLevel.OnLaunchEvent -= StartLevel;
        _eventDeath.OnLaunchEvent -= Death;
        _eventRestart.OnLaunchEvent -= Restart;
        _eventPause.OnLaunchEvent -= Pause;
        _eventReachLine.OnLaunchEvent -= Win;
        _eventResume.OnLaunchEvent -= Resume;
    }
    private void Win()
    {
        _win = true;
    }
    public void StartLevel()
    {
        Time.timeScale = 1;
        _win = false;
        alreadyLoad = false;
    }

    public void Restart()
    {
        if (!alreadyLoad)
        {
            alreadyLoad = true;
            StartCoroutine(LoadScene());
        }
    }

    public void Death()
    {
        StartCoroutine(DeathTimer());
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(0.5f);
        _eventRestart.OnLaunchEvent?.Invoke();
    }

    IEnumerator LoadScene()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        while (!operation.isDone)
        {
            yield return null;
        }
        Debug.Log(SceneManager.GetActiveScene().name);
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        _eventStartLevel.OnLaunchEvent?.Invoke();
    }
}


public enum GameState
{
    InMainMenu,
    InCinematique,
    InGame,
    InPause
}
