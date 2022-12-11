using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CountDownHUD : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    [SerializeField] TimeSO timeSO;
    [SerializeField] EventSO _OnStartLevelEvent;
    [SerializeField] EventSO _OnStartRunEvent;
    private void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        timeSO.OnValueChange += ChangeText;
        _OnStartLevelEvent.OnLaunchEvent += OnStartLevel;
        _OnStartRunEvent.OnLaunchEvent += OnRunEvent;
    }
    private void OnStartLevel()
    {
        textMeshProUGUI.enabled = true;
    }
    private void OnRunEvent()
    {
        textMeshProUGUI.enabled = false;
    }

    private void OnDisable()
    {
        timeSO.OnValueChange -= ChangeText;
        _OnStartLevelEvent.OnLaunchEvent -= OnStartLevel;
        _OnStartRunEvent.OnLaunchEvent -= OnRunEvent;
    }

    private void ChangeText(float value)
    {
        if (value != 0)
            textMeshProUGUI.text = value.ToString();
        else
            textMeshProUGUI.text = "";
    }
}
