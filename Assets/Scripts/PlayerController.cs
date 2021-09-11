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
    private bool HoldingGun;
    private bool pistolActive = false;
    private float isGunTaken;
    private bool CrouchActive = false;
    private bool CrouchAnim;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    public int amountOfJumps = 1;
    private float jumpTimer;
    private float turnTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    public bool PistolTaken;
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

    public GameObject arm1;
    public GameObject arm2;
    public BoxCollider2D player;
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
        player = player.GetComponent<BoxCollider2D>();
        
     
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckIfCanJump();
        UpdateAnimatons();
        CheckIfWallSliding();
        WhileCrouching();




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

        if (pistolActive == false)
        {
            if (PistolTaken && Input.GetKeyDown(KeyCode.Alpha1))
            {

                WeaponSwitch.gl.glock();
                anim.SetBool("HoldingGun", HoldingGun);
                pistolActive = true;
            }
            




        }
        else if (pistolActive == true){

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

                pistolActive = false;
                WeaponSwitch.gl.glockInavtive();
                anim.SetBool("HoldingGun", !HoldingGun);
                



            }




        }

       if (isGrounded && !isWallSliding) {

            if (Input.GetButton("Crouch"))
            {
                CrouchActive = true;
                player.size = new Vector2(0.4395781f, 0.8f);
                player.offset = new Vector2(0.0002865791f, -0.2f);
                CrouchAnim = true;
                movementSpeed = 3.0f;
          
                
              

            }
           

            else if (CrouchActive = true && isGrounded && !isWallSliding)
            {
                if (Input.GetButtonUp("Crouch"))
                {
                   
                    CrouchActive = false;
                    player.size = new Vector2(0.4395781f, 1.25598f);
                    player.offset = new Vector2(0.0002865791f, 0.001142859f);
                    CrouchAnim = false;
                    movementSpeed = 10.0f;
                }


            }
        }
        

        if (Input.GetButtonDown("Drop"))
        {
            Slot.Sl.DropItem();

          


        }

        



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
     anim.SetFloat("Speed", Mathf.Abs(movementInputDirection));



        //crouch anim without weapon
        if(CrouchActive == true)
        {
           
            anim.SetBool("Crouch", CrouchAnim);

        }





    }


    


    
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "glockPistol")
        {
            HoldingGun = true;
            PistolTaken = true;



        }

        

        }
    


        private void ApplyMoment()
    {

        if (isGrounded)
        {
           
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



    void WhileCrouching()
    {

        if (Input.GetButtonDown("Crouch"))
        {

           arm1.transform.position = new Vector2(arm1.transform.position.x, arm1.transform.position.y - 0.3f);
           arm2.transform.position = new Vector2(arm2.transform.position.x, arm2.transform.position.y - 0.3f);
            
            

        }else if (Input.GetButtonUp("Crouch"))
        {

            arm1.transform.position = new Vector2(arm1.transform.position.x, arm1.transform.position.y + 0.3f);
            arm2.transform.position = new Vector2(arm2.transform.position.x, arm2.transform.position.y + 0.3f);

        }



    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(WallCheck.position, new Vector3(WallCheck.position.x + wallCheckDistance, WallCheck.position.y, WallCheck.position.z));
    }
}
