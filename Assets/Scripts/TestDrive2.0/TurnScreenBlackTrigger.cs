using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnScreenBlackTrigger : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private Camera cam;

    [SerializeField] private float time;

    private void Awake()
    {
        cam = camera.GetComponent<Camera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CarController>())
        {
            TurnScreenBlack(time);
        }
    }

    private IEnumerator TurnScreenBlack(float time)
    {
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.cullingMask = Cu
        
    }
}
