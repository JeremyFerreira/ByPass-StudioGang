using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputSO", menuName = "Input/InputSO", order = 6)]
public class InputSO : ScriptableObject
{
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;

    public event Action OnSlowTimePressed;
    public event Action OnSlowTimeReleased;

    public event Action OnGrapplePressed;
    public event Action OnGrappleReleased;

    public event Action OnRestartPressed;

    public event Action OnPausePressed;

    public event Action<Vector2> OnMoveChanged;

    public event Action<Vector2> OnLookChanged;

    public void PausePressed()
    {
        OnPausePressed?.Invoke();
    }
    public void RestartPressed()
    {
        OnRestartPressed?.Invoke();
    }
    public void GrappleReleased()
    {
        OnGrappleReleased?.Invoke();
    }
    public void GrapplePressed()
    {
        OnGrapplePressed?.Invoke();
    }
    public void SlowTimeReleased()
    {
        OnSlowTimeReleased?.Invoke();
    }
    public void SlowTimePressed()
    {
        OnSlowTimePressed?.Invoke();
    }
    public void JumpPressed()
    {
        OnJumpPressed?.Invoke();
    }

    public void JumpReleased()
    {
        OnJumpReleased?.Invoke();
    }

    public void OnMove(Vector2 value)
    {
        OnMoveChanged?.Invoke(value);
    }

    public void OnLook(Vector2 value)
    {
        OnLookChanged?.Invoke(value);
    }
}
