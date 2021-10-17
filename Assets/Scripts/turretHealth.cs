using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretHealth : MonoBehaviour
{
    public int AIHealth = 100;
    public Animator anim;
    public int currentHealth;
    public static AI_health AH;
    public float Hitpoints;
    public int counter = 0;
    public HealthBar healthBar2;
    public GameObject BarPosition;
    





    private void Awake()
    {
       
    }


    void Start()
    {
        currentHealth = AIHealth;
        healthBar2.SetMaxHealth(AIHealth);
    }

    private void Update()
    {

        Death();


    }



    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "glockBullet")
        {

            //AIHealth = AIHealth - 10;

            AIHealth -= 5;
            currentHealth = AIHealth;

            healthBar2.SetHealth(currentHealth);
            //ajSaryWasHere


        }




    }



    public void Death()
    {



        if (AIHealth <= 0)
        {
            
            BarPosition.SetActive(false);
            anim.SetBool("isDead", true);
            Destroy(gameObject, 1f);

            Debug.Log("AI dead");
            //purple_abeWasHere

        }




    }
}
