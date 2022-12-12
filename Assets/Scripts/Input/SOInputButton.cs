using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Button", menuName = "InputData/New Button", order = 6)]
public class SOInputButton : ScriptableObject
{
    [SerializeField] bool value;
    public void SetValue(bool a)
        { value = a; }
    public event Action OnPressed;
    public event Action OnReleased;


    public void OnButtonPressed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnPressed?.Invoke();
        }
        value = ctx.performed;
    }
    public void OnButtonReleased(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            OnReleased?.Invoke();
        }
        value = ctx.canceled;
    }
}
