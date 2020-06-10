using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] private TestEventManager testEventManager;
    [SerializeField] private int secondsTillManualControl;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            
          //other.gameObject.GetComponent<ManualController>().enabled = false;
          //testEventManager.EndTrigger(other);
          //Debug.Log("Before waiting...");
          //yield return new WaitForSeconds(secondsTillManualControl);
          //Debug.Log("After waiting...");
          //other.gameObject.GetComponent<ManualController>().enabled = true;
          //GetComponent<BoxCollider>().enabled = false;
          //StartCoroutine(Triggered(other));
          StartCoroutine(Triggered(other));

        }
    }

    private IEnumerator Triggered(Collider other)
    {
        //other.gameObject.GetComponent<ManualController>().enabled = false; 
        testEventManager.EndTrigger(other); 
        Debug.Log("Before waiting End...");
        yield return new WaitForSecondsRealtime(secondsTillManualControl);
        Debug.Log("After waiting End...");
        GetComponent<BoxCollider>().enabled = false;
        //other.gameObject.GetComponent<ManualController>().enabled = true;
        testEventManager.ActivateHUD();
        GetComponent<BoxCollider>().enabled = false;
        Debug.Log("Does he make it here?");
        
    }
}
