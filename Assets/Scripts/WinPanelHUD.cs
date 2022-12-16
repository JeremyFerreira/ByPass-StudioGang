using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanelHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textScore;
    [SerializeField] TextMeshProUGUI _textBestScore;
    [SerializeField] TextMeshProUGUI _timeBetweenScore;
    [SerializeField] TextMeshProUGUI _textNewHighScore;
    [SerializeField] TextMeshProUGUI _textPosPlayer;
    [SerializeField] EventSO _eventReachLine;
    [SerializeField] EventSO _eventChangePos;
    [SerializeField] TimeSO _timer;
    [SerializeField] Image currentMedal;
    [SerializeField] Sprite[] spritesMedal;
    Animator animator;
    int medal;
    public void SetMedal(int i)
    {
        medal = i;
    }
    [SerializeField] EventSO _eventHighScore;
    // Start is called before the first frame update
    void Start()
    {
        _textNewHighScore.enabled = false;
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        _eventReachLine.OnLaunchEvent += ReachLine;
        _eventChangePos.OnLauchEventInt += ChangePosPlayer;
        _eventHighScore.OnLaunchEvent += UpdateUIHighScore;
    }
    private void OnDisable()
    {
        _eventReachLine.OnLaunchEvent -= ReachLine;
        _eventChangePos.OnLauchEventInt -= ChangePosPlayer;
        _eventHighScore.OnLaunchEvent -= UpdateUIHighScore;
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

        float difference = data.BestTime - _timer.TotalTime;
        _timeBetweenScore.text = difference < 0 ? "+ " + TimerFormat.FormatTime(Mathf.Abs(difference)) : "- " + TimerFormat.FormatTime(Mathf.Abs(difference));
        _timeBetweenScore.color = difference < 0 ? Color.red : Color.blue;
        animator.CrossFade("WinPanel", 0, 0);
    }
    private void UpdateUIHighScore()
    {
        if (medal <= 2)
        {
            currentMedal.sprite = spritesMedal[medal];
        }
        else
        {
            currentMedal.sprite = null;
        }
        _textNewHighScore.enabled = true;
    }
}
