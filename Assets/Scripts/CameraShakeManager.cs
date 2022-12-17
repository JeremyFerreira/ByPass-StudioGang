using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void Shake(ShakeData shakeData)
    {
        if(Settings.UseCameraShake)
        {
            CameraShakerHandler.Shake(shakeData);
        }
        
    }
}
