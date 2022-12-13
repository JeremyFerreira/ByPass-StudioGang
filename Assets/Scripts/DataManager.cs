using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] EventSO _reachFinishLine;
    [SerializeField] EventSO _eventBestScore;
    [SerializeField] EventSO _getPosPlayer;
    [SerializeField] EventSO _eventStartRun;
    [SerializeField] TimeSO _timer;

    [SerializeField] GhostsSO _ghostsToShow;
    [SerializeField] GhostSO ghostSave;
    [field: SerializeField] public List<World> AllWorld { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }



    private void OnEnable()
    {
        ghostSave.ValueChange += SaveGhost;
        _reachFinishLine.OnLaunchEvent += OnFinishLine;
        _eventStartRun.OnLaunchEvent += AddGhost;
    }

    private void SaveGhost()
    {
        for (int i = 0; i < AllWorld.Count; i++)
        {
            for (int j = 0; j < AllWorld[i].WorldData.Count; j++)
            {
                if (AllWorld[i].WorldData[j].MapName == ghostSave.Ghost.LevelName)
                {
                    GhostSO ghost = ScriptableObject.CreateInstance<GhostSO>();
                    ghost.Ghost = ghostSave.Ghost;
                    AllWorld[i].WorldData[j].GhostPlayer = ghost;

                    PlayfabGhost.Instance.SaveGhost(i);
                }
            }
        }
    }

    private void OnDisable()
    {
        ghostSave.ValueChange -= SaveGhost;
        _reachFinishLine.OnLaunchEvent -= OnFinishLine;
        _eventStartRun.OnLaunchEvent -= AddGhost;
    }

    private void OnFinishLine()
    {
        StartCoroutine(Wait());
    }

    private void AddGhost()
    {
        SceneSO data = GetSceneData(SceneManager.GetActiveScene().buildIndex);
        if (data.GhostPlayer != null)
            _ghostsToShow.AddGhostPlayer(data.GhostPlayer.Ghost);
    }


    private void ReachFinishLine()
    {
        SceneSO data = DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex);

        if (data.BestTime == 0 || _timer.TotalTime < data.BestTime)
        { 
            data.BestTime = _timer.TotalTime;
            _eventBestScore.OnLauchEventSceneSO?.Invoke(data);
            _eventBestScore.OnLaunchEvent?.Invoke();
        }
        else
        {
            _getPosPlayer.OnLaunchEvent?.Invoke();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.3f);
        ReachFinishLine();
    }
    public void ModifyBestTime(string levelName, float value)
    {
        value *= -1;
        for (int i = 0; i < AllWorld.Count; i++)
        {
            for (int j = 0; j < AllWorld[i].WorldData.Count; j++)
            {
                if (AllWorld[i].WorldData[j].MapName == levelName)
                {
                    AllWorld[i].WorldData[j].BestTime = value;
                    return;
                }
            }
        }
    }

    public SceneSO GetSceneData(int sceneIndex)
    {
        for (int i = 0; i < AllWorld.Count; i++)
        {
            for (int j = 0; j < AllWorld[i].WorldData.Count; j++)
            {
                if (AllWorld[i].WorldData[j].IndexScene == sceneIndex)
                {
                    return AllWorld[i].WorldData[j];
                }
            }
        }

        return null;
    }
}

[System.Serializable]
public class World
{
    public List<SceneSO> WorldData;
}

[System.Serializable]
public class GhostSave
{
    public List<DataGhost> FantomeData;

    public GhostSave()
    {
        FantomeData = new List<DataGhost>();
    }
}
