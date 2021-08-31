using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{




    private Rigidbody2D rb;
    private Animator anim;

    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canJump;
    private bool canWallJump;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool isDashing;
    private bool HoldingGun;

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
    
    public int maxHealth = 100;
    public int currentHealth;


    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;



    public Transform groundCheck;
    public LayerMask WhatIsGround;
    public Transform WallCheck;
    public HealthBar healthBar;
    Transform playerGraphics;


   





    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
        
     
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        UpdateAnimatons();
        CheckIfWallSliding();
        
       




    }
    private void FixedUpdate()
    {
        ApplyMoment();
        CheckSurrondings();

    }


    private void CheckIfWallSliding()
    {
        //if istouching wall true
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
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
        isTouchingWall = Physics2D.Raycast(WallCheck.position, transform.right, wallCheckDistance, WhatIsGround);
    }




    private void CheckIfCanJump()
    {
        //Jump count
        if((isGrounded && rb.velocity.y <= 0.01f) || isWallSliding)
        {

            amountOfJumpsLeft = amountOfJumps;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
             else
        {

            canJump = true;

        }
       
        //:3

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
       


        //Space Input
        if (Input.GetButtonDown("Jump"))
        {
            Jump();


        }
        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);

        }
        

        /*
        //jump higher when player holding space
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
           

        }
        */
}



    void TakeDamage(int damage)
    {

        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

    }
  
    private void UpdateAnimatons()
    {
        //setting up the animations
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("HoldingGun", HoldingGun);
        anim.SetFloat("Speed", Mathf.Abs(movementInputDirection));

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
      

        
    }


    /*
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

              Jump();
            }


        }
        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;

        }



    }
    */
    private void Jump()
    {
        //taking to velocity of rb on x axis
        if (canJump && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            amountOfJumpsLeft--;
           
        }
        else if(isWallSliding && movementInputDirection ==0 && canJump) //walhop
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);

        }
        else if((isWallSliding || isTouchingWall) && movementInputDirection != 0 && canJump)
        {

            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);

        }


    }
   
    /*
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
    */


    private void ApplyMoment()
    {

        if (isGrounded)
        {
            ////////////////////////////////////////////////
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);

        }
        else if(!isGrounded && !isWallSliding && movementInputDirection != 0)
        {

            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            if(Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);


            }
            else if(!isGrounded && !isWallSliding && movementInputDirection == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);


            }
        }
        


        if(isWallSliding)
        {

            if(rb.velocity.y < -wallSlideSpeed)
            {

                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);

            }
        }


       
        }



       
    



    //Sprite Flipper
    private void Flip()
    {



        if (!isWallSliding)
        {


            facingDirection *= -1;
        isFacingRight = !isFacingRight;
        //rotate the sprite
        transform.Rotate(0.0f, 180f, 0.0f);
        }
    }








    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallCheckDistance, WallCheck.position.y, WallCheck.position.z));
    }
}
