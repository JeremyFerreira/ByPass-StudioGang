using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class PlayerCam : MonoBehaviour
{
    public static PlayerCam Instance;
    [SerializeField] float sensibility;

    [SerializeField] Transform orientation;
    [SerializeField] Transform camHolder;

    [SerializeField] SOInputVector lookInput;
    Vector2 looking;

    float xRotation;
    float yRotation;


    [SerializeField] bool IsGamePad;

    [SerializeField] EventSO startRun;
    [SerializeField] EventSO stopRun;

    private void EnableInput()
    {
        lookInput.OnValueChanged += SetLookInput;
    }
    private void DisableInput()
    {
        lookInput.OnValueChanged -= SetLookInput;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        startRun.OnLaunchEvent += EnableInput;
        stopRun.OnLaunchEvent += DisableInput;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void Update()
    {
        DoLooking();
    }
    private void SetLookInput(Vector2 look)
    {
        looking = look;
    }
    private void DoLooking()
    {
        //getMouse Inputs
        float lookX;
        float lookY;
        lookX = looking.x * sensibility * Time.unscaledDeltaTime;
        lookY = looking.y * sensibility * Time.unscaledDeltaTime;


        yRotation += lookX;
        xRotation -= lookY;
        

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.5f);
    }
}