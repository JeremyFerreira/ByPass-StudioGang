using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabHighScore : MonoBehaviour
{
    public static PlayFabHighScore Instance;

    [SerializeField] EventSO NewHighScoreEvent;
    [SerializeField] EventSO _eventChangePos;
    [SerializeField] EventSO _eventOnReachLine;

    [SerializeField] EventSO eventGetLeaderboardAroundPlayer;
    [SerializeField] EventSO leaderBoardTop;
    [SerializeField] EventSO leaderBoardFriend;

    [SerializeField] LederboardSO leaderboardSO;

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
        NewHighScoreEvent.OnLauchEventSceneSO += NewHighScore;
        _eventOnReachLine.OnLaunchEvent += GetPosPlayer;

        eventGetLeaderboardAroundPlayer.OnLauchEventSceneSO += GetLeaderBoardAroundPlayer;
        leaderBoardTop.OnLauchEventSceneSO += GetTopLeaderBord;
        leaderBoardFriend.OnLauchEventSceneSO += GetFriendLeaderBoard;
    }

    private void OnDisable()
    {
        NewHighScoreEvent.OnLauchEventSceneSO -= NewHighScore;
        _eventOnReachLine.OnLaunchEvent -= GetPosPlayer;

        eventGetLeaderboardAroundPlayer.OnLauchEventSceneSO -= GetLeaderBoardAroundPlayer;
        leaderBoardTop.OnLauchEventSceneSO -= GetTopLeaderBord;
        leaderBoardFriend.OnLauchEventSceneSO -= GetFriendLeaderBoard;
    }

    void NewHighScore(SceneSO data)
    {
        UpdateHighScoreCloud(data.MapName, data.BestTime*1000);
    }

    public void UpdateHighScoreCloud(string levelName, float timer)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdateLeaderboard",
            FunctionParameter = new { Leaderboardname = levelName, Playerscore = (int)-timer },
            GeneratePlayStreamEvent = true,
        }, OnCloudResultUpdateLeaderBoard, OnError);
    }

    void OnCloudResultUpdateLeaderBoard(ExecuteCloudScriptResult result)
    {
        _eventChangePos.OnLauchEventInt?.Invoke(int.Parse(result.FunctionResult.ToString()) + 1);
        Debug.Log(result.FunctionResult.ToString());
        //HudMainMenu.Instance.ChangePosPlayerText(int.Parse(result.FunctionResult.ToString()));
    }

    public void GetPosPlayer()
    {
        SceneSO data = DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex);
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "GetLeaderboardPosition",
            FunctionParameter = new { Leaderboardname = data.MapName},
            GeneratePlayStreamEvent = true,
        }, OnCloudResult, OnError);
    }

    void OnCloudResult(ExecuteCloudScriptResult result)
    {
        _eventChangePos.OnLauchEventInt?.Invoke(int.Parse(result.FunctionResult.ToString()) + 1);
        Debug.Log(result.FunctionResult.ToString());
        //HudMainMenu.Instance.ChangePosPlayerText(int.Parse(result.FunctionResult.ToString()));
    }

    public void GetTopLeaderBord(SceneSO mapName)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = mapName.MapName,
            StartPosition = 0,
            MaxResultsCount = 50,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardTopGet, OnError);
    }

    public void GetLeaderBoardAroundPlayer(SceneSO mapName)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = mapName.MapName,
            MaxResultsCount = 50,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }

    public void GetFriendLeaderBoard(SceneSO mapName)
    {
        var request = new GetFriendLeaderboardRequest
        {
            StatisticName = mapName.MapName,
            MaxResultsCount = 100,
            IncludeSteamFriends = true,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetFriendLeaderboard(request, OnLeaderboardFriend, OnError);
    }

    void OnLeaderboardFriend(GetLeaderboardResult result)
    {
        List<LeaderboardValue> value = new List<LeaderboardValue>();
        foreach (var item in result.Leaderboard)
        {
            value.Add(new LeaderboardValue(item.Profile.DisplayName, item.Position, item.StatValue, item.Profile.AvatarUrl, item.PlayFabId));
        }
        leaderboardSO.AddValueOnLeaderBoard(value);
    }

    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        List<LeaderboardValue> value = new List<LeaderboardValue>();
        foreach (var item in result.Leaderboard)
        {
            value.Add(new LeaderboardValue(item.Profile.DisplayName, item.Position, item.StatValue, item.Profile.AvatarUrl, item.PlayFabId));
        }
        leaderboardSO.AddValueOnLeaderBoard(value);
    }

    void OnLeaderboardTopGet(GetLeaderboardResult result)
    {
        List<LeaderboardValue> value = new List<LeaderboardValue>();
        foreach (var item in result.Leaderboard)
        {
            value.Add(new LeaderboardValue(item.Profile.DisplayName, item.Position, item.StatValue, item.Profile.AvatarUrl, item.PlayFabId));
        }
        leaderboardSO.AddValueOnLeaderBoard(value);
    }



    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
}
