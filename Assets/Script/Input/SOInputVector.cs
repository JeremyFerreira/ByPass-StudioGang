using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "vector", menuName = "InputData/new Vector", order = 6)]
public class SOInputVector : ScriptableObject
{
    [SerializeField] Vector2 value;
    public event Action<Vector2> OnValueChanged;

    public void OnJoystickMoved(InputAction.CallbackContext ctx)
    {
         OnValueChanged?.Invoke(ctx.ReadValue<Vector2>());
         value = ctx.ReadValue<Vector2>();
    }
}
