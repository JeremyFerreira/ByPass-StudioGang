using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostClass
{
    public string LevelName;
    public List<Vector3> positionPlayer;
    public List<Quaternion> rotationPlayer;
    public List<float> time;

    public GhostClass()
    {
        positionPlayer = new List<Vector3>();
        rotationPlayer = new List<Quaternion>();
        time = new List<float>();
    }

    public void SaveData(Vector3 position, Quaternion rotation, float time)
    {
        positionPlayer.Add(position);
        rotationPlayer.Add(rotation);
        this.time.Add(time);
    }

}
