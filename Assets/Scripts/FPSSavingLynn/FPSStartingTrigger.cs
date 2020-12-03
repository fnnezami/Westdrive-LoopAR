using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSStartingTrigger : MonoBehaviour
{
    public string scene;
    private void OnTriggerEnter(Collider other)
    {
        //finds the script SecondFPS attached to a GameObject
        GameObject fps = GameObject.Find("FPSManager");
        SecondFPS fpsScript = fps.GetComponent<SecondFPS>();
        
        //Clears the data for the next recording
        fpsScript.times.Clear();
        fpsScript._frameRates.Clear();
        
        //starts the recording
        fpsScript.running = true;
        
        //passes the scene name
        fpsScript.scene = scene;
        

    }
}
