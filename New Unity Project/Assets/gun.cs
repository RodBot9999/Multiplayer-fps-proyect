
using UnityEngine;
using System.Collections;

public class gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 10000f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;
    private bool isScoped = false;
    public GameObject scopeOverlay;
    public bool IsInInventory = false;
    //private bool nobug = false;

    public Camera fpsCam;
    public ParticleSystem MuzzleFlash;
    public GameObject impactEffect;
    public ParticleSystem Trail;
    public AudioSource Shot;
    public GameObject WeaponCamera;
    public Camera MainCamera;
    public Camera WeaponCamera2;

    public float scopedFOV = 12f;
    private float normalFOV;

    private float nextTimeToFire = 0f;
   

    public Animator animator;
    

    void Start ()
    {
        currentAmmo = maxAmmo;
    }
    void OnEnable ()
    {
        //ameObject.layer = 11;
        isReloading = false;
        animator.SetBool("Reloading", false);
          scopeOverlay.SetActive(false);
         WeaponCamera.SetActive(true);
          animator.SetBool("Scoped",false);
          WeaponCamera2.cullingMask = 1 << 11;

         MainCamera.fieldOfView = 26f;


        //nobug = false;
    }


   

    

    // Update is called once per frame
    void Update () {
        if(isReloading)
        return;
        if(IsInInventory)
        {
            gameObject.layer = 11;
            animator.SetBool("IsInventory", true);
        }
    
    
        
        if (currentAmmo <= 0 && IsInInventory)
        {
            StartCoroutine(Reload());
            return;
        }

        
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && IsInInventory)
        {
            
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }

        if (Input.GetButtonDown("Fire2") && IsInInventory)
        {
            isScoped = !isScoped;
            animator.SetBool("Scoped",isScoped);

            if (isScoped)
               StartCoroutine(OnScoped());
            else
               OnUnscoped();
        
            
            
            
        }




        
    }
    void OnUnscoped ()
    {
       scopeOverlay.SetActive(false);
       WeaponCamera.SetActive(true);
       MainCamera.fieldOfView = normalFOV;

     
    }
    IEnumerator OnScoped ()
    {
        gameObject.layer = 11;

        yield return new WaitForSeconds(.16f);

         scopeOverlay.SetActive(true);
         WeaponCamera.SetActive(false);
         normalFOV = MainCamera.fieldOfView;

         MainCamera.fieldOfView = scopedFOV;



    }
     
    IEnumerator Reload ()
    {
        isReloading = true;
        Debug.Log("reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading",false);
        yield return new WaitForSeconds(.25f);


        currentAmmo = maxAmmo;
        isReloading = false;
        //nobug = true;
    }

    void Shoot ()
    {

        MuzzleFlash.Play();
        Shot.Play();
        Trail.Play();
        
        currentAmmo--;
        RaycastHit hit;
         if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
         {

             Debug.Log(hit.transform.name);

             Target target = hit.transform.GetComponent<Target>();
             if (target != null)
             {
                 target.TakeDamage(damage);
             }

             if(hit.rigidbody != null)
             {
                 hit.rigidbody.AddForce(-hit.normal * impactForce );
             }

             GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
             Destroy(impactGo, 2f );
           
         }


    }
}
