using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    static bool created = false;
    bool alreadyLoad;
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

    private void Pause()
    {
        if (Time.timeScale == 0)
        {
            
            Time.timeScale = 1;
        }
        else
            Time.timeScale = 0;

    }

    private void OnEnable()
    {
        _eventStartLevel.OnLaunchEvent += StartLevel;
        _eventDeath.OnLaunchEvent += Death;
        _eventRestart.OnLaunchEvent += Restart;
        _eventPause.OnLaunchEvent += Pause;
    }
    private void OnDisable()
    {
        _eventStartLevel.OnLaunchEvent -= StartLevel;
        _eventDeath.OnLaunchEvent -= Death;
        _eventRestart.OnLaunchEvent -= Restart;
        _eventPause.OnLaunchEvent -= Pause;
    }

    public void StartLevel()
    {
        Time.timeScale = 1;
    }

    public void Restart()
    {
        StartCoroutine(LoadScene());
    }

    public void Death()
    {
        StartCoroutine(DeathTimer());
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(3f);
        _eventRestart.OnLaunchEvent?.Invoke();
    }

    IEnumerator LoadScene()
    {
        if (!alreadyLoad)
        {
            alreadyLoad = true;
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        while (!operation.isDone)
        {
            yield return null;
        }
            Debug.Log(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
            _eventStartLevel.OnLaunchEvent?.Invoke();
            alreadyLoad = false;
        }    
    }


}

public enum GameState
{
    InMainMenu,
    InCinematique,
    InGame,
    InPause
}
