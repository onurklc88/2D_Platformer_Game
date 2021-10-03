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
    public Transform leftLimit;
    public Transform rightLimit;

    private RaycastHit2D hit;
    private Transform target;
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
        SelectTarget();


    }

    
    void Update()
    {
        if (!attackMode)
        {

            Move();
        }

        if(!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("PatrolAI_attack"))
        {

            SelectTarget();

        }


        if(inRange)
        {

            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, raycastMask);
            RaycastDebugger();
            moveSpeed = 6;
            anim.SetBool("inRange", true);


        }
        //when raycast is not null
        if(hit.collider != null)
        {

            EnemyLogic();

        }
        //if nothing is in range
        else if(hit.collider == null) 
        {
            moveSpeed = 2;
            inRange = false;
        }

        if(inRange == false)
        {
            moveSpeed = 2;
            StopAttack();
            anim.SetBool("inRange", false);
          
        }

    }

   void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.tag == "Player")
        {

            //detect player
            target = trig.transform;
            inRange = true;

            Flip();


        }
    }


    void RaycastDebugger()
    {

        if(distance > attackDistance)
        {

            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.red);


        }
        else if(attackDistance > distance)
        {

            Debug.DrawRay(rayCast.position, transform.right * rayCastLength, Color.green);

        }


    }

    void EnemyLogic()
    {

        distance = Vector2.Distance(transform.position, target.position);

        if(distance > attackDistance)
        {
           
            StopAttack();
            //methiewwashere

        }
        else if (attackDistance >= distance && cooling == false)
        {

            Attack();
            
            

        }
        if (cooling)
        {
            Cooldown();
            anim.SetBool("Attack", false);
        }

    }
    void Move()
    {
        anim.SetBool("canWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("PatrolAI_attack"))
        {
            //player position
            Vector2 targetposition = new Vector2(target.position.x, transform.position.y);
            //Move to player
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
    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;


    }

  private void SelectTarget()
    {
        //puriandWasHere and jiriaeruWasHere

        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.position);

        if(distanceToLeft > distanceToRight)
        {

            target = leftLimit;

        }
        else
        {
            target = rightLimit;

        }

        Flip();

    }

    private void Flip()
    {

        Vector3 rotation = transform.eulerAngles;
        if(transform.position.x > target.position.x)
        {

            rotation.y = 180f;

        }
        else
        {

            rotation.y = 0f;

        }

        transform.eulerAngles = rotation;


    }

    void Cooldown()
    {

        timer -= Time.deltaTime;


        if(timer <= 0 && cooling && attackMode)
        {

            cooling = false;
            timer = intTimer;
        }

    }

    public void TriggerCooling()
    {

        cooling = true;


    }
}
