using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LedgeGrabbing lg;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("ClimbJumping")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;

    public KeyCode jumpKey = KeyCode.Space;
    public int climbJumps;
    private int climbJumpsLeft;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    private void Start()
    {
        lg = GetComponent<LedgeGrabbing>();//Grabs component
    }

    private void Update()
    {
        //Run functions
        WallCheck();
        StateMachine();

        if (climbing && !exitingWall) ClimbingMovement();//Perform if climbing but not leaving the wall
    }

    private void StateMachine()
    {
        //LEDGE GRAB
        if(lg.holding)
        {
            if(climbing) StopClimbing();
        }


        //CLIMBING
        else if(wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle && !exitingWall)
        {
            if(!climbing && climbTimer > 0) StartClimbing();

            //CLIMB TIMER
            if(climbTimer > 0) climbTimer -= Time.deltaTime;
            if(climbTimer < 0) StopClimbing();
        }

        //EXITING
        else if(exitingWall)
        {
            if(climbing) StopClimbing();

            if(exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if(exitWallTimer < 0) exitingWall = false;
        }

        //NONCE
        else
        {
            if(climbing) StopClimbing();
        }

        if(wallFront && Input.GetKeyDown(jumpKey) && climbJumpsLeft > 0) ClimbJump(); //Jumping off wall
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall); //Calc using sphere cast the wall infront of the player
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);//Calc wall angle the user is looking at

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange; //Checks if the previous wall climbed is the same

        //Resets Climb timer once grounded
        if ((wallFront && newWall) || pm.grounded)
        {
            //reset
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }
    
    private void StartClimbing()
    {
        //Set values to true
        climbing = true;
        pm.climbing = true;

        //stores last hit wall info
        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);//Set y vel to climb speed
    }

    private void StopClimbing()
    {
        //Deactivate climbing
        climbing = false;
        pm.climbing = false;
    }

    private void ClimbJump()
    {
        //Checks if not grounded or leaving a ledge.
        if(pm.grounded) return;
        if(lg.holding || lg.exitingLedge) return;

        //Sets exiting wall true & reset wall timer
        exitingWall = true;
        exitWallTimer = exitWallTime;

        //Applys forces from climbJumpUpForce
        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;

        //Calcs velocity and adds it using ForceMode.Impulse
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        //Counts Climbjumps after each
        climbJumpsLeft--;
    }
}
