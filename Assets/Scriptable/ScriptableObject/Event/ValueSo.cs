using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New ValueData", menuName = "ValueData/New Value Data", order = 7)]
public class ValueSo : ScriptableObject
{
    [SerializeField] float value { get; set; }
    [SerializeField] bool boolValue{ get; set; }
    public delegate void OnValueChangeEvent(float value);
    public OnValueChangeEvent OnValueChange;
    public delegate void OnBoolValueChangeEvent(bool value);
    public OnBoolValueChangeEvent OnBoolValueChange;

    public void ChangeValue(float value)
    {
        this.value = value;
        OnValueChange?.Invoke(value);
    }
    public void ChangeBoolValue(bool value)
    {
        boolValue = value;
        OnBoolValueChange?.Invoke(value);
    }
}
