using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_AI : MonoBehaviour
{

    public GameObject bullet;
    public float speed;
    private Transform player;
    private Vector2 target;







    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BulletFire()
    {
        Instantiate(bullet, transform.position, Quaternion.identity);


    }
   void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Player"))
        {
            BulletFire();



        }

    }

}
