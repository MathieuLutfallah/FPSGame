using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class playerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float jumpSpeed = 20.0f;
    public float gravity = 14.0f;

    public bool isGrounded;

    public float verticalVelocity;
    public float jumpForce = 10.0f;

    public GameObject pistol;
    public GameObject ar;
    private GameObject current;
    // Start is called before the first frame update
    void Start()
    {
        isGrounded = controller.isGrounded;
        current = pistol;
    }
   
    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        Vector3 move = Vector3.zero;


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * z;

        if (isGrounded)
        {

            if (Input.GetButton("Jump"))
            {
                move.y = jumpSpeed;
            }
        }
        else
            move.y -= gravity;



        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (current.name != "Pistol")
            {
                DisableCurrentGun();
                ActivatePistol();
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (current.name != "AssaultRifle")
            {
                DisableCurrentGun();
                ActivateAR();
            }
        }

    }


    void DisableCurrentGun()
    {
        current.SetActive(false);

    }

    void ActivatePistol()
    {
        pistol.SetActive(true);
        current = pistol;
    }

    void ActivateAR()
    {
        ar.SetActive(true);
        current = ar;
    }

}
