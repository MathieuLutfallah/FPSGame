using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public Text pickUpText;
    public float pickDistance = 5f;
    public GameObject pistol;
    WeaponScript PistolWS;
    WeaponScript ARWS;
    AudioSource pickupSource;
    Camera mainCam;
    public AudioClip pickUPAC;
    public LayerMask layer;
    public GameObject AR;
    PlayerScript player;
    public int regeneration;
    private void Start()
    {
        player = GetComponent<PlayerScript>();
        pickupSource = GetComponent<AudioSource>();
        PistolWS = pistol.GetComponent <WeaponScript>();
        ARWS = AR.GetComponent<WeaponScript>();
        mainCam = Camera.main;
    }
    private void Update()
    {
        ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit, pickDistance, layer)){
            pickUpText.gameObject.SetActive(true);
            pickUpText.text = hit.transform.tag;
            if (hit.transform.tag == "PistolAmo")
            {
                PickUpPistolAmmo(10);
            }
            if (hit.transform.tag == "ARAmo")
            {
                PickUpARAmmo(25);
            }
            if (hit.transform.tag == "HealthKit")
            {
                HealthKit();
            }
        }
        else
        {
            pickUpText.gameObject.SetActive(false);
        }
    }
    void PickUpPistolAmmo(int ammo)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Destroy(hit.transform.gameObject);
            PistolWS.getAmmo(ammo);
            pickupSource.PlayOneShot(pickUPAC);
            pickUpText.gameObject.SetActive(false);
        }

    }
    void PickUpARAmmo(int ammo)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Destroy(hit.transform.gameObject);
            ARWS.getAmmo(ammo);
            pickupSource.PlayOneShot(pickUPAC);
            pickUpText.gameObject.SetActive(false);
        }

    }
    void HealthKit()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Destroy(hit.transform.gameObject);
            player.regen(regeneration);
            pickUpText.gameObject.SetActive(false);
        }
    }
}
