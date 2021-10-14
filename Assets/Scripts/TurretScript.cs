using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public Transform firepoint;
    public GameObject Bullet;
    float timeBetween;
    public float StartTimeBetween;
    private Animator anim;





    // Start is called before the first frame update
    void Start()
    {

        timeBetween = StartTimeBetween;



    }

    // Update is called once per frame
    void Update()
    {




        if (detectArea.DA.detectA == true)
        {



        
        if (timeBetween <= 0)
        {

            Instantiate(Bullet, firepoint.position, firepoint.rotation);
            timeBetween = StartTimeBetween;

        }
        else
        {

            timeBetween -= Time.deltaTime;

        }
        }

        

    }
}
