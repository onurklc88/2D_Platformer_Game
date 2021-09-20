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
    public float reloadTime = 1f;
    


    private bool isReloading;
    
    public int currrentAmmo;
    public int maxAmmo = 10;
    public int magazineSize = 30;
    public int BulletRemainder;
    private bool glockShooting;
    private bool ReloadAnim;
   

    public static Weapon WeaponScript;
    public Transform pistolFire;
    public TextMeshProUGUI ammoInfoText;
    public GameObject ReloadSprite;
    Transform firePoint;
    
    public LayerMask whatToHit;
    public Transform BulletTrailPrefab;
    private Animator anim;

    void Awake()
    {

        WeaponScript = this;


        firePoint = transform.Find("FirePoint");

        if (firePoint == null)
        {
            Debug.LogError("Anayin Amii");


        }
      



    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        currrentAmmo = maxAmmo;
        
    }

    // Update is called once per frame
    void Update()
    {

       




        Weapon currentGun = FindObjectOfType<Weapon>();
        ammoInfoText.text = currentGun.currrentAmmo + " / " + currentGun.magazineSize;


        if (isReloading)
            return;



        if (fireRate == 0)
        {

            if (Input.GetMouseButtonDown(0) && currrentAmmo != 0)
            {



               

                glockShooting = true;
                
                anim.SetBool("glockShooting", glockShooting);
                CinemachineShake.Instance.ShakeCamera(4f, .1f);
               
                Shoot();
                

            }
            else if (Input.GetMouseButtonUp(0) && currrentAmmo != 0)
            {
               
                glockShooting = false;
               
                anim.SetBool("glockShooting", glockShooting);
                

            }
            else if (Input.GetMouseButtonDown(0) && currrentAmmo == 0)
            {
                CinemachineShake.Instance.ShakeCamera(0f, 0f);
                Shoot();
               
                
               
            }
            

        }

        else
        {
            if (Input.GetMouseButton(0) && Time.time > timeToFire)
            {

                timeToFire = Time.time + 1 / fireRate;
                Shoot();
               
            }




        }
    }
    

   

    public void ReloadSystem()
    {


        

        if (currrentAmmo > 0 && (magazineSize - BulletRemainder) > 0 && magazineSize >= 5)
        {

            
            BulletRemainder = maxAmmo - currrentAmmo;
            currrentAmmo = currrentAmmo + BulletRemainder;
            magazineSize = magazineSize - BulletRemainder;
           

            StartCoroutine(ReloadAction());
           
        
            return;


        }
        else if (currrentAmmo == 0)
        {



            if (magazineSize >= maxAmmo)
            {
                
                anim.SetBool("isReloading", ReloadAnim);
                currrentAmmo = maxAmmo;
                magazineSize -= maxAmmo;
                StartCoroutine(ReloadAction());
                return;

            }
            else
            {

                currrentAmmo = magazineSize;
                magazineSize = 0;
                StartCoroutine(ReloadAction());
                return;

            }

        }


        if ((magazineSize - BulletRemainder) < 0)
        {
            
            currrentAmmo = currrentAmmo + magazineSize;
            if(currrentAmmo > maxAmmo)
            {
                magazineSize = currrentAmmo - maxAmmo;
                currrentAmmo = maxAmmo;
                StartCoroutine(ReloadAction());
                return;

            }
            else if(magazineSize <= 5 && currrentAmmo != maxAmmo)
            {
                Debug.Log(currrentAmmo);
                Debug.Log(magazineSize);
                
                magazineSize = 0;
                StartCoroutine(ReloadAction());
                return;


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
                StartCoroutine(ReloadAction());
                return;
                



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
        Transform clone = Instantiate(pistolFire, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(4f, 1.97f);

        clone.localScale = new Vector3(3f, 3f, size);
        
        Destroy(clone.gameObject, 0.02f);

    }

    IEnumerator ReloadAction()
    {

        isReloading = true;
       
        Debug.Log("Reloading..");
        ReloadSprite.SetActive(true);
      anim.SetBool("isReloading", true);
        yield return new WaitForSeconds(reloadTime - 1f);
        Debug.Log(reloadTime);
        ReloadSprite.SetActive(false);
        anim.SetBool("isReloading", false);
        

        isReloading = false;
       
        

      

    }

   


}

     

