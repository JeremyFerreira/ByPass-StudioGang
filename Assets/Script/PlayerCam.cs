using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class PlayerCam : MonoBehaviour
{
    public static PlayerCam Instance;
    [SerializeField] float sensibilityMouse;
    [SerializeField] float sensibilityGamePad;

    [SerializeField] Transform orientation;
    [SerializeField] Transform camHolder;

    float xRotation;
    float yRotation;


    [SerializeField] bool IsGamePad;
    static Input input;

    private void Awake()
    {
        Instance = this;
        input = new Input();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    private void Update()
    {
        DoLooking();
    }
    private void DoLooking()
    {
        //getMouse Inputs
        float lookX;
        float lookY;
        Vector2 looking = input.InGame.Look.ReadValue<Vector2>();

        if (IsGamePad == false)
        {
            lookX = looking.x * sensibilityMouse * Time.unscaledDeltaTime;
            lookY = looking.y * sensibilityMouse * Time.unscaledDeltaTime;
        }
        else
        {
            lookX = looking.x * sensibilityGamePad * Time.unscaledDeltaTime;
            lookY = looking.y * sensibilityGamePad * Time.unscaledDeltaTime;
        }


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