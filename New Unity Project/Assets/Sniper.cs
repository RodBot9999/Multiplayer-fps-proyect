
using UnityEngine;
using System.Collections;
public class Sniper : MonoBehaviour
{
    private bool isScoped = false;
    public GameObject scopeOverlay;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 10000f;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;
    int layerMask;
    public GameObject WeaponCamera;

    public Camera fpsCam;
    public Animator animator;

  public float scopezoom = 15f;

  private float unscopezoom;
    public ParticleSystem MuzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;
    void Start()
    {
        if (currentAmmo <= 9)
        {
            currentAmmo = maxAmmo;
        }
        layerMask = LayerMask.GetMask("Target");
        Debug.Log("Started");
    }

    void OnEnable()
    {
        WeaponCamera.SetActive(true);
        fpsCam.fieldOfView = 60f;
        scopeOverlay.SetActive(false);
        Debug.Log("i am alive");
        isReloading = false;
        animator.SetBool("Reload", false);
    }


    // Update is called once per frame
    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
         if (Input.GetButtonDown("Fire2"))
         {
            isScoped = !isScoped;
             animator.SetBool("Scoped", isScoped);
            if (isScoped)
               StartCoroutine(OnScoped());
               
            else
                OnUnscoped();    
         }


      


        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            Debug.Log("Hello");
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }





    }
    void OnUnscoped ()
    {
     fpsCam.fieldOfView = unscopezoom;
     WeaponCamera.SetActive(true);
     scopeOverlay.SetActive(false);
    }
    IEnumerator OnScoped ()
    {
     unscopezoom = fpsCam.fieldOfView;
     yield return new WaitForSeconds(.25f);
     WeaponCamera.SetActive(false);
     scopeOverlay.SetActive(true);
     fpsCam.fieldOfView = scopezoom;
    }
    IEnumerator Reload()
    {
        Debug.Log("Reloading...");
        animator.SetBool("Reload", true);

        yield return new WaitForSeconds(reloadTime - 0.25f);

        animator.SetBool("Reload", false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {

        MuzzleFlash.Play();
        Debug.Log("Shooting...");
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask))
        {

            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGo, 2f);
        }


    }
}
