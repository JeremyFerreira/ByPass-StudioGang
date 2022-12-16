using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardHUD : MonoBehaviour
{
    [SerializeField] LederboardSO LeaderboardSO;
    [SerializeField] GameObject PanelLeaderboard;
    [SerializeField] Transform parentPanel;

    [SerializeField] List<GameObject> panel;

    private void Start()
    {
        panel = new List<GameObject>();
    }
    private void OnEnable()
    {
        LeaderboardSO.OnValueChange += OnValueChange;
    }

    private void OnDisable()
    {
        LeaderboardSO.OnValueChange -= OnValueChange;
    }

    void OnValueChange()
    {
        foreach (var item in panel)
        {
            Destroy(item);
        }
        panel.Clear();

        foreach (var item in LeaderboardSO.Leaderboard)
        {
            GameObject go = Instantiate(PanelLeaderboard, parentPanel);
            panel.Add(go);
            go.GetComponent<PanelHighScore>().ChangeValue(item);
        }
    }
}
