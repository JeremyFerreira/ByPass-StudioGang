using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeHUD : MonoBehaviour
{
    [SerializeField] EventSO EventPause;

    public void OnClick()
    {
        EventPause.OnLaunchEvent?.Invoke();
    }
}
