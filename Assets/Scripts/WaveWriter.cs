using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveWriter : MonoBehaviour
{
    
    
    int countWave;
    
    
    
    //Modify the text showing the wave number
    
    void Start()
    {
        this.GetComponent<Text>().text = "Wave 0";
        WaveManager.current.roundEnded += UpdateWave;
        countWave = 0;
    }

    private void UpdateWave()
    {
        countWave++;
        this.GetComponent<Text>().text = "Wave " + countWave+ ":";
        //Debug.Log("wave" + countWave);
    }
    
    public void restart()
    {
        this.GetComponent<Text>().text = "Wave 0";
        countWave = 0;
    }
}
