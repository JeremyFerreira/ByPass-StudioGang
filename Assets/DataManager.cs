using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [SerializeField] List<World> AllWorld;
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
                if (AllWorld[i].WorldData[j].name == levelName)
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
