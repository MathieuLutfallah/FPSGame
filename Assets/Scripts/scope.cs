using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

//Class used to increase the field of view of the camera when scope button is pressed
public class scope : MonoBehaviour
{
    public Animator animator;
    public bool isScoped = false;
    public GameObject dot;
    public GameObject scopeImage;
    public GameObject weaponCamera;
    public GameObject Player;
    public Camera mainCamera;
    public float normalFOV;
    public float scopedFOV;
    public GameObject Minimap;
    int currentGun;


    void Start()
    {
        
        normalFOV = 60f;
        currentGun = 1;
    }
  
    void Update()
    {
        
        if (Input.GetButtonDown("Fire2") &&currentGun==2)
        {
            isScoped = !isScoped;
            //Debug.Log("scoped");
            animator.SetBool("isScoped", isScoped);
            dot.SetActive(!isScoped);

            if (isScoped)
                StartCoroutine(OnScoped());
            else
                OnUnscoped();

        }
    }
    //Start the aniation which rise the gun and make it approch the player eye, then sets the scope image while increasing the field of view
    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(0.15f);
        Minimap.SetActive(false);
        //Player.GetComponent<FirstPersonController>().m_UseFovKick = false;
        scopeImage.SetActive(true);
        weaponCamera.SetActive(false);
        Debug.Log("Scoped");
        //normalFOV = mainCamera.fieldOfView;
        mainCamera.fieldOfView = scopedFOV;
        Debug.Log("camera scope" + mainCamera.fieldOfView);
    }
    void OnUnscoped()
    {
        Minimap.SetActive(true);
        //Player.GetComponent<FirstPersonController>().m_UseFovKick = true;
        scopeImage.SetActive(false);
        weaponCamera.SetActive(true);
        mainCamera.fieldOfView = normalFOV;
    }

    public void setCurrentGun(int currentGun)
    {
        this.currentGun = currentGun;
    }
}

