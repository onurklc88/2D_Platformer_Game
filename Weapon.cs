using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0;
    public float damage = 10;
    public float timeToFire = 0;
    public float timeToSpawnEffect;
    public float effectSpawnRate = 10;
    public float reloadTime = 2f;

    private bool isReloading;
    public int currrentAmmo;
    public int maxAmmo = 10;
    public int magazineSize = 30;
    public int BulletRemainder;


    public static Weapon WeaponScript;
    public TextMeshProUGUI ammoInfoText;
    Transform firePoint;
    public LayerMask whatToHit;
    public Transform BulletTrailPrefab;


    void Awake()
    {

        WeaponScript = this;


        firePoint = transform.Find("FirePoint");

        if(firePoint == null)
        {
            Debug.LogError("Anayin Amii");


        }




    }

    private void Start()
    {
        currrentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {


        Weapon currentGun = FindObjectOfType<Weapon>();
        ammoInfoText.text = currentGun.currrentAmmo + " / " + currentGun.magazineSize;


     if(currrentAmmo == 0 && !isReloading)
        {

           
        }




        if (fireRate == 0)
        {

            if(Input.GetMouseButtonDown(0))
            {

                Shoot();
               
            }

        }

        else
        {
            if(Input.GetMouseButton(0) && Time.time > timeToFire)
            {

                timeToFire = Time.time + 1 / fireRate;
                Shoot();

            }




        }
     

    }


    public void ReloadSystem()
    {

        if (magazineSize > 0 && currrentAmmo > 0)
        {


            BulletRemainder = maxAmmo - currrentAmmo;
            currrentAmmo = currrentAmmo + BulletRemainder;

            if ((magazineSize - BulletRemainder) > 0)
            {

                magazineSize = magazineSize - BulletRemainder;


            }
            
                    
                    
                    




            }



        }
      



    


      


    void Shoot()
    {
        if (currrentAmmo > 0)
        {




            Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
            RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
            currrentAmmo--;
            if (Time.time >= timeToSpawnEffect)
            {
                Effect();
                timeToSpawnEffect = Time.time + 1 / effectSpawnRate;

            }
        }
        else if(currrentAmmo == 0)
        {

            if (magazineSize >= maxAmmo)
            {
                currrentAmmo = maxAmmo;
                magazineSize -= maxAmmo;


            }
            else
            {

                currrentAmmo = magazineSize;
                magazineSize = 0;


            }

        }



        

 
    }
    void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);



    }


}

     

