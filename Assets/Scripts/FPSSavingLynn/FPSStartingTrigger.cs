using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSStartingTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject fps = GameObject.Find("FPSManager");
        SecondFPS fpsScript = fps.GetComponent<SecondFPS>();
        fpsScript.running = true;
    }
}
