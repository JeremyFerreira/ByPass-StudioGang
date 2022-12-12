using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InMainMenu : MonoBehaviour
{
    [SerializeField] EventSO _eventInmainMenu;
    private void Start()
    {
        _eventInmainMenu.OnLaunchEvent?.Invoke();
        Time.timeScale = 1f;
    }
}
