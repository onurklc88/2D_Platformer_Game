using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float MovementSpeed = 1f;
    float horizontalMove = 0f;
    public float JumpForce = 1f;
    
    private Rigidbody2D rigidbody;
    public Animator animator;
    bool OnLanding;
    


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    
    private void Update()
    {
        //Yatay movement ayarý
      horizontalMove = Input.GetAxis("Horizontal") * MovementSpeed;
        transform.position += new Vector3(horizontalMove, 0, 0) * Time.deltaTime * MovementSpeed;
        //Karakterin Hýzýna göre Animasyon çaðýrmak
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
      
        //baktýðý yöne döndürme kodu
      
        if (!Mathf.Approximately(0, horizontalMove))

            transform.rotation = horizontalMove < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;


     



        //Zýplama 
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rigidbody.velocity.y) < 0.001f)
        {
            rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
           
            animator.SetBool("Is_Jumping", true);
        }

      


    }
  
    


  
        
}


