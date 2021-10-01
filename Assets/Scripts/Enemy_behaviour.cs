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
            anim.SetBool("canWalk", false);
            StopAttack();
          
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
            StopAttack();
            //methiewwashere

        }
        else if (attackDistance >= distance && cooling == false)
        {

            Attack();
            
            

        }
        if (cooling)
        {

            anim.SetBool("Attack", false);
        }

    }
    void Move()
    {
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PatrolAI_attack"))
        {
            Vector2 targetposition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetposition, moveSpeed * Time.deltaTime);




        }
      

    }

    void Attack()
    {

        timer = intTimer; //reset timer when players enter attack range
        attackMode = true; //to check if enemy can still attack or not
        anim.SetBool("Attack", true);
        anim.SetBool("canWalk", false);
    }

    void StopAttack()
    {
        cooling = false;
        attackMode = false;
        anim.SetBool("Attack", false);
        


    }




}
