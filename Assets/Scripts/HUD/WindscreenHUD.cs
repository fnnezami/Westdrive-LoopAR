using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindscreenHUD : MonoBehaviour
{
    public Text Speed;
    public Text MaxSpeed;
    public Text Date;
    public Text Weather;
    public GameObject NonEventAnzeigen;
    public GameObject EventAnzeigen;
    public Image SpeedGauge;
    public GameObject linkeSeite;
    public GameObject rechteSeite;
    public float speedLimit = 70;
    private float nextUpdate = 1;
    public bool Event;
    private float speed;
    public GameObject KameraHalt;
    List<GameObject> currentCollider = new List<GameObject>();
    List<string> objectLinks = new List<string>();
    List<string> objectRechts = new List<string>();
    private CarController _carController;
    private AimedSpeed _aimedSpeed;
    private CriticalEventController _EventController;
    private ManualController _manualController;
    public GameObject KreisFuerEvents;
    private GameObject Kreis;
    private GameObject other;
    private GameObject TestAccident;
    int m_Event;
    private GameObject TestAuto;
    private bool EventActive = false;
    private Collider m_Collider;
    private int p = 0;
    void Start()
    {
        PlayerPrefs.SetInt("Event", 0);
        TestAuto = GameObject.Find("CarBody");
        NonEventAnzeigen.SetActive(true);
        EventAnzeigen.SetActive(false);
        _carController = TestAuto.GetComponent<CarController>();
        _aimedSpeed = TestAuto.GetComponent<AimedSpeed>();
        _EventController = GameObject.Find("MvPCriticalTrafficEvents").transform.GetChild(p).GetComponent<CriticalEventController>();
        _manualController = TestAuto.GetComponent<ManualController>();
        m_Collider = GameObject.Find("Starting Trigger").transform.GetChild(p).GetComponent<BoxCollider>();
       
    }
    public void EventStart()
    {

        
        if (m_Collider.enabled)
        {
            m_Collider.enabled = !m_Collider.enabled;
        }


        NonEventAnzeigen.SetActive(false);
        EventAnzeigen.SetActive(true);
        TestAccident = _EventController.GetTransform();
        other = GameObject.Find("Center").gameObject;
        


        //foreach (Transform child in TestAccident.transform)
        //{
            //other = child.gameObject;
            if (currentCollider.Contains(other.gameObject) == false)
            {
                currentCollider.Add(other.gameObject);
                Kreis = Instantiate(KreisFuerEvents);
                Kreis.transform.SetParent(other.gameObject.transform, false);
            }
        //}



        foreach (GameObject gObject in currentCollider)
        {
            Vector3 direction = gObject.transform.position - KameraHalt.transform.position;
            //Vector3 direction_arrow = KameraHalt.transform.forward;
            RaycastHit hit;
            RaycastHit hit_arrow;

            // Does the ray intersect any objects  and object has a child
            if (Physics.Raycast(KameraHalt.transform.position, direction, out hit, Mathf.Infinity) && (gObject.transform.childCount > 0))
            {
                switch (hit.collider.gameObject.name)
                {
                    //make sure that Event collider layer is on IgnoreRaycast!!!
                    case "EventAnzeigen":
                        gObject.transform.GetChild(0).gameObject.SetActive(true);
                        float dist = Vector3.Distance(gObject.transform.position, hit.point);
                        float quot = (5 / dist) + 1;
                        //set Kreis position and rotation
                        Kreis = gObject.transform.Find("Magical_Circle(Clone)").gameObject;
                        Kreis.transform.position = hit.point;
                        Kreis.transform.localScale = new Vector3(0.03f * quot, 0.03f * quot, 1f);
                        Kreis.transform.rotation = Quaternion.Euler(hit.collider.gameObject.transform.rotation.x, hit.collider.gameObject.transform.rotation.y,
                         hit.collider.gameObject.transform.rotation.z);


                        //Wenn das object vor dir ist, ist es nicht hinter dir
                        if (objectLinks.Count == 0) { linkeSeite.SetActive(false); }
                        if (objectRechts.Count == 0) { rechteSeite.SetActive(false); }
                        if (objectLinks.Contains(gObject.gameObject.name) == true)
                        {
                            objectLinks.Remove(gObject.gameObject.name);
                        }
                        if (objectRechts.Contains(gObject.gameObject.name) == true)
                        {
                            objectRechts.Remove(gObject.gameObject.name);
                        }

                        break;
                    case "linkeSeite":
                        gObject.transform.GetChild(0).gameObject.SetActive(false);

                        if (objectLinks.Contains(gObject.gameObject.name) == false)
                        {
                            objectLinks.Add(gObject.gameObject.name);
                        }
                        if (objectLinks.Count > 0)
                        {
                            linkeSeite.SetActive(true);
                        }
                        break;
                    case "rechteSeite":
                        gObject.transform.GetChild(0).gameObject.SetActive(false);
                        //Debug.Log("Rechts ist was vor der Liste");
                        if (objectRechts.Contains(gObject.gameObject.name) == false)
                        {
                            objectRechts.Add(gObject.gameObject.name);
                            //Debug.Log(objectRechts+ "   neues Object rechts"+ gObject.gameObject.name);
                        }
                        if (objectRechts.Count > 0)
                        {
                            rechteSeite.SetActive(true);
                            //Debug.Log("Rechts ist was");
                        }

                        break;
                    default:

                        if (objectLinks.Count == 0) { linkeSeite.SetActive(false); }
                        if (objectRechts.Count == 0) { rechteSeite.SetActive(false); }

                        break;
                }
            }


            else
            {
                if (gObject.transform.childCount > 0)
                {
                    gObject.transform.GetChild(0).gameObject.SetActive(false);

                }
            }
            //Debug.DrawRay(KameraHalt.transform.position, KameraHalt.transform.forward * hit.distance * 20, Color.yellow);
            Debug.DrawRay(KameraHalt.transform.position, direction * hit.distance * 20, Color.green);
            //Debug.DrawRay(KameraHalt.transform.position, Vector3.LerpUnclamped(KameraHalt.transform.forward, direction, 0.015f) * hit.distance * 20, Color.red);
        }
    }


    private void EventEnde()
    {
        if (!m_Collider.enabled)
        {
            m_Collider.enabled = m_Collider.enabled;
        }
        if (Time.time >= nextUpdate)
        {

            // Change the next update (current second+1)
            nextUpdate = (Mathf.FloorToInt(Time.time) + 1);
            // Call your function
            UpdateEverySecond();
        }
        if (currentCollider.Count > 0)
        {

            // Remove the GameObject collided with from the list.
            currentCollider.Remove(other.gameObject);
            if (other.gameObject.transform.childCount > 0)
            {
                // we have children!
                //Destroy(other.transform.Find("Magical_Circle(Clone)").gameObject);
                if (objectLinks.Count == 0) { linkeSeite.SetActive(false); }
                if (objectRechts.Count == 0) { rechteSeite.SetActive(false); }

                //not any more 
            }
        }
    }


    // Update is called once per frame
    void LateUpdate()
    {
        m_Event = PlayerPrefs.GetInt("Event", 0);


        if (m_Event == 1)
        {
            EventActive = true;
        }
        else
        {
            EventActive = false;
        }


        if (EventActive == true)
        {
            EventStart();
        }
        else
        {
            EventEnde();
        }

    }


    // Update is called once per second
    void UpdateEverySecond()
    {
        //Datum
        var today = System.DateTime.Now;
        Date.text = today.ToString("HH:mm");
        //MaxSpeed
        
        Weather.text = "Westbrueck \n 22°C";

        NonEventAnzeigen.SetActive(true);
        linkeSeite.SetActive(false);
        rechteSeite.SetActive(false);
        speed = Mathf.Round(_carController.GetCurrentSpeedInKmH());
        speedLimit = Mathf.Round(_aimedSpeed.GetAimedSpeed() * 3.6f);
        MaxSpeed.text = speedLimit + "";
        Speed.text = speed + "";
        float quotientSpeed = speed / speedLimit;
        SpeedGauge.fillAmount = 0.75f * (Mathf.Round(quotientSpeed * 36) / 36);


        if (speed >= speedLimit - 5)
        {
            SpeedGauge.color = Color.red;
            Speed.color = Color.red;
            MaxSpeed.color = Color.red;
        }
        else
        {
            SpeedGauge.color = Color.green;
            Speed.color = Color.green;
            MaxSpeed.color = Color.white;
        }



    }
}