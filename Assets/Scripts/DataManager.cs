using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [field: SerializeField] public List<World> AllWorld { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);    // Suppression d'une instance précédente (sécurité...sécurité...)

        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void ModifyBestTime(string levelName, float value)
    {
        for (int i = 0; i < AllWorld.Count; i++)
        {
            for (int j = 0; j < AllWorld[i].WorldData.Count; j++)
            {
                if (AllWorld[i].WorldData[j].MapName == levelName)
                {
                    AllWorld[i].WorldData[j].BestTime = value;
                    return;
                }
            }
        }
    }
}

[System.Serializable]
public class World
{
    public List<SceneSO> WorldData;
}

[System.Serializable]
public class GhostSave
{
    public List<GhostClass> FantomeData;

    public GhostSave()
    {
        FantomeData = new List<GhostClass>();
    }
}
