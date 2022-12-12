using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Leaderboard", menuName = "New Leaderboard", order = 1)]
public class LederboardSO : ScriptableObject
{
    public delegate void LaunchEvent();
    public LaunchEvent OnValueChange;

    public List<LeaderboardValue> Leaderboard = new List<LeaderboardValue>();

    public void AddValueOnLeaderBoard(List<LeaderboardValue> leaderboard)
    {
        Leaderboard = leaderboard;
        OnValueChange?.Invoke();
    }
}

public class LeaderboardValue
{
    public string DisplayName;
    public int Position;
    public int StatValue;
    public string AvatarUrl;
    public string PlayfabID;

    public LeaderboardValue(string displayName, int position, int statValue, string avatarUrl, string playfabID)
    {
        DisplayName = displayName;
        Position = position;
        StatValue = statValue;
        AvatarUrl = avatarUrl;
        PlayfabID = playfabID;
    }
}
