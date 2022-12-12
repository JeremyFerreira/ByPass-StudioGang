
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        bool autoLauncj = true;
#if UNITY_EDITOR
        autoLauncj = false;


#endif
        if(autoLauncj)
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadScene()
    {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);
            while (!operation.isDone)
            {
                yield return null;
            } 
    }
}


