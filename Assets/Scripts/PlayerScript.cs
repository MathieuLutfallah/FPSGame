using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Reflection;
using System;


//Script managing the health of the player as well as switching guns
public class PlayerScript : MonoBehaviour
{

    // Start is called before the first frame update
    public int maxhealth = 100;
    public int currentHealth;
    int currentGunNumber;
    public Text health;
    public GameObject pistol;
    public GameObject ar;
    private GameObject current;
    public GameObject WeaponHolder;
    bool reloading;
    public UIManager manager;

    public Image DamageImage;
    public Color damageColor;
    public float colorSmoothing;
    bool isTakingDamage;

    void Awake()
    {

        currentGunNumber = 1;
        currentHealth = maxhealth;
        health.text = ""+ currentHealth;
        current = pistol;
        reloading = false;
        DamageImage.color =Color.clear;
        isTakingDamage = false;
    }
    void Update()
    {
        //Show the blood scene around the UI
        if(isTakingDamage)
        {
            DamageImage.color = damageColor;

        }
        else
        {
            //Make the blood scene fade away as time goes after the player took damage
            DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, colorSmoothing * Time.deltaTime);
        }
        isTakingDamage = false;
        //Change guns based on the button pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            reloading = pistol.GetComponent<WeaponScript>().IsReloading || ar.GetComponent<WeaponScript>().IsReloading;
            if (current.name != "Pistol" && !reloading)
            {
                DisableCurrentGun();
                ActivatePistol();
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            reloading = pistol.GetComponent<WeaponScript>().IsReloading || ar.GetComponent<WeaponScript>().IsReloading;
            if (current.name != "AssaultRifle" && !reloading)
            {
                DisableCurrentGun();
                ActivateAR();
            }
        }
    }
   //reduce the player health once the enemy attacks it
    public void TakeDamage(int amount)
    {
        isTakingDamage = true;
        currentHealth -= amount;
        health.text = ""+currentHealth;
        if (currentHealth <= 0)
        {

            manager.EndGame();
        }
    }
    //Function used to switch weapons
    void DisableCurrentGun()
    {
        current.SetActive(false);

    }
    //Function used to switch weapons
    void ActivatePistol()
    {
        pistol.SetActive(true);
        current = pistol;
        currentGunNumber = 1; 
        WeaponHolder.GetComponent<scope>().setCurrentGun(currentGunNumber);
    }
    //Function used to switch weapons
    void ActivateAR()
    {
        ar.SetActive(true);
        current = ar;
        currentGunNumber = 2;
        WeaponHolder.GetComponent<scope>().setCurrentGun(currentGunNumber);
    }

    //restore the player health once the health box is picked up
    public void regen(int reg)
    {
        currentHealth += reg;
        if (currentHealth > maxhealth)
        {
            currentHealth = maxhealth;

        }
        health.text = "" + currentHealth;
    }
    //Restart the player health and first gun
    public void Restart()
    {
        currentHealth = maxhealth;
        DisableCurrentGun();
        ActivatePistol();
    }
}
