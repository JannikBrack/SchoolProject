using UnityEngine;

// Contains the command the user wishes upon the character
struct Cmd
{
    public float forwardMove;
    public float rightMove;
    public float upMove;
}

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    //Camera Settings
    [SerializeField] Transform playerView;
    [SerializeField] float playerViewYOffset = 0.6f;
    [SerializeField] float xMouseSensitivity = 30.0f;
    [SerializeField] float yMouseSensitivity = 30.0f;

    // Camera rotations
    private float rotX = 0.0f;
    private float rotY = 0.0f;
    private Vector3 moveDirectionNorm = Vector3.zero;
    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 weaponOrigen;
    private float playerTopVelocity = 0.0f;

    // Gravity & Friction
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float friction = 6;

    //Movement Vatiables
    [Header("Movement Vatiables")]
    [SerializeField] private float moveSpeed = 7.0f;                
    [SerializeField] private float runAcceleration = 14.0f;         
    [SerializeField] private float runDeacceleration = 10.0f;       
    [SerializeField] private float airAcceleration = 2.0f;          
    [SerializeField] private float airDecceleration = 2.0f;         
    [SerializeField] private float airControl = 0.3f;               
    [SerializeField] private float sideStrafeAcceleration = 50.0f;  
    [SerializeField] private float sideStrafeSpeed = 1.0f;          
    [SerializeField] private float jumpSpeed = 8.0f;                
    [SerializeField] private bool holdJumpToBhop = false;

    //Character Controller
    private CharacterController _controller;

    //Player can queue the next jump just before he hits the ground
    private bool wishJump = false;

    //Used to display real time fricton
    private float playerFriction = 0.0f;

    //Player commands, stores commands (Forward, back, jump, etc)
    private Cmd _cmd;

    //Manages the inventory
    [Header("Inventory")]
    public InvOpenClose InvOpenClose;
    [SerializeField] GameObject Inventory;

    //Important variables for walljumping
    [Header("WallJump")]
    [SerializeField] private float wallDistance = 0.5f;
    [SerializeField] private bool wallLeft = false;
    [SerializeField] private bool wallRight = false;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private Transform orientation;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    //Healthamount for Fast Chicken;
    [Header("PlayerHealth")]
    [SerializeField] PlayerHealth playerHealth;

    //Dash
    private float dashCooldownTime;
    private float dashTime;
    [SerializeField] private bool canDash;
    [SerializeField] private bool dashing;

    //Variables Managing the Player like Skills or if the Player is dead...
    private bool deadPlayer;
    private bool fastChicken;
    private bool sideStep;
    #endregion

    #region Start&Update

    //Load Presettings
    private void Start()
    {
        LoadBooleans();
        ResetCooldown();

        //Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                playerView = mainCamera.gameObject.transform;
        }

        //Put the camera inside the capsule collider
        playerView.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);

        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        LoadBooleans();

        if (!deadPlayer)
        {
            wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
            wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);


            /* Ensure that the cursor is locked into the screen */
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                if (!Inventory.activeInHierarchy)
                {
                    if (Input.GetButtonDown("Fire1"))
                        Cursor.lockState = CursorLockMode.Locked;
                }

            }

            if (!InvOpenClose.InvOpen)
            {
                rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
                rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;

                //Clamp the X rotation
                if (rotX < -90)
                    rotX = -90;
                else if (rotX > 90)
                    rotX = 90;
            }

            if ((playerHealth.GetHealth() <= (playerHealth.GetMaxHealth() * 0.1f)) && fastChicken) moveSpeed = 10.5f;
            else moveSpeed = 7;


            this.transform.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
            playerView.rotation = Quaternion.Euler(rotX, rotY, 0); // Rotates the camera

            /* Movement, here's the important part */
            QueueJump();
            if(Input.GetKey(KeyCode.F) && sideStep && canDash)
            {
                dashing = true;
            }
            Dash();
            if (_controller.isGrounded)
                GroundMove();
            else if (!_controller.isGrounded && wallLeft && Input.GetKey(KeyCode.Space))
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                playerVelocity = wallRunJumpDirection * wallJumpForce;
            }
            else if (!_controller.isGrounded && wallRight && Input.GetKey(KeyCode.Space))
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                playerVelocity = wallRunJumpDirection * wallJumpForce;
            }
            else if (!_controller.isGrounded)
                AirMove();

            //Move the controller
            _controller.Move(playerVelocity * Time.deltaTime);

            /* Calculate top velocity */
            Vector3 udp = playerVelocity;
            udp.y = 0.0f;
            if (udp.magnitude > playerTopVelocity)
                playerTopVelocity = udp.magnitude;

            //Need to move the camera after the player has been moved because otherwise the camera will clip the player if going fast enough and will always be 1 frame behind.
            //Set the camera's position to the transform
            playerView.position = new Vector3(
                transform.position.x,
                transform.position.y + playerViewYOffset,
                transform.position.z);
        }
        
    }
    #endregion

    #region Movement

    /**
     * Sets the movement direction based on player input
     */
    private void SetMovementDir()
    {
        _cmd.forwardMove = Input.GetAxisRaw("Vertical");
        _cmd.rightMove   = Input.GetAxisRaw("Horizontal");
    }

    /**
     * Queues the next jump 
     */
    private void QueueJump()
    {
        if(holdJumpToBhop)
        {
            wishJump = Input.GetButton("Jump");
            return;
        }

        if(Input.GetButtonDown("Jump") && !wishJump)
            wishJump = true;
        if(Input.GetButtonUp("Jump"))
            wishJump = false;
    }

    /**
     * Execs when the player is in the air
    */
    private void AirMove()
    {
        Vector3 wishdir;
        float wishvel = airAcceleration;
        float accel;
        
        SetMovementDir();

        wishdir =  new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        wishdir.Normalize();
        moveDirectionNorm = wishdir;

        //CPM: Aircontrol
        float wishspeed2 = wishspeed;
        if (Vector3.Dot(playerVelocity, wishdir) < 0)
            accel = airDecceleration;
        else
            accel = airAcceleration;
        //If the player is ONLY strafing left or right
        if(_cmd.forwardMove == 0 && _cmd.rightMove != 0)
        {
            if(wishspeed > sideStrafeSpeed)
                wishspeed = sideStrafeSpeed;
            accel = sideStrafeAcceleration;
        }

        Accelerate(wishdir, wishspeed, accel);
        if(airControl > 0)
            AirControl(wishdir, wishspeed2);
        //!CPM: Aircontrol

        //Apply gravity
        playerVelocity.y -= gravity * Time.deltaTime;
    }



    /* Air control occurs when the player is in the air, it allows
     * players to move side to side much faster rather than being
     * 'sluggish' when it comes to cornering.
     */
    private void AirControl(Vector3 wishdir, float wishspeed)
    {
        float zspeed;
        float speed;
        float dot;
        float k;

        //Can't control movement if not moving forward or backward
        if(Mathf.Abs(_cmd.forwardMove) < 0.001 || Mathf.Abs(wishspeed) < 0.001)
            return;
        zspeed = playerVelocity.y;
        playerVelocity.y = 0;
        //Next two lines are equivalent to idTech's VectorNormalize()
        speed = playerVelocity.magnitude;
        playerVelocity.Normalize();

        dot = Vector3.Dot(playerVelocity, wishdir);
        k = 32;
        k *= airControl * dot * dot * Time.deltaTime;

        //Change direction while slowing down
        if (dot > 0)
        {
            playerVelocity.x = playerVelocity.x * speed + wishdir.x * k;
            playerVelocity.y = playerVelocity.y * speed + wishdir.y * k;
            playerVelocity.z = playerVelocity.z * speed + wishdir.z * k;

            playerVelocity.Normalize();
            moveDirectionNorm = playerVelocity;
        }

        playerVelocity.x *= speed;
        playerVelocity.y = zspeed;
        playerVelocity.z *= speed;
    }

    //Called every frame when the engine detects that the player is on the ground
    private void GroundMove()
    {
        Vector3 wishdir;

        // Do not apply friction if the player is queueing up the next jump
        if (!wishJump)
            ApplyFriction(1.0f);
        else
            ApplyFriction(0);

        SetMovementDir();


        wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();
        moveDirectionNorm = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        Accelerate(wishdir, wishspeed, runAcceleration);

        // Reset the gravity velocity
        playerVelocity.y = -gravity * Time.deltaTime;

        if(wishJump)
        {
            playerVelocity.y = jumpSpeed;
            wishJump = false;
        }
    }

    //Makes Player Dash in -180 Degrees
    private void Dash()
    {
        if (dashCooldownTime > 0)
        {
            canDash = false;
            dashCooldownTime -= Time.deltaTime;
        }
        else
        {
            canDash = true;
            if (dashing)
            {
                if (dashTime > 0)
                {
                    SetMovementDir();
                    if (!(_cmd.forwardMove == 1))
                    {
                        moveSpeed = 20f;
                    }

                    dashTime -= Time.deltaTime;
                }
                else
                {
                    moveSpeed = 7;
                    ResetCooldown();
                }
            }
        }
    }
    private void ResetCooldown()
    {
        dashCooldownTime = 3f;
        dashTime = 0.5f;
        canDash = true;
        dashing = false;
    }

    //Load PlayerManager booleans
    private void LoadBooleans()
    {
        deadPlayer = PlayerManager.instance.deadPlayer;
        fastChicken = PlayerManager.instance.fastChicken;
        sideStep = PlayerManager.instance.sideStep;
    }
    #endregion

    #region Applyfriction


    //Applies friction to the player, called in both the air and on the ground
    private void ApplyFriction(float t)
    {
        Vector3 vec = playerVelocity;
        float speed;
        float newspeed;
        float control;
        float drop;

        vec.y = 0.0f;
        speed = vec.magnitude;
        drop = 0.0f;

        //Only if the player is on the ground then apply friction
        if(_controller.isGrounded)
        {
            control = speed < runDeacceleration ? runDeacceleration : speed;
            drop = control * friction * Time.deltaTime * t;
        }

        newspeed = speed - drop;
        playerFriction = newspeed;
        if(newspeed < 0)
            newspeed = 0;
        if(speed > 0)
            newspeed /= speed;

        playerVelocity.x *= newspeed;
        playerVelocity.z *= newspeed;
    }
    #endregion

    #region Accelerate
    //Calculate the Speed as Acceleration
    private void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addspeed;
        float accelspeed;
        float currentspeed;

        currentspeed = Vector3.Dot(playerVelocity, wishdir);
        addspeed = wishspeed - currentspeed;
        if(addspeed <= 0)
            return;
        accelspeed = accel * Time.deltaTime * wishspeed;
        if(accelspeed > addspeed)
            accelspeed = addspeed;

        playerVelocity.x += accelspeed * wishdir.x;
        playerVelocity.z += accelspeed * wishdir.z;
    }
    #endregion
}
