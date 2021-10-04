using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_behaviour : MonoBehaviour
{

  
    
   
    public float attackDistance; //Minimum distance for attack
    public float timer; //Timer for cooldown between attakcs
    
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    public GameObject hotZone;
    public GameObject triggerArea;


    private Animator anim;
    private float distance;
    private bool attackMode;
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

        
        //when raycast is not null
        

        if(inRange)
        {
            EnemyLogic();
          
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

  public void SelectTarget()
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

    public void Flip()
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





