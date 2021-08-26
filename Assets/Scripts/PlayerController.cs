using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{




    private Rigidbody2D rb;
    private Animator anim;

    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool isDashing;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    public int amountOfJumps = 1;
    private float jumpTimer;
    private float turnTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;





    private float movementInputDirection;
    public float movementSpeed = 10.0f;
    public float JumpForce = 3.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.01f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;


    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;



    public Transform groundCheck;
    public LayerMask WhatIsGround;
    public Transform WallCheck;







    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        UpdateAnimatons();
        CheckIfWallSliding();
        CheckJump();
        CheckDash();

    }
    private void FixedUpdate()
    {
        ApplyMoment();
        CheckSurrondings();

    }


    private void CheckIfWallSliding()
    {
        //if istouching wall true
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0)
        {

            isWallSliding = true;

        }
        else
        {

            isWallSliding = false;
        }

    }





    //cheking for wall or smh
    private void CheckSurrondings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, WhatIsGround);
        isTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, wallCheckDistance);
    }




    private void CheckIfCanJump()
    {
        //Jump count
        if((isGrounded && rb.velocity.y < 0.01f) || isWallSliding)
        {

            amountOfJumpsLeft = amountOfJumps;
        }
        if (isTouchingWall)
        {
            canWallJump = true;
        }


        if (amountOfJumpsLeft <= 0)
        {

            canNormalJump = false;

        }
        else
        {

            canNormalJump = true;
        }



    }


    //We'r going to check where do we look exactly
    private void CheckMovementDirection()
    {


        //turn left if character is moving on -x axis
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();



        }
        //turn right if character is moving on +x axis
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

    }

    private void CheckInput()
    {
        //A and D ýnput
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        //calling to animation Run
        anim.SetFloat("Speed", Mathf.Abs(movementInputDirection));


        //Space Input
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || (amountOfJumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();

            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;

            }


        }
        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;

            }

        }
        if (!canMove)
        {

            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {

                canMove = true;
                canFlip = true;
            }
        }

        //jump higher when player holding space
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);

        }

        if (Input.GetButtonDown("Dash"))
        {
            if(Time.time >= (lastDash + dashCoolDown))
            {
                
                AttemptToDash();
                isDashing = true;
            }

        }



    }
    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterPool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;

    }
    private void CheckDash()
    {
        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {

                    PlayerAfterPool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;


                }

            }
            

        }
        if(dashTimeLeft <=0 || isTouchingWall)
        {
            isDashing = false;
            canMove = true;
            canFlip = true;

        }

    }


    private void UpdateAnimatons()
    {
        //setting up the animations
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);

    }





    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            //wallJump
            if (!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();

            }
            else if (isGrounded)
            {

                NormalJump();
            }


        }
        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;

        }



    }
    private void NormalJump()
    {
        //taking to velocity of rb on x axis
        if (canNormalJump && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
        }


    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);


            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * -facingDirection, wallHopForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }

    }



    private void ApplyMoment()
    {





        if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {

            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);

        }
        else if (canMove)
        {
            //taking to velocity of rb on y axis
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);


        }



        //decrease speed when character is sliding on the wall

        if (isWallSliding)
        {

            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);

            }

        }

    }



    //Sprite Flipper
    private void Flip()
    {





        isFacingRight = !isFacingRight;
        //rotate the sprite
        transform.Rotate(0.0f, 180f, 0.0f);
    }








    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallCheckDistance, WallCheck.position.y, WallCheck.position.z));
    }
}
