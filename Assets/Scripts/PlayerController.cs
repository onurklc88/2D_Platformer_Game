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
    private bool isTouchingWall;
    private bool isWallSliding;


    private int amountOfJumpsLeft;
    public int amountOfJumps = 1;


    private float movementInputDirection;
    public float movementSpeed = 10.0f;
     public float JumpForce = 3.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;



    public Transform groundCheck;
    public LayerMask WhatIsGround;
    public Transform WallCheck;







    void Start()
    {
      
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
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

        if(isTouchingWall && !isGrounded && rb.velocity.y < 0)
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
        if(isGrounded && rb.velocity.y <= 0)
        {
            
            amountOfJumpsLeft = amountOfJumps;
        }

        
        if(amountOfJumpsLeft <= 0)
        {

            canJump = false;

        }
        else
        {

            canJump = true;
        }
       
        

    }


    //We'r going to check where do we look exactly
    private void CheckMovementDirection()
    {

        
        //turn left if character is moving on -x axis
        if(isFacingRight && movementInputDirection < 0)
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
            Jump();

        }



    }


    private void UpdateAnimatons()
    {
        //setting up the animations
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);

    }





    private void Jump()
        {
        //taking to velocity of rb on x axis
        if(canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            amountOfJumpsLeft--;
        }
        }


 
    private void ApplyMoment()
    {

      

        

            //taking to velocity of rb on y axis
           rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
       
       





        if (isWallSliding)
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
