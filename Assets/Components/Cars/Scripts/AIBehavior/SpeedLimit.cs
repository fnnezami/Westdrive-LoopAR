﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SpeedLimit : MonoBehaviour
{
    [Space]
    [Header("Speed Limit (0 - 100)")]
    [Range(0, 100)]
    public float speedLimit = 50f;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AIControler>() != null)
        {
            other.GetComponent<AIControler>().SetRuleSpeed(speedLimit/3.6f);
        }
    }
}
