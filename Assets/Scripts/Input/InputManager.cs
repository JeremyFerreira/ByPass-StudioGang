using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;

public enum ControlDeviceType
{
    KeyboardAndMouse,
    Gamepad,
}
public class InputManager : MonoBehaviour
{

    public static Input _input { private set; get; }
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
    [SerializeField] EventSO _eventStartLevel;


    void Awake()
    {
        if (Instance == null)
        {
            _input = new Input();
            Instance = this;
            SensibilityMouseX = PlayerPrefs.GetFloat("SensibilityMouseX", 100f);
            SensibilityMouseY = PlayerPrefs.GetFloat("SensibilityMouseY", 100f);
            SensibilityGamePadX = PlayerPrefs.GetFloat("SensibilityGamePadX", 100f);
            SensibilityGamePadY = PlayerPrefs.GetFloat("SensibilityGamePadY", 100f);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void StartLevel()
    {
        _input.UI.Disable();
    }

    private void StartRun()
    {
        EnableGameInput();
    }

    private void OnEnable()
    {
        if (Instance == this)
        {

        _eventStartLevel.OnLaunchEvent += StartLevel;
        _eventStartRun.OnLaunchEvent += StartRun;
        _eventPause.OnLaunchEvent += Pause;
        _eventDead.OnLaunchEvent += DisableGameInput;
        _eventReachFinishLine.OnLaunchEvent += ReachFinishLine;
        _eventRestart.OnLaunchEvent += DisableGameInput;
        _eventResume.OnLaunchEvent += Resume;
        _input.Enable();
        }
    }

    private void Resume()
    {
        _input.UI.Disable();
        EnableGameInput();
    }

    private void Pause()
    {
        DisableGameInput();
        _input.UI.Enable();
    }

    private void ReachFinishLine()
    {
        DisableGameInput();
        _input.UI.Enable();
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
        _input.Disable();
        _eventStartLevel.OnLaunchEvent -= StartLevel;
        _eventStartRun.OnLaunchEvent -= StartRun;
        _eventPause.OnLaunchEvent -= Pause;
        _eventDead.OnLaunchEvent -= DisableGameInput;
        _eventReachFinishLine.OnLaunchEvent -= ReachFinishLine;
        _eventRestart.OnLaunchEvent -= DisableGameInput;
        _eventResume.OnLaunchEvent -= Resume;
        }

    }



    void EnableGameInput()
    {
        _input.InGame.Enable();

        _input.InGame.Look.performed += context => _inputSO.OnLook(_input.InGame.Look.ReadValue<Vector2>());
        _input.InGame.Look.canceled += context => _inputSO.OnLook(Vector2.zero);

        _input.InGame.Jump.performed += context => _inputSO.JumpPressed();
        _input.InGame.Jump.canceled += context => _inputSO.JumpReleased();

        _input.InGame.SlowTime.performed += context => _inputSO.SlowTimePressed();
        _input.InGame.SlowTime.canceled += context => _inputSO.SlowTimeReleased();

        _input.InGame.Grappling.performed += context => _inputSO.GrapplePressed();
        _input.InGame.Grappling.canceled += context => _inputSO.GrappleReleased();

        _input.InGame.RestartLevel.performed += context => _inputSO.RestartPressed();

        _input.InGame.Pause.performed += context => _inputSO.PausePressed();

            _input.InGame.Move.performed += context => _inputSO.OnMove(_input.InGame.Move.ReadValue<Vector2>());
            _input.InGame.Move.canceled += context => _inputSO.OnMove(Vector2.zero);
    }

    void DisableGameInput()
    {
        _input.InGame.Move.performed -= context => _inputSO.OnMove(_input.InGame.Move.ReadValue<Vector2>());
        _input.InGame.Look.performed -= context => _inputSO.OnLook(_input.InGame.Look.ReadValue<Vector2>());
        _input.InGame.Move.canceled -= context => _inputSO.OnMove(Vector2.zero);
        _input.InGame.Look.canceled -= context => _inputSO.OnLook(Vector2.zero);

        _input.InGame.Jump.performed -= context => _inputSO.JumpPressed();
        _input.InGame.Jump.canceled -= context => _inputSO.JumpReleased();

        _input.InGame.SlowTime.performed -= context => _inputSO.SlowTimePressed();
        _input.InGame.SlowTime.canceled -= context => _inputSO.SlowTimeReleased();

        _input.InGame.Grappling.performed -= context => _inputSO.GrapplePressed();
        _input.InGame.Grappling.canceled -= context => _inputSO.GrappleReleased();

        _input.InGame.RestartLevel.performed -= context => _inputSO.RestartPressed();

        _input.InGame.Pause.performed -= context => _inputSO.PausePressed();

        _input.InGame.Disable();
        Debug.Log("disable Input");
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
        InputAction action = _input.asset.FindAction(actionName);
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
        if (_input == null)
        {
            _input = new Input();
        }

        InputAction action = _input.asset.FindAction(actionName);
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
        if (_input == null)
            _input = new Input();

        InputAction action = _input.asset.FindAction(actionName);

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
        InputAction action = _input.asset.FindAction(actionName);

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
