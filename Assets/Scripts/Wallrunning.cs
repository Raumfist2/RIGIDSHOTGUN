using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrunning : MonoBehaviour
{
    [Header("WallRunning")]
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovement pm;
    private LedgeGrabbing lg;
    private Rigidbody rb;

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        lg = GetComponent<LedgeGrabbing>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }
    
    private void StateMachine()
    {
        //get inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        //Wallrunning
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            //Start wallrun
            if(!pm.wallrunning)
            StartWallRun();

            //Start wallruntimer
            if(wallRunTimer > 0)
            wallRunTimer -= Time.deltaTime;

            //Wallruntimer Ended
            if(wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            //WallJumping
            if(Input.GetKey(jumpKey)) 
            WallJump();
        }

        //Exiting Wall
        else if(exitingWall)
        {
            //Cancels active wallrun
            if(pm.wallrunning)
                StopWallRun();

            //Amount of time to leave wall
            if(exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            //Reaches 0
            if(exitWallTimer <= 0)
                exitingWall = false;
        }

        //Nonce
        else
        {   
            if(pm.wallrunning)
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Apply camera effects
        cam.DoFov(90f);
        if(wallLeft) cam.DoTilt(-5f);
        if(wallRight) cam.DoTilt(5f);
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //Adds force depending on direction orientated
        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = - wallForward;

        //forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        //diagonal movement
        if(upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);

        if(downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        //pushing towards wall
        if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        rb.AddForce(-wallNormal * 100, ForceMode.Force);

        //weaken gravity
        if(useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;

        //Reset Camera effects
        cam.DoFov(80f);
        cam.DoTilt(0f);
    }

    private void WallJump()
    {
        if(pm.grounded) return;
        if(lg.holding || lg.exitingLedge) return;
        //exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
