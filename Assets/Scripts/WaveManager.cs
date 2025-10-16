using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

//Event based class which informs all the elements of the game that the wave has ended
public class WaveManager : MonoBehaviour
{
    public static WaveManager current;

 
    private void Awake()
    {
        current = this;
     
    }

    public event Action roundEnded;

    public void RoundEnded()
    {
        if (roundEnded != null)
        {
            
            roundEnded();
        }
    }


}
