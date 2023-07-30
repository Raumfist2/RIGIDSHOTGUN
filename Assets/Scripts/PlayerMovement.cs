using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    //MOVEMENT STATES
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //Assigns rigidbody
        rb.freezeRotation = true; //Stops player from falling over

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        //Inputs every frame
        MyInput();
        SpeedControl();
        StateHandler();

        //Handles Drag
        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate() 
    {
        MovePlayer();
    }

    private void MyInput()
    {
        //Get Movement Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Get Jump Inputs
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;//Reset the jump

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);//Lets the player continously jump while holding the key
        }

        //Get Crouch Inputs
        if(Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        //CROUCHING
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        //SPRINTING
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        //WALKING
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        //AIR
        else
        {
            state = MovementState.air;
        }
    }


    private void MovePlayer()
    {
        //Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; //Moving in the direction looking

        //On Ground
        if(grounded)
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); //Apply force to Rigidbody

        //In Air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); //Apply force to Rigidbody

    }

    private void SpeedControl()
    {
        //Manually limiting
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limiting if greater
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
