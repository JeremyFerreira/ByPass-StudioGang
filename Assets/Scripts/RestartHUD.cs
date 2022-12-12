using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartHUD : MonoBehaviour
{
    [SerializeField] EventSO restartEvent;

    public void OnClick()
    {
        restartEvent.OnLaunchEvent.Invoke();
    }
}
