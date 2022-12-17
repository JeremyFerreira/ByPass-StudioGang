using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PanelHighScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textposition;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] TextMeshProUGUI textTime;

    LeaderboardValue leaderboardData;

    public void ChangeValue(LeaderboardValue value)
    {
        leaderboardData = value;
        textposition.text = (value.Position+1).ToString();
        textName.text = value.DisplayName;
        float i = ((float)value.StatValue / 1000) * -1;
        textTime.text = TimerFormat.FormatTime(i);

    }
}
