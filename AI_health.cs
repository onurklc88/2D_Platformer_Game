using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_health : MonoBehaviour
{
    public int AIHealth = 100;
    public Animator anim;
    public float currentHealth;
    public AIHealthBar Healthbar;
    public float Hitpoints;




    void Start()
    {
        Hitpoints = AIHealth;
        Healthbar.SetHealth(Hitpoints, AIHealth);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Healthbar.SetHealth(Hitpoints, AIHealth);

        if (collision.gameObject.CompareTag("glockBullet"))
        {

            AIHealth = AIHealth - 10;

            if (AIHealth <= 0)
            {

                //dead
               
                
                //Destroy(gameObject);
                Debug.Log("AI dead");


            }

        }

       


    }



}

    
