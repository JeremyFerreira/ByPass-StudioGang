using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cinematic : MonoBehaviour
{
    [SerializeField] GameObject cutSceneToPlay;
    [SerializeField] GameObject cameras;
    [SerializeField] GameObject timeLine;
    [SerializeField] GameObject cameraPlayer;


    public void PlayCinematic (bool active)
    {
        cameraPlayer.SetActive(!active);
        timeLine.SetActive(active);
        cameras.SetActive(active);      
        cutSceneToPlay.SetActive(active);
    }
}
