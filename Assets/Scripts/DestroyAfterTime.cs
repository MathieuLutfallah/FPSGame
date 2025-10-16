using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    //Destroy the objects after a certain time
    public float timeTolive;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, timeTolive);
    }

}
