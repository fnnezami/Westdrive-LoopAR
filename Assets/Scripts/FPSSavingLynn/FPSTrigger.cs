using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class FPSTrigger : MonoBehaviour
{
    public List<int> myframeRates;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
        GameObject fps = GameObject.Find("FPSManager");
        Manager fpsScript = fps.GetComponent<Manager>();
        myframeRates = fpsScript._frameRates;
        

        fpsScript.running = false;
        
        fpsScript.Save();

    } 
}
