using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] bool _haveAlreadySeeCinematique;
    [SerializeField] GameState _gameState;
    [SerializeField] EventSO _eventStartLevel;
    [SerializeField] EventSO _eventLaunchLevel;
    [SerializeField] EventSO _eventStartCinematique;

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
}

public enum GameState
{
    InMainMenu,
    InCinematique,
    InGame,
    InPause
}
