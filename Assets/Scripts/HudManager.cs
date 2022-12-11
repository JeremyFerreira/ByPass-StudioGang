using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Components;
public class HudManager : MonoBehaviour
{
    [Header("PANEL")]
    [SerializeField] UIContainer _mainMenu;
    [SerializeField] UIContainer _settings;
    [SerializeField] UIContainer _levelSelectionPanel;
    [SerializeField] UIContainer _worldSelectionPanel;
    [SerializeField] UIContainer _pausePanel;
    [SerializeField] UIContainer _InGamePanel;
    [SerializeField] UIContainer _DeathPanel;
    [SerializeField] UIContainer _winPanel;
    [SerializeField] UIContainer _HighScorePanel;

    [Header("EVENT")]
    [SerializeField] EventSO _eventReachFinishLine;
    [SerializeField] EventSO _eventStartLevel;
    [SerializeField] EventSO _eventInMainMenu;
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
        _eventReachFinishLine.OnLaunchEvent += OpenWinPanel;
        _eventStartLevel.OnLaunchEvent += OpenInGamePanel;
        _eventInMainMenu.OnLaunchEvent += OpenMainMenu;
    }
    private void OnDisable()
    {
        _eventReachFinishLine.OnLaunchEvent -= OpenWinPanel;
        _eventStartLevel.OnLaunchEvent -= OpenInGamePanel;
        _eventInMainMenu.OnLaunchEvent -= OpenMainMenu;
    }

    private void OpenMainMenu()
    {
        CloseAllPanel();
        _mainMenu.Show();
    }

    private void OpenInGamePanel()
    {
        CloseAllPanel();
        _InGamePanel.Show();
    }

    private void OpenWinPanel()
    {
        CloseAllPanel();
        _winPanel.Show();
    }

    public void CloseAllPanel()
    {
        _mainMenu.Hide();
        _settings.Hide();
        _levelSelectionPanel.Hide();
        _worldSelectionPanel.Hide();
        _pausePanel.Hide();
        _InGamePanel.Hide();
        _DeathPanel.Hide();
        _winPanel.Hide();
        _HighScorePanel.Hide();
    }
}
