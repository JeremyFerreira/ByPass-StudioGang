using PlayFab;
using PlayFab.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using PlayFab.ClientModels;

public class PlayfabGhost : MonoBehaviour
{
    public static PlayfabGhost Instance;
    private readonly Dictionary<string, string> _entityFileJson = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _tempUpdates = new Dictionary<string, string>();
    public string ActiveUploadFileName;
    public string NewFileName;
    public int GlobalFileLock = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void OnSharedFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        GlobalFileLock -= 1;
    }
    public void SaveGhost(int worldIndex)
    {
        GhostSave save = new GhostSave();
        foreach (var item in DataManager.Instance.AllWorld[worldIndex].WorldData)
        {
            save.FantomeData.Add(item.GhostPlayer.Ghost);
        }             
        string data = JsonUtility.ToJson(save);
        string filepath = Application.persistentDataPath + "/";
        File.WriteAllText(filepath, data);

        UploadFile(worldIndex.ToString());
    }
    public void LoadAllFiles()
    {
        if (GlobalFileLock != 0)
            throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");

        GlobalFileLock += 1; // Start GetFiles
        var request = new PlayFab.DataModels.GetFilesRequest { Entity = new PlayFab.DataModels.EntityKey { Id = PlayFabLogin.Instance.EntityId, Type = PlayFabLogin.Instance.EntityType } };
        PlayFabDataAPI.GetFiles(request, OnGetFileMeta, OnSharedFailure);
    }

    void OnGetFileMeta(PlayFab.DataModels.GetFilesResponse result)
    {
        Debug.Log("Loading " + result.Metadata.Count + " files");

        _entityFileJson.Clear();
        foreach (var eachFilePair in result.Metadata)
        {
            _entityFileJson.Add(eachFilePair.Key, null);
            GetActualFile(eachFilePair.Value);
        }
        GlobalFileLock -= 1; // Finish GetFiles
    }


    void GetActualFile(PlayFab.DataModels.GetFileMetadata fileData)
    {
        GlobalFileLock += 1; // Start Each SimpleGetCall
        PlayFabHttp.SimpleGetCall(fileData.DownloadUrl,
            result => {

                GhostSave save = JsonUtility.FromJson<GhostSave>(Encoding.UTF8.GetString(result));
                for (int i = 0; i < save.FantomeData.Count; i++)
                {
                    bool breakLoops = false;
                    for (int j = 0; j < DataManager.Instance.AllWorld.Count; j++)
                    {
                        for (int k = 0; k < DataManager.Instance.AllWorld[j].WorldData.Count; k++)
                        {
                            if (save.FantomeData[i].LevelName == DataManager.Instance.AllWorld[j].WorldData[k].MapName)
                            {
                                GhostSO ghost = ScriptableObject.CreateInstance<GhostSO>();
                                ghost.Ghost = save.FantomeData[i];
                                DataManager.Instance.AllWorld[j].WorldData[k].GhostPlayer = ghost;
                                breakLoops = true;
                                break;
                            }
                        }
                        if (breakLoops)
                        {
                            break;
                        }
                    }
                }
                GlobalFileLock -= 1; 
            
            }, // Finish Each SimpleGetCall
            error => { Debug.Log(error); }
        );
    }

    public void UploadFile(string fileName)
    {
        if (GlobalFileLock != 0)
            throw new Exception("This example overly restricts file operations for safety. Careful consideration must be made when doing multiple file operations in parallel to avoid conflict.");

        ActiveUploadFileName = fileName;

        GlobalFileLock += 1; // Start InitiateFileUploads
        var request = new PlayFab.DataModels.InitiateFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = PlayFabLogin.Instance.EntityId, Type = PlayFabLogin.Instance.EntityType },
            FileNames = new List<string> { ActiveUploadFileName },
        };
        PlayFabDataAPI.InitiateFileUploads(request, OnInitFileUpload, OnInitFailed);
    }

    void OnInitFailed(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EntityFileOperationPending)
        {
            // This is an error you should handle when calling InitiateFileUploads, but your resolution path may vary
            GlobalFileLock += 1; // Start AbortFileUploads
            var request = new PlayFab.DataModels.AbortFileUploadsRequest
            {
                Entity = new PlayFab.DataModels.EntityKey { Id = PlayFabLogin.Instance.EntityId, Type = PlayFabLogin.Instance.EntityType },
                FileNames = new List<string> { ActiveUploadFileName },
            };
            PlayFabDataAPI.AbortFileUploads(request, (result) => { GlobalFileLock -= 1; UploadFile(ActiveUploadFileName); }, OnSharedFailure); GlobalFileLock -= 1; // Finish AbortFileUploads
            GlobalFileLock -= 1; // Failed InitiateFileUploads
        }
        else
            OnSharedFailure(error);
    }

    void OnInitFileUpload(PlayFab.DataModels.InitiateFileUploadsResponse response)
    {
        string worldsFolder = Application.persistentDataPath + "/"+ response.UploadDetails[0].FileName;
        string payloadStr = "{}";
        if (File.Exists(worldsFolder))
        {
            payloadStr = File.ReadAllText(worldsFolder);  
        }

        var payload = Encoding.UTF8.GetBytes(payloadStr);

        GlobalFileLock += 1; // Start SimplePutCall
        PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
            payload,
            FinalizeUpload,
            error => { Debug.Log(error); }
        );
        GlobalFileLock -= 1; // Finish InitiateFileUploads
    }

    void FinalizeUpload(byte[] data)
    {
        GlobalFileLock += 1; // Start FinalizeFileUploads
        var request = new PlayFab.DataModels.FinalizeFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = PlayFabLogin.Instance.EntityId, Type = PlayFabLogin.Instance.EntityType },
            FileNames = new List<string> { ActiveUploadFileName },
        };
        PlayFabDataAPI.FinalizeFileUploads(request, OnUploadSuccess, OnSharedFailure);
        GlobalFileLock -= 1; // Finish SimplePutCall
    }
    void OnUploadSuccess(PlayFab.DataModels.FinalizeFileUploadsResponse result)
    {
        Debug.Log("File upload success: " + ActiveUploadFileName);
        GlobalFileLock -= 1; // Finish FinalizeFileUploads

        File.Delete(Application.persistentDataPath + "/" + ActiveUploadFileName);
    }
}