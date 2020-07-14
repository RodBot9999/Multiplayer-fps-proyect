using UnityEngine;
using System.Collections;

public class gunpistol : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 10000f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;
    
    
    //private bool nobug = false;

    public Camera fpsCam;
    public ParticleSystem MuzzleFlash;
    public GameObject impactEffect;
    public ParticleSystem Trail;
    public AudioSource Shot;

    private float nextTimeToFire = 0f;

    public Animator animator;
    

    void Start ()
    {
        currentAmmo = maxAmmo;
    }

    void OnEnable ()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
        //nobug = false;
    }

    

    // Update is called once per frame
    void Update () {
        if(isReloading)
        return;
    
    
        
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }

        




        
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
