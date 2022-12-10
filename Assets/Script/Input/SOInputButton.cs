using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputData", menuName = "Button")]
public class SOInputButton : MonoBehaviour
{
    [SerializeField] bool value;
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
        value = ctx.performed;
    }
}
