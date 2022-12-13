using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ghost : MonoBehaviour
{
    [SerializeField]
    private GhostsSO _ghostsToShow;

    [SerializeField]
    private TimeSO _timerData;

    [SerializeField]
    private EventSO _eventStartRun;
    [SerializeField]
    private EventSO _eventReachFinshLine;
    [SerializeField]
    private EventSO _eventPause;

    [SerializeField]
    private GhostSO _ghostContent;

    [SerializeField]
    private Transform _playerTransform;

    #region Save Variables
    [SerializeField]
    private float _recurenceSave;
    private DataGhost _saveGhost;
    private bool _save;
    private bool _pause;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InitGhost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Fonction Init
    private void InitGhost ()
    {
        InitGhostsToShow();
        InitEvents();
        _saveGhost = new DataGhost(DataManager.Instance.GetSceneData(SceneManager.GetActiveScene().buildIndex).ToString());
        _save = true;
    }

    private void InitEvents ()
    {
        _eventStartRun.OnLaunchEvent += StartCoroutineSave;
        _eventReachFinshLine.OnLaunchEvent += SaveGhostinContent;
        _eventPause.OnLaunchEvent += PauseSaveGhost;
    }

    private void InitGhostsToShow ()
    {
        for(int i = 0; i<_ghostsToShow.ghostsToShow.Count;i++)
        {
            GameObject ghostSpawn = new GameObject("ghosts" + i, typeof(GhostController));
            GhostController ghostSpawnController = ghostSpawn.GetComponent<GhostController>();
            ghostSpawnController.InitGhostController(_ghostsToShow.ghostsToShow[i]);
        }
    }
    #endregion


    private void SaveTimeAndPositionGhost ()
    {
        _saveGhost.AddTransfomTime(_playerTransform.position, _timerData.TotalTime);
    }


    #region Coroutine Gestion Save
    private void StartCoroutineSave ()
    {
        _save = true;
        StartCoroutine(CoroutineSaveTime());
    }

    private void StopCoroutineSave ()
    {
        _save = false;
        StopAllCoroutines();
    }

    private void SaveGhostinContent ()
    {
        StopCoroutineSave();
        _ghostContent.Ghost = _saveGhost;
    }

    private void PauseSaveGhost ()
    {
        if(!_pause)
        {
            StopCoroutineSave();
            _pause = true;
        }
        else
        {
            StartCoroutineSave();
            _pause = false;
        }
    }

    IEnumerator CoroutineSaveTime ()
    {
        do
        {
            Debug.Log("je suis appeler");
            SaveTimeAndPositionGhost();
            yield return new WaitForSeconds(_recurenceSave);
        }
        while (_save);
    }
    #endregion

    private void OnDisable()
    {
        _eventStartRun.OnLaunchEvent -= StartCoroutineSave;
        _eventReachFinshLine.OnLaunchEvent -= SaveGhostinContent;
        _eventPause.OnLaunchEvent -= PauseSaveGhost;
    }
}
