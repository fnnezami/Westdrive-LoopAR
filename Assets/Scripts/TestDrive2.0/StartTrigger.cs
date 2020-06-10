using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField] private int aimedSpeed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>())
        {
            StartCoroutine(Triggered(other));
        }
    }

    private IEnumerator Triggered(Collider other)
    {

        other.gameObject.GetComponent<ManualController>().enabled = false;
        Debug.Log("Before Waiting Start");
        yield return new WaitForSecondsRealtime(2);
        Debug.Log("After Waiting Start");

        other.gameObject.GetComponent<ManualController>().enabled = true;
        other.gameObject.GetComponent<AimedSpeed>().SetRuleSpeed(aimedSpeed);
        other.gameObject.GetComponent<AIController>().SetLocalTarget();
        other.gameObject.GetComponent<ControlSwitch>().SwitchControl(false);
        GetComponent<BoxCollider>().enabled = false;
    }

}
