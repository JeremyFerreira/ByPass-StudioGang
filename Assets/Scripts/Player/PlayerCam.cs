using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine.UIElements;

public class PlayerCam : MonoBehaviour
{
    public static PlayerCam Instance;
    [SerializeField] float sensibility;

    [SerializeField] Transform orientation;
    [SerializeField] Transform camHolder;
    [SerializeField] Transform camParent;

    Vector2 looking;

    float xRotation;
    float yRotation;
    Camera cam;
    float fov;
    public float GetFov()
    {
        return fov;
    }
    public float fovDash;
    public float fovallRun;


    [SerializeField] bool IsGamePad;
    [SerializeField] InputSO _inputSO;


    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        _inputSO.OnLookChanged += SetLookInput;
    }
    private void OnDisable()
    {
        _inputSO.OnLookChanged -= SetLookInput;
    }
    private void Start()
    {
        cam = GetComponent<Camera>();
        fov = cam.fieldOfView;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        
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
        lookX = looking.x * sensibility * Time.deltaTime;
        lookY = looking.y * sensibility * Time.deltaTime;


        yRotation += lookX;
        xRotation -= lookY;
        

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoTilt(float zTilt)
    {
        camParent.transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.5f);
    }
    public void DoFov(float fov)
    {
        cam.DOFieldOfView(fov, 0.25f);
    }

}