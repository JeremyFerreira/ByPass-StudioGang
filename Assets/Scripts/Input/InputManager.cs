using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using DG.Tweening.Core.Easing;

public enum ControlDeviceType
{
    KeyboardAndMouse,
    Gamepad,
}
public class InputManager : MonoBehaviour
{

    public static Input _Input { private set; get; }
    public static InputManager Instance;

    [SerializeField] PlayerInput playerInput;
    public static ControlDeviceType currentControlDevice;

    public static float SensibilityMouseY;
    public static float SensibilityMouseX;
    public static float SensibilityGamePadY;
    public static float SensibilityGamePadX;

    public static event Action rebindComplete;
    public static event Action rebindCanceled;
    public static event Action<InputAction, int> rebindStarted;

    [SerializeField] List<InputActionReference> allBinding;

    [SerializeField] InputSO _inputSO;

    [SerializeField] EventSO _eventStartRun;
    [SerializeField] EventSO _eventPause;
    [SerializeField] EventSO _eventDead;
    [SerializeField] EventSO _eventReachFinishLine;
    [SerializeField] EventSO _eventRestart;
    [SerializeField] EventSO _eventResume;

    private void Awake()
    {
        _Input = new Input();
        Instance = this;
        SensibilityMouseX = PlayerPrefs.GetFloat("SensibilityMouseX", 100f);
        SensibilityMouseY = PlayerPrefs.GetFloat("SensibilityMouseY", 100f);
        SensibilityGamePadX = PlayerPrefs.GetFloat("SensibilityGamePadX", 100f);
        SensibilityGamePadY = PlayerPrefs.GetFloat("SensibilityGamePadY", 100f);
    }


    private void OnEnable()
    {
        _eventStartRun.OnLaunchEvent += EnableGameInput;
        _eventPause.OnLaunchEvent += Pause;
        _eventDead.OnLaunchEvent += DisableGameInput;
        _eventReachFinishLine.OnLaunchEvent += ReachFinishLine;
        _eventRestart.OnLaunchEvent += DisableGameInput;
        _eventResume.OnLaunchEvent += Resume;
        _Input.Enable();
    }

    private void Resume()
    {
        _Input.UI.Disable();
        EnableGameInput();
    }

    private void Pause()
    {
        DisableGameInput();
        _Input.UI.Enable();
    }

    private void ReachFinishLine()
    {
        DisableGameInput();
        _Input.UI.Enable();
    }

    private void OnDisable()
    {
        _Input.Disable();
        _eventStartRun.OnLaunchEvent -= EnableGameInput;
        _eventPause.OnLaunchEvent -= Pause;
        _eventDead.OnLaunchEvent -= DisableGameInput;
        _eventReachFinishLine.OnLaunchEvent -= DisableGameInput;
        _eventRestart.OnLaunchEvent -= DisableGameInput;
        _eventResume.OnLaunchEvent -= EnableGameInput;

    }



    void EnableGameInput()
    {
        _Input.InGame.Move.performed += context => _inputSO.OnMove(_Input.InGame.Move.ReadValue<Vector2>());
        _Input.InGame.Look.performed += context => _inputSO.OnLook(_Input.InGame.Look.ReadValue<Vector2>());
        _Input.InGame.Move.canceled += context => _inputSO.OnMove(Vector2.zero);
        _Input.InGame.Look.canceled += context => _inputSO.OnLook(Vector2.zero);

        _Input.InGame.Jump.performed += context => _inputSO.JumpPressed();
        _Input.InGame.Jump.canceled += context => _inputSO.JumpReleased();

        _Input.InGame.SlowTime.performed += context => _inputSO.SlowTimePressed();
        _Input.InGame.SlowTime.canceled += context => _inputSO.SlowTimeReleased();

        _Input.InGame.Grappling.performed += context => _inputSO.GrapplePressed();
        _Input.InGame.Grappling.canceled += context => _inputSO.GrappleReleased();

        _Input.InGame.RestartLevel.performed += context => _inputSO.RestartPressed();

        _Input.InGame.Pause.performed += context => _inputSO.PausePressed();
        _Input.InGame.Enable();
    }

    void DisableGameInput()
    {
        _Input.InGame.Disable();
        _Input.InGame.Move.performed -= context => _inputSO.OnMove(_Input.InGame.Move.ReadValue<Vector2>());
        _Input.InGame.Look.performed -= context => _inputSO.OnLook(_Input.InGame.Look.ReadValue<Vector2>());
        _Input.InGame.Move.canceled -= context => _inputSO.OnMove(Vector2.zero);
        _Input.InGame.Look.canceled -= context => _inputSO.OnLook(Vector2.zero);

        _Input.InGame.Jump.started -= context => _inputSO.JumpPressed();
        _Input.InGame.Jump.canceled -= context => _inputSO.JumpReleased();

        _Input.InGame.SlowTime.started -= context => _inputSO.SlowTimePressed();
        _Input.InGame.SlowTime.canceled -= context => _inputSO.SlowTimeReleased();

        _Input.InGame.Grappling.started -= context => _inputSO.GrapplePressed();
        _Input.InGame.Grappling.canceled -= context => _inputSO.GrappleReleased();

        _Input.InGame.RestartLevel.started -= context => _inputSO.RestartPressed();

        _Input.InGame.Pause.started -= context => _inputSO.PausePressed();
    }

    private void OnControlsChanged(UnityEngine.InputSystem.PlayerInput obj)
    {
        if (obj.currentControlScheme == "Gamepad")
        {
            if (currentControlDevice != ControlDeviceType.Gamepad)
            {
                
            }
        }
        else
        {
            if (currentControlDevice != ControlDeviceType.KeyboardAndMouse)
            {
                
            }
        }
    }


    public void SetSensibilityGamePad(float y)
    {
        PlayerPrefs.SetFloat("SensibilityGamePadY", y);
        SensibilityGamePadY = y;
        PlayerPrefs.SetFloat("SensibilityGamePadX", y);
        SensibilityGamePadX = y;
    }

    public void SetSensibilityMouse(float y)
    {
        PlayerPrefs.SetFloat("SensibilityMouseY", y);
        SensibilityMouseY = y;
        PlayerPrefs.SetFloat("SensibilityMouseX", y);
        SensibilityMouseX = y;
    }

    public static void StartRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse)
    {
        InputAction action = _Input.asset.FindAction(actionName);
        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couln't find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
            {
                DoRebind(action, bindingIndex, statusText, true, excludeMouse);
            }
        }

        else
            DoRebind(action, bindingIndex, statusText, false, excludeMouse);
    }

    private static void DoRebind(InputAction actionToRebind, int bindingIndex, TextMeshProUGUI statusText, bool allCompositeParts, bool excludeMouse)
    {
        Debug.Log("Binging");
        if (actionToRebind == null || bindingIndex < 0)
            return;

        statusText.text = $"Press a {actionToRebind.expectedControlType}";
        actionToRebind.Disable();
        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);
        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if (allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
            }
            SaveBindingOverride(actionToRebind);
            rebindComplete?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            rebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/escape");

        if (excludeMouse)
            rebind.WithControlsExcluding("Mouse");

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start();
    }


    public static string GetBindingName(string actionName, int bindingIndex)
    {
        if (_Input == null)
        {
            _Input = new Input();
        }

        InputAction action = _Input.asset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
    }

    private static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        if (_Input == null)
            _Input = new Input();

        InputAction action = _Input.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i, action.bindings[i].overridePath)))
            {
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i, action.bindings[i].overridePath));
            }
        }
    }

    public static void ResetBinding(string actionName, int bindingIndex)
    {
        InputAction action = _Input.asset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex)
        {
            print("Coulnd not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
            {
                action.RemoveBindingOverride(i);
            }
        }

        else
            action.RemoveBindingOverride(bindingIndex);



        SaveBindingOverride(action);
    }
}
