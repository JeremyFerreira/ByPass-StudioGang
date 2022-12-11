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
    static bool created = false;
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

    private void OnEnable()
    {
        _eventDeath.OnLaunchEvent += Death;
        _eventRestart.OnLaunchEvent += Restart;
    }
    private void OnDisable()
    {
        _eventDeath.OnLaunchEvent -= Death;
        _eventRestart.OnLaunchEvent -= Restart;
    }
    public void ChangeLevel()
    {
        if (_haveAlreadySeeCinematique)
        {
            _eventStartLevel.OnLaunchEvent?.Invoke();
        }
        else
        {
            _eventStartCinematique.OnLaunchEvent?.Invoke();
            _haveAlreadySeeCinematique = true;
        }
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
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        while (!operation.isDone)
        {
            yield return null;
        }
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
