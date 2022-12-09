using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //VARIABLES ////////////////////////////////////////////////////////////////////////////////

    public static PlayerController Instance;

    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] float speedMax;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float grapplinAcceleration;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationTimer;
    float accelerationTimeReset;
    private float resetWalkSpeed;
    [SerializeField] float wallrunSpeed;
    [SerializeField] float speedIncreaseMultiplier;
    [SerializeField] float groundDrag;


    [Header("Jumping")]
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
    bool hasDoubleJumped;

    public bool CanDoubleJump()
    {
        return canDoubleJump;
    }
    public void SetCanDoubleJump(bool a)
    {
        canDoubleJump = a;
    }

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask whatIsGround;
    bool grounded;
    bool behindGround;
    public bool IsGrounded() { return grounded; }
    [SerializeField] float timeToJump;
    private float resetTimeToJump;
    public bool canJump;

    [SerializeField] Transform orientation;
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
    Input input;
    bool isGrappling;
    CapsuleCollider collider;
    public void SetGrappin(bool a) { isGrappling = a; }


    // LOOPS AND FUNCTIONS///////////////////////////////////////////////////////////////////
    private void Awake()
    {
        Instance = this;
        stateGroundOld = true;
        input = new Input();

        collider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        input.Enable();
        input.InGame.Jump.performed += context => GetPlayerJump();
        input.InGame.Jump.canceled += context => PlayerJumpDown(false);
    }
    private void OnDisable()
    {
        input.Disable();
        input.InGame.Jump.performed += context => GetPlayerJump();
        input.InGame.Jump.canceled += context => PlayerJumpDown(false);
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
        resetMaxJumpTime = _maxJumpTime;
    }

    private void Update()
    {
        Debug.Log(state);
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, collider.height * 0.5f + Physics.defaultContactOffset * 2, whatIsGround);
        behindGround = Physics.Raycast(transform.position - (Vector3.up * 0.3f), moveDirection.normalized, playerHeight, whatIsGround) || Physics.Raycast(transform.position + (Vector3.up * 0.3f), moveDirection.normalized, playerHeight, whatIsGround);

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
        MyInput();
        StateHandler();
    }

    private void FixedUpdate()
    {
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
    private void MyInput()
    {
        horizontalInput = input.InGame.Move.ReadValue<Vector2>().x;
        verticalInput = input.InGame.Move.ReadValue<Vector2>().y;
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
    private void MovePlayer()
    {
        
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode.Force);
            //slow down player if no inputs
            if (moveDirection.magnitude == 0f)
            {
                rb.velocity = new Vector3(rb.velocity.x / 1.05f, rb.velocity.y, rb.velocity.z / 1.05f);
            }
        }
        // in grappin
        else if (isGrappling )
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
            if (moveDirection.magnitude == 0f)
            {
                rb.velocity = new Vector3(rb.velocity.x / 1.05f, rb.velocity.y, rb.velocity.z / 1.05f);
            }

        }
        // in air
        else
        {
            //slow down player if no inputs
            if (moveDirection.magnitude == 0f)
            {
                rb.velocity = new Vector3(rb.velocity.x / 1.05f, rb.velocity.y, rb.velocity.z / 1.05f);
            }
            else
            {
                rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
        }

        //limit velocity
        if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > speedMax)
        {
            rb.velocity = (new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * speedMax) + (Vector3.up * rb.velocity.y);
        }
    }
    private IEnumerator SmoothlyLerpMoveSpeed()
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
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed

        if (flatVel.magnitude > moveSpeed &&!isGrappling)
        {
             Vector3 limitedVel = flatVel.normalized * moveSpeed;
             rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        if(flatVel.magnitude > moveSpeed && isGrappling)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed *1.5f;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
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

            hasDoubleJumped = true;
            // reset y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //JumpForce
            rb.AddForce(this.transform.up * _jumpVelocityChange * 1f, ForceMode.VelocityChange);
            canJump = false;

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
}
