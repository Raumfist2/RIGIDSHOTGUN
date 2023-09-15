using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement pm;
    public Transform orientation;
    public Transform cam;
    public Rigidbody rb;

    [Header("Ledge Grabbing")]
    public float moveToLedgeSpeed;
    public float maxLedgeGrabDistance;

    public float minTimeOnLedge;
    private float timeOnLedge;

    public bool holding;

    [Header("Ledge Jumping")]
    public KeyCode jumpKey = KeyCode.Space;
    public float ledgeJumpForwardForce;
    public float ledgeJumpUpwardForce;

    [Header("Ledge Detection")]
    public float ledgeDetectionLength;
    public float ledgeSphereCastRadius;
    public LayerMask whatIsLedge;

    private Transform lastLedge; 
    private Transform currLedge;

    private RaycastHit ledgeHit;

    [Header("Exiting")]
    public bool exitingLedge;
    public float exitLedgeTime;
    private float exitLedgeTimer;

    private void Update()
    {
        //Run functions
        LedgeDetection();
        SubStateMachine();
    }

    private void SubStateMachine()
    {
        //Get input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        //Checks if there is input
        bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;

        //HOLDING LEDGE
        if(holding)
        {
            FreezeRigidbodyOnLedge();//Stops rigidbody
            timeOnLedge += Time.deltaTime;//Timer

            if(timeOnLedge > minTimeOnLedge && anyInputKeyPressed) ExitLedgeHold();//No time left

            if(Input.GetKeyDown(jumpKey)) LedgeJump();//jumps off
        }
        //EXITING LEDGE
        else if(exitingLedge)
        {
            if(exitLedgeTimer > 0) exitLedgeTimer -= Time.deltaTime;
            else exitingLedge = false;
        }
    }

    private void LedgeDetection()
    {
        bool ledgeDetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.forward, out ledgeHit, ledgeDetectionLength, whatIsLedge); //Uses spherecast to check if there is a ledge infront of the player
        
        if(!ledgeDetected) return;//No ledge
        
        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);//Distance from the ledge detected
        
        if (ledgeHit.transform == lastLedge) return;//Same ledge
        
        if (distanceToLedge < maxLedgeGrabDistance && !holding) EnterLedgeHold();//All values correct then EnterLedgeHold(
    }

    private void LedgeJump()
    {
        ExitLedgeHold();//Gets off ledge

        Invoke(nameof(DelayedJumpForce), 0.05f);//Appies force
    }

    private void DelayedJumpForce()
    {
        Vector3 forceToAdd = cam.forward * ledgeJumpForwardForce + orientation.up * ledgeJumpUpwardForce;//Finds the rotation etc, of where to apply the force
        rb.velocity = Vector3.zero;//resets velocity
        rb.AddForce(forceToAdd, ForceMode.Impulse);//Applies using ForceMode.Impulse
    }

    private void EnterLedgeHold()
    {
        //Assigns values
        holding = true;
        pm.unlimited = true;
        pm.restricted = true;

        currLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    private void FreezeRigidbodyOnLedge()
    {
        //Disables gravity
        rb.useGravity = false;
        
        
        Vector3 directionToLedge = currLedge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, currLedge.position);

        //move player to ledge
        if(distanceToLedge > 1f)
        {
            if(rb.velocity.magnitude < moveToLedgeSpeed)
                rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
        }

        //hold onto ledge
        else
        {
            if(!pm.freeze) pm.freeze = true;
            if(pm.unlimited)pm.unlimited = false;
        }

        //exiting uh oh
        if(distanceToLedge > maxLedgeGrabDistance) ExitLedgeHold();
    }

    private void ExitLedgeHold()
    {
        exitingLedge = true;
        exitLedgeTimer = exitLedgeTime;

        holding = false;
        timeOnLedge = 0f;

        pm.restricted = false;
        pm.freeze = false;

        rb.useGravity = true;
        StopAllCoroutines();
        //One second timeframe between ledge jumps
        Invoke(nameof(ResetLastLedge), 1f);
    }

    private void ResetLastLedge()
    {
        lastLedge = null;
    }
}
