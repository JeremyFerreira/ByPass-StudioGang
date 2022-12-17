using DG.Tweening;
using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //VARIABLES ////////////////////////////////////////////////////////////////////////////////

    public static PlayerController Instance;
    [Space(10)]
    [Header("Movement")]
    [Space(10)]
    private float moveSpeed;
    [SerializeField] float speedMax;
    [SerializeField] float verticalSpeedMax;
    public float SpeedMax()
    {
        return speedMax;
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    [SerializeField] float walkSpeed;
    public float WalkSpeed()
    {
        return walkSpeed;
    }
    [SerializeField] float grapplinAcceleration;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationTimer;
    float accelerationTimeReset;
    private float resetWalkSpeed;
    [SerializeField] float wallrunSpeed;
    [SerializeField] float speedIncreaseMultiplier;
    [SerializeField] float groundDrag;


    [Header("Jumping")]
    [Space(10)]
    [SerializeField] private float _jumpVelocityChange;
    [SerializeField] private float _jumpAcceleration;
    [SerializeField] private float _maxJumpTime;
    float resetMaxJumpTime;
    [SerializeField] private float earlyPressTime;
    float earlyPressTimeReset;
    [SerializeField] float behindGroundJumpingForce;

    [SerializeField] float airMultiplier;
    [SerializeField] float jumpCoolDown;
    [SerializeField] float jumpForceHoldInput;
    [SerializeField] bool isJumping;
    bool readyToJump;
    bool canDoubleJump;

    public bool CanDoubleJump()
    {
        return canDoubleJump;
    }
    public void SetCanDoubleJump(bool a)
    {
        if (a && !canDoubleJump)
        {
            audioGetDoubleJump.PlayAudioCue();
        }
        canDoubleJump = a;
        
    }

    [Header("Ground Check")]
    [Space(10)]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsMovableGround;
    bool grounded;
    bool behindGround;
    public bool IsGrounded() { return grounded; }
    [SerializeField] float timeToJump;
    private float resetTimeToJump;
    public bool canJump;

    [SerializeField] Transform orientation;

    [Header("Input")]
    [Space(10)]
    
    float horizontalInput;
    float verticalInput;
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }
    public float GetVerticalInput()
    {
        return verticalInput;
    }

    public static Vector3 moveDirection;

    Rigidbody rb;
    public Rigidbody GetRB()
    {
        return rb;
    }
   
    [SerializeField] PlayerCam playerCam;

    [SerializeField] MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        air,
        grappling,
    }

    public bool wallrunning;
    WallRunController wallRunController;
    GrapplingGun grapplingGun;
    bool exitingWall;
    float timeWallDoubleJump = 0.8f;
    float resetWallTimeDoubleJump = 0.8f;

    public float deltaTime;
    public float timeToPress;

    bool stateGroundOld;
    public bool isGrappling;
    CapsuleCollider col;
    private bool dash;
    [SerializeField] float dashSpeed;
    [SerializeField] float timeToDash;
    float timeTodashReset;
    [SerializeField] SlowTime SlowTime;

    public void SetGrappin(bool a) { isGrappling = a; }

    bool hasJustLanded;
    bool hasJustLandedTemp;
    float playerYposLastFrame;

    [SerializeField] InputSO _inputSO;

    [Header("Audio")]
    [Space(10)]
    //audio
    [SerializeField] AudioComponent audioJump;
    [SerializeField] AudioComponent audioDoubleJump;
    [SerializeField] AudioComponent audioGetDoubleJump;
    [SerializeField] AudioComponent audioLanded;
    [SerializeField] AudioSource audioSourceWind;
    [SerializeField] AnimationCurve windPitch;
    float timeToUpdatePitch;

    [Header("CameraShake")]
    [Space(10)]
    [SerializeField] ShakeData dashShake;
    [SerializeField] ShakeData jumpShake;
    [SerializeField] ShakeData doubleJumpShake;
    [SerializeField] ShakeData landedShake;

    [SerializeField] bool canMove = false;
    [SerializeField] EventSO _eventStartRun;
    [SerializeField] EventSO _eventResumeRun;
    [SerializeField] EventSO _eventPauseRun;

    [SerializeField] Animator grappinAnim;


    public GameObject groundObject;
    [SerializeField] LayerMask ralentiGround;
    SlowTimeMaterial slowTimeMaterial;
    private void CanMove()
    {
        canMove = true;
    }
    private void CantMove()
    {
        canMove = false;
    }
    private void OnEnable()
    {
        _eventStartRun.OnLaunchEvent += CanMove;
        _eventResumeRun.OnLaunchEvent += CanMove;
        _eventPauseRun.OnLaunchEvent += CantMove;
        _inputSO.OnJumpPressed += GetPlayerJump;
        _inputSO.OnJumpReleased += CancelJump;
        _inputSO.OnMoveChanged += MoveDirection;
    }

    private void OnDisable()
    {
        _eventStartRun.OnLaunchEvent -= CanMove;
        _eventResumeRun.OnLaunchEvent -= CanMove;
        _eventPauseRun.OnLaunchEvent -= CantMove;
        _inputSO.OnJumpPressed -= GetPlayerJump;
        _inputSO.OnJumpReleased -= CancelJump;
        _inputSO.OnMoveChanged -= MoveDirection;
    }
    // LOOPS AND FUNCTIONS///////////////////////////////////////////////////////////////////
    private void Awake()
    {
        
        stateGroundOld = true;

        col = GetComponent<CapsuleCollider>();
        Instance = this;
    }
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        wallRunController = GetComponent<WallRunController>();

        //initializing resets
        resetTimeToJump = timeToJump;
        resetWalkSpeed = walkSpeed;
        accelerationTimeReset = accelerationTimer;
        earlyPressTimeReset = earlyPressTime;
        earlyPressTime = 0;
        timeTodashReset = timeToDash;
        resetMaxJumpTime = _maxJumpTime;

        //audio
        audioSourceWind.Play();
    }

    private void Update()
    {
        
        
        //time To Jump if not on ground;
        if (grounded)
        {
            stateGroundOld = grounded;
            timeToJump = resetTimeToJump;
            canJump = true;
            rb.useGravity = false;
        }
        else
        {
            stateGroundOld = grounded;
            rb.useGravity = true;
            timeToJump -= Time.deltaTime;
            if (timeToJump <= 0)
            {
                canJump = false;
            }
        }


        if (grounded)
        {
            canDoubleJump = true;
        }
        StateHandler();

        if(dash)
        {
            timeToDash -= Time.deltaTime;
            if (timeToDash <= 0)
            {
                dash = false;
                timeToDash = timeTodashReset;
                playerCam.DoFov(playerCam.GetFov());
            }
        }

        //audio
        audioSourceWind.pitch = windPitch.Evaluate(Mathf.Sqrt((new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude / (float)speedMax))*Time.timeScale);
    }

    private void FixedUpdate()
    {
        // ground check

        grounded = Physics.Raycast(transform.position, Vector3.down, col.height * 0.5f + Physics.defaultContactOffset * 3, whatIsGround);
        behindGround = Physics.Raycast(transform.position - (Vector3.up * 0.3f), moveDirection.normalized, playerHeight, whatIsGround) || Physics.Raycast(transform.position + (Vector3.up * 0.3f), moveDirection.normalized, playerHeight, whatIsGround);
        
        RaycastHit hit2;
        if(Physics.Raycast(transform.position, Vector3.down,out hit2, col.height * 0.5f + Physics.defaultContactOffset * 3, whatIsMovableGround))
        {
            col.gameObject.transform.SetParent(hit2.transform, true);
        }
        else
        {
            transform.parent = null;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down,out hit, col.height * 0.5f +1))
        {
            if (hit.collider.TryGetComponent<SlowTimeMaterial>(out slowTimeMaterial))
            {
                groundObject = hit.collider.gameObject;
                slowTimeMaterial = groundObject.GetComponent<SlowTimeMaterial>();
                slowTimeMaterial.isUsing = true;
                Debug.Log("a");
            }
            
        }
        else
        {
            if (groundObject!= null && !slowTimeMaterial.slowTime && GrapplingGun.instance.grappinObject != groundObject)
            {
                slowTimeMaterial.isUsing = false;
                slowTimeMaterial.MatRealime();
            }
            groundObject = null;
        }

        hasJustLanded = grounded && playerYposLastFrame > transform.position.y + 0.05f;
        playerYposLastFrame = transform.position.y;
        if (hasJustLanded && hasJustLandedTemp)
        {
            CameraShakeManager.instance.Shake(landedShake);
            audioLanded.PlayAudioCue();
            hasJustLanded = false;
        }
        if(!grounded)
        {
            hasJustLandedTemp = true;
        }
        
        MovePlayer();
        SpeedControl();
        Accelerate();
        LongJump();
        EarlyPressJump();
        BehindGroundJump();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
            rb.drag = 0;

       

    }

    //Input////////////////////////////////////////////////////////////////
    private void MyInput(Vector2 direction)
    {
        horizontalInput = direction.x;
        verticalInput = direction.y;
    }

    //STATEMACHINE////////////////////////////////////////////////////////////
    private void StateHandler()
    {
        // Mode - Wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //mode -grappling
        else if(isGrappling)
        {
            state = MovementState.grappling;
            desiredMoveSpeed = walkSpeed;//////////////////////////////////////
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desired move speed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    //MOVEMENT////////////////////////////////////////////////////////////////
    public void MoveDirection(Vector2 dir)
    {
        // calculate movement direction
        verticalInput = dir.y;
        horizontalInput = dir.x;
    }
    private void MovePlayer()
    {
        if(canMove)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
        
        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
            //slow down player if no inputs
            if (moveDirection.magnitude == 0f && !dash)
            {
                rb.velocity = new Vector3(rb.velocity.x / 1.05f, rb.velocity.y, rb.velocity.z / 1.05f);
            }
        }
        // in grappin
        else if (isGrappling && !dash)
        {
            if (rb.velocity.y<-1)
            {

                rb.AddForce(moveDirection * (moveSpeed + Mathf.Abs(rb.velocity.y)));
            }
            else
            {
                rb.AddForce(moveDirection * (moveSpeed + acceleration)*10f, ForceMode.Acceleration);
            }

            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
            //slow down player if no inputs
            if (moveDirection.magnitude == 0f && !dash)
            {
                rb.velocity = new Vector3(rb.velocity.x / 1.05f, rb.velocity.y, rb.velocity.z / 1.05f);
            }
            

        }
        // in air
        else
        {
            //slow down player if no inputs
            if (moveDirection.magnitude == 0f && !dash)
            {
                rb.velocity = new Vector3(rb.velocity.x / 1.05f, rb.velocity.y, rb.velocity.z / 1.05f);
            }
            else
            {
                rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
            if (Mathf.Abs(rb.velocity.y) > verticalSpeedMax)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y > 0 ? verticalSpeedMax : -verticalSpeedMax, rb.velocity.z);
            }
        }

        //limit velocity
        if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > speedMax && !dash)
        {
            rb.velocity = (new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * speedMax) + (Vector3.up * rb.velocity.y);
        }

        
    }
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        if(!dash)
        {
            // smoothly lerp movementSpeed to desired value
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
            float startValue = moveSpeed;

            while (time < difference)
            {
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
                time += Time.deltaTime * speedIncreaseMultiplier;

                yield return null;
            }
            moveSpeed = desiredMoveSpeed;
        }
        
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(!dash)
        {
            if (flatVel.magnitude > moveSpeed && !isGrappling)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
            if (flatVel.magnitude > moveSpeed && isGrappling)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed * 1.5f;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
    private void Accelerate()
    {
        if (new Vector2(verticalInput, horizontalInput).magnitude >= 0.2f && verticalInput > -0.2f)
        {
            accelerationTimer -= Time.deltaTime;
            if (accelerationTimer < 0 && walkSpeed < speedMax)
            {
                walkSpeed += acceleration * Time.deltaTime;

                accelerationTimer = accelerationTimeReset;
            }

        }
        if ((new Vector2(verticalInput, horizontalInput).magnitude <= 0.2f|| rb.velocity.magnitude<resetWalkSpeed) && walkSpeed > resetWalkSpeed)
        {
            walkSpeed -= resetWalkSpeed/2 * Time.deltaTime;
            accelerationTimer = accelerationTimeReset;
        }
    }

    //JUMP///////////////////////////////////////////////////////////////////
    #region Jump
    public void GetPlayerJump()
    {
        // when to jump
        if (readyToJump && (grounded || canJump) && !wallrunning)
        {
            readyToJump = false;

            Jump();

            //resets the possibility to jump to true after jumpCoolDown Time
            Invoke(nameof(ResetJump), jumpCoolDown);
        }
        else if (readyToJump && canDoubleJump && !wallrunning)
        {
            DoubleJump();
            canDoubleJump = false;
        }
        else
        {
            earlyPressTime = earlyPressTimeReset;
        }
    }
    public void CancelJump()
    {
        isJumping = false;
    }
    public void PlayerJumpDown(bool a)
    {
        isJumping = a;
    }
    public void Jump()
    {
            isJumping = true;
            _maxJumpTime = resetMaxJumpTime;

            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //JumpForce
            rb.AddForce(Vector3.up * _jumpVelocityChange, ForceMode.VelocityChange);
            canJump = false;
            canDoubleJump = true;

            audioJump.PlayAudioCue();
            CameraShakeManager.instance.Shake(jumpShake);
            //Rumbler.instance.RumbleConstant(0.1f, 1f, 0.3f);
    }
    public void DoubleJump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 1.2f, whatIsGround))
        {
            Jump();
        }
        else
        {
            isJumping = true;
            _maxJumpTime = resetMaxJumpTime;

            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //JumpForce
            rb.AddForce(this.transform.up * _jumpVelocityChange * 1f, ForceMode.VelocityChange);
            
            canJump = false;


            audioDoubleJump.PlayAudioCue();
            CameraShakeManager.instance.Shake(doubleJumpShake);
            if(!isGrappling)
            {
                grappinAnim.CrossFade("DoubleJump", 0, 0);
            }
            
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    public void LongJump()
    {
        _maxJumpTime -= Time.deltaTime;

        if (isJumping && (_maxJumpTime > 0))
        {
            rb.AddForce(Vector3.up * _jumpAcceleration, ForceMode.Acceleration);
        }

    }
    public void EarlyPressJump()
    {
        if(earlyPressTime > 0)
        {
            earlyPressTime -= Time.deltaTime;
            if(grounded && canJump)
            {
                GetPlayerJump();
                earlyPressTime = earlyPressTimeReset;
            }
        }
    }

    public void WallRunDoubleJump()
    {
        if (wallRunController.GetExitingWall())
        {
            exitingWall = true;
        }
        if (exitingWall)
        {
            timeWallDoubleJump -= Time.deltaTime;
            if (timeWallDoubleJump <= 0)
            {
                canDoubleJump = true;
                exitingWall = false;
                timeWallDoubleJump = resetWallTimeDoubleJump;
            }
        }
    }
    public void BehindGroundJump()
    {
        if (behindGround && isJumping)
        {
            rb.AddForce(behindGroundJumpingForce * Vector3.up);
        }
    }
    #endregion
    public void Dash()
    {
         rb.velocity = orientation.forward * dashSpeed;
         dash = true;
         CameraShakeManager.instance.Shake(dashShake);
         playerCam.DoFov(playerCam.fovDash);
    }
}
