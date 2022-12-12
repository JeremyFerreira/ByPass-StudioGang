using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinPanelHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textScore;
    [SerializeField] TextMeshProUGUI _textBestScore;
    [SerializeField] TextMeshProUGUI _textPosPlayer;
    [SerializeField] EventSO _eventReachLine;
    [SerializeField] EventSO _eventChangePos;
    [SerializeField] TimeSO _timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        _eventReachLine.OnLaunchEvent += ReachLine;
        _eventChangePos.OnLauchEventInt += ChangePosPlayer;
    }
    private void OnDisable()
    {
        _eventReachLine.OnLaunchEvent -= ReachLine;
        _eventChangePos.OnLauchEventInt += ChangePosPlayer;
    }
    void ChangePosPlayer(int pos)
    {
        _textPosPlayer.text = "TOP # " + pos.ToString();
    }
    void ReachLine()
    {
        SceneSO data = DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex);
        _textScore.text = TimerFormat.FormatTime(_timer.TotalTime);
        _textBestScore.text = "BEST TIME : " + TimerFormat.FormatTime(data.BestTime);
    }
}
