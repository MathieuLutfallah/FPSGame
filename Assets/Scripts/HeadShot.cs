using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script use to Remove the zombie head if he dies with a headshot and to initiate the blood particle effect
public class HeadShot : MonoBehaviour
{
    public GameObject head;
    public GameObject blood;

    public void dead()
    {
        head.SetActive(false);
        blood.SetActive(true);

    }

    public void stopblood()
    {
     blood.SetActive(false);
    }
    public void returnHead()
    {
        head.SetActive(true);
    }
}
