using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerHUD : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    [SerializeField] TimeSO timeSO;
    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        timeSO.OnValueChange += ChangeText;
    }
    private void OnDisable()
    {
        timeSO.OnValueChange -= ChangeText;
    }

    private void ChangeText(float value)
    {
        textMeshProUGUI.text = TimerFormat.FormatTime(value);
    }
}
