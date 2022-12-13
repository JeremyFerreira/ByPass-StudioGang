using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataGhost 
{
    #region Variable
    private string _levelName;

    [SerializeField] private List<Vector3> _transfomPlayer;
    [SerializeField] private List<float> _timeTransforme;
    #endregion

    #region Accessor
    public string LevelName
    {
        get { return _levelName; }
    }
    public List<Vector3> TransfomPlayer
    {
        get { return _transfomPlayer; }
    }
    public List<float> TimeTransforme
    {
        get { return _timeTransforme; }
    }
    #endregion

    #region Fonction
    public DataGhost (string levelName)
    {
        _levelName = levelName;
        _transfomPlayer = new List<Vector3>();
        _timeTransforme = new List<float>();
    }
    public void AddTransfomTime(Vector3 transform, float time)
    {
        _transfomPlayer.Add(transform);
        _timeTransforme.Add(time);
    }
    #endregion
}
