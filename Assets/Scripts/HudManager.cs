using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using Doozy.Runtime.UIManager.Components;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using TMPro;

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
    [SerializeField] EventSO _eventPause;

    [Header("LevelSelector")]
    [SerializeField] GameObject _parentLevelSelection;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject CardWorldPrefab;
    [SerializeField] TextMeshProUGUI starText;

    static bool created = false;
    bool _pause = false;
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
        _eventPause.OnLaunchEvent += OpenPauseMenu;
    }
    private void OnDisable()
    {
        _eventReachFinishLine.OnLaunchEvent -= OpenWinPanel;
        _eventStartLevel.OnLaunchEvent -= OpenInGamePanel;
        _eventInMainMenu.OnLaunchEvent -= OpenMainMenu;
        _eventPause.OnLaunchEvent -= OpenPauseMenu;
    }
    private void OpenPauseMenu()
    {
        if (_pause)
        {
            OpenInGamePanel();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _pause = false;
        }
        else
        {
            _pause = true;
            CloseAllPanel();
            _pausePanel.Show();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
    private void OpenMainMenu()
    {
        CloseAllPanel();
        _mainMenu.Show();
    }

    public void OpenLevelSelector(int worldIndex)
    {
        CloseAllPanel();
        _levelSelectionPanel.Show();
        int totalStar = 0;
        int starUnlock = 0;


        foreach (var item in _parentLevelSelection.GetComponentsInChildren<CardWorld>())
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < DataManager.Instance.AllWorld[worldIndex].WorldData.Count; i++)
        {
            GameObject cardObj = Instantiate(CardWorldPrefab, _parentLevelSelection.transform);
            int starLevel = 0;
            SceneSO mapData = DataManager.Instance.AllWorld[worldIndex].WorldData[i];
            for (int j = 0; j < mapData.TimeStar.Length; j++)
            {
                totalStar++;
                if (mapData.BestTime <= mapData.TimeStar[j] && mapData.BestTime != 0)
                {
                    starLevel++;
                    starUnlock++;
                }
            }

            if (i == 0)
            {
                eventSystem.SetSelectedGameObject(cardObj.GetComponent<UIButton>().gameObject);
            }

            cardObj.GetComponent<CardWorld>().ChangeInformation(mapData, mapData.BestTime, totalStar, true, i);
        }

        starText.text = "STAR : " + starUnlock.ToString() + " / " + totalStar.ToString();
    }

    private void OpenInGamePanel()
    {
        CloseAllPanel();
        _InGamePanel.Show();
        _pause = false;
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
