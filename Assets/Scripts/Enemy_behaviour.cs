using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behaviour : MonoBehaviour
{

    public Transform rayCast;
    public LayerMask raycastMask;
    public float rayCastLength;
    public float attackDistance; //Minimum distance for attack
    public float moveSpeed;
    public float timer; //Timer for cooldown between attakcs

    private RaycastHit2D hit;
    private GameObject target;
    private Animator anim;
    private float distance;
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;


    private void Awake()
    {
        intTimer = timer; //Store the inital value of timer
        anim = GetComponent<Animator>();

    }

    
    void Update()
    {
        if(inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, raycastMask);
            RaycastDebugger();



        }
        //when player detected
        if(hit.collider != null)
        {

            EnemyLogic();

        }
        else if(hit.collider == null)
        {

            inRange = false;
        }

        if(inRange == false)
        {

          
        }

    }

   void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.tag == "Player")
        {
            target = trig.gameObject;
            inRange = true;


        }
    }


    void RaycastDebugger()
    {

        if(distance > attackDistance)
        {

            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);


        }
        else if(attackDistance > distance)
        {

            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);

        }




    }

    void EnemyLogic()
    {

        distance = Vector2.Distance(transform.position, target.transform.position);

        if(distance > attackDistance)
        {
            Move();
            
            

        }
        else if (attackDistance >= distance && cooling == false)
        {
            anim.SetBool("Attack", false);
            
        }

    }
    void Move()
    {

      

    }

}
