using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SOInputButton : MonoBehaviour
{
    [SerializeField] bool value;
    //public event Action OnPressed;

    public void OnButtonPressed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnPressed?.Invoke();
        }
        value = ctx.performed;
    }
}
