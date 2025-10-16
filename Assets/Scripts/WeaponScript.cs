using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;


//This script controls the behavior of the Weapon it has all the needed parameters to define a gun: range,damage,fireRate,Ammo, and the associated animations
public class WeaponScript : MonoBehaviour
{
    public float range = 100f;
    public float damage = 10f;
    public Animator animator;
    public float damageHead = 50;
    public Animator HandAnimator;

    public Camera fpsCam;
    public float fireRate = 1.25f;
    public int currentAmmo = -1;
    public float reloadTime = 1f;

    public float nextTimeFire = 0f;
    public int maxAmmo = 10;
    public int totalCurrentAmmo;
    public int totalMaxAmmo;
    public bool IsReloading = false;
    public AudioSource audioSource;
    public int startTotalAmo;
    public ParticleSystem muzzleflash;
    public ParticleSystem bulletCasing;
    public AudioSource reload;
    public GameObject bulletHolePrefab;

    public GameObject BloodSystem;
    public float timeOfBlood;
     Animator animShooting;
    public AudioClip dryFireAC;
    public Text AmoText;
    public bool active;
    void Awake()
    {

        if (currentAmmo == -1)
            currentAmmo = maxAmmo;
    }
 
    void OnDisable()
    {
        active = false;
    }
    // At start we need stop the animations and reset the ammo
    void Start()
    {
        AmoText.text = "" + currentAmmo + "/" + totalCurrentAmmo;
        muzzleflash.Stop();
        bulletCasing.Stop();
        animShooting = GetComponent<Animator>();
    }
    void OnEnable()
    {
        AmoText.text = "" + currentAmmo + "/" + totalCurrentAmmo;
        active = true;
        muzzleflash.Stop();
        bulletCasing.Stop();
    }

    //The gun should be able to reload when it has ammo, and fire when its ready
    void Update()
    {
        //Check if gun is reloading to stop new reload action or firing
        if (IsReloading)
            return;
        //make sure we ahve enough ammo and the button R is pressed to reload
        if (totalCurrentAmmo > 0 && (Input.GetKeyDown(KeyCode.R)) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;

        }
        //Check if no ammo is available to see if bullet should be launched or not
        if (transform.name == "RussianPistol")
        {
            if (currentAmmo <= 0 && Input.GetButtonDown("Fire1") && Time.time >= nextTimeFire)
                {
                    nextTimeFire = Time.time + 1f / fireRate;
                    Dryfire();
                    return;
                }
        }
        else
        {
            if (currentAmmo <= 0 && Input.GetButtonDown("Fire1") && Time.time >= nextTimeFire)
            {
                nextTimeFire = Time.time + 1f / fireRate;
                Dryfire();
                return;
            }
        }
        if (transform.name == "RussianPistol")
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeFire)
            {
                nextTimeFire = Time.time + 1f / fireRate;

                Shoot();
            }
        }
        else
        {

            if (Input.GetButton("Fire1") && Time.time >= nextTimeFire)
            {
                nextTimeFire = Time.time + 1f / fireRate;

                Shoot();
            }
        }
    }
    //firing animation without the bullet and the smoke
    void Dryfire()
    {
        audioSource.PlayOneShot(dryFireAC);
        animShooting.SetTrigger("DryFire");
        HandAnimator.SetTrigger("Firing");
    }
    //Firing animation where we send a ray to see if enemy is at crosshair location, then if the enemy is hit reduce his health otherwise create a bullet holoe animation
    void Shoot()
    {

        
        currentAmmo--;
        HandAnimator.SetTrigger("Firing");
        animShooting.SetTrigger("Shooting");
        AmoText.text = "" + currentAmmo + "/" + totalCurrentAmmo;
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layerMask))
        {

            //Debug.Log(hit.transform.name);
            //Debug.Log(hit.collider.tag);
            

            if (hit.collider.tag == "Wall") {
                //Debug.Log("spwaning  bullet hole");
                Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));

            }
            if (hit.collider.tag == "enemy")
            {
                //Debug.Log("spwaning  blood system");
                GameObject blood = Instantiate(BloodSystem, hit.point, Quaternion.LookRotation(hit.normal));

             
               
                    EnemyAttack enemyScript = hit.transform.GetComponent<EnemyAttack>();
                    enemyScript.TakeDamage(damage, hit.point);
                


                Destroy(blood, timeOfBlood);
            }
            //In case of headshot the enemy should lose an extra ammount
            if(hit.collider.tag == "Head")
            {
                EnemyAttack enemyScript = hit.transform.GetComponentInParent<EnemyAttack>();
                bool dead= enemyScript.TakeDamage(damageHead, hit.point);
               
                //The alien ranged enemy should lose his head when killed with a headshot
                if (dead & hit.transform.GetComponentInParent<EnemyAttack>().isRanged == false)
                {
                    hit.transform.GetComponent<HeadShot>().dead();
                }
                else
                {
                    GameObject blood = Instantiate(BloodSystem, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(blood, timeOfBlood);
                }
            }
            //GameObject impageGO=Instantiate(impacteffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impageGO, 2);

        }
    }


    //Reload Routine which plays the animation needed and remove the ammo from the inventory and put it in the gun
    IEnumerator Reload()
    {
        IsReloading = true;
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("reloading", true);
      
        reload.Play();
        yield return new WaitForSeconds(reloadTime);
        IsReloading = false;
        animator.SetBool("reloading", false);
        int neededAmmo = maxAmmo - currentAmmo;
        if (neededAmmo < totalCurrentAmmo)
        {
            currentAmmo = maxAmmo;
            totalCurrentAmmo -= neededAmmo;
        }
        else
        {
            currentAmmo += totalCurrentAmmo;
            totalCurrentAmmo = 0;
        }
        AmoText.text = "" + currentAmmo + "/" + totalCurrentAmmo;
    }


    //methods used as events in animation to time the sounds with the pistol movement
    IEnumerator WeaponEffects()
    {
        muzzleflash.Play();
        audioSource.Play();
        yield return new WaitForEndOfFrame();
        muzzleflash.Stop();

    }
    //methods used as events in animation to time the sounds with the pistol movement
    IEnumerator EjectCasing()
    {
        bulletCasing.Play();
        yield return new WaitForEndOfFrame();
        bulletCasing.Stop();


    }
    //Method called when an ammo box is picked
    public void getAmmo(int ammo)
    {
        totalCurrentAmmo += ammo;
        if (totalCurrentAmmo > totalMaxAmmo)
            totalCurrentAmmo = totalMaxAmmo;
        if(active == true)
        AmoText.text = "" + currentAmmo + "/" + totalCurrentAmmo;
    }
    //Restart the gun inventory
    public void Restart()
    {
        currentAmmo = maxAmmo;
        totalCurrentAmmo = startTotalAmo;
    }
}
