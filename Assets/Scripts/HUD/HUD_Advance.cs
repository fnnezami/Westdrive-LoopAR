using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Advance : MonoBehaviour
{
    [Header("Event")]
    [SerializeField]
    [Header("Pre Experiment")]


    private AudioSource audioSource;
    public RawImage WarningTriangle;
    public Text WarningText;
    public AudioClip WarningVerbalSound, WarningSound;
    public RawImage YouDriving;
    public GameObject highlightSymbol;

    [Header("No Event")]
    public Text Speed;
    public Text MaxSpeed, Date, Weather;
    public Image SpeedGauge;
    public RawImage Kreis;
    public RawImage AIDriving;

    [Header("Event End")]
    public RawImage TorBackSign;
    public Text TorBackText;
    public AudioClip TorBackSound;
    [Header("Classes")]
    public AimedSpeed _aimedSpeed;
    public CarController _carController;

    [Space]
    [Header("Event")]
    [Header("Experiment")]

    public bool IsEvent;
    public bool TimeShow, SpeedShow, SpeedLimitShow;
    public float TimeTillWarningSign;
    public float BlinkingFrequence;
    public float BlinkingForTime;
    public bool BlinkingText;
    public bool BlinkingTriangle;
    public float TimeTillSound;
    public float TimeTillWarningSound;
    [SerializeField] private List<GameObject> _eventObjectsToMark;
    [SerializeField] private List<GameObject> _highlightedObjects;

    [Header("No Event AI Drive")]
    [SerializeField] private int speedLimit;

    [SerializeField] private float nextUpdate = 1;

    [SerializeField] private float speed;
    private int EventCount;
    public float TorBackBlinkingFrequency;
    public float TorBackBlinkingLength;
    public bool TorBackBlinkingText, TorBackBlinkingImage;
    [Header("No Event Manual Drive")]
    public bool ManualDriving;
    private IEnumerator coroutine;
    private float BlinkFreq;
    private float BlinkLength;
    private ArrayList TorObjectsList = new ArrayList();
    private ArrayList NonEventDisplay = new ArrayList();
    private ArrayList EventDisplay = new ArrayList();



    // Start is called before the first frame update
    void Start()
    {
        _aimedSpeed = _carController.gameObject.GetComponent<AimedSpeed>();
        if (!IsEvent)
        {
            WarningText.enabled = false;
            WarningTriangle.enabled = false;
            TorBackSign.enabled = false;
            TorBackText.enabled = false;
            if (!ManualDriving)
            {
                AIDrive();
            }
            else
            {
                ManualDrive();
            }
            //Debug.Log("Dreieck Aus");
        }

    }
    public void ActivateHUD(GameObject testAccidentObject)
    {
        List<GameObject> ObjectToMark = new List<GameObject>();
        ObjectToMark.Add(testAccidentObject);
        ActivateHUD(ObjectToMark);

    }

    public void ActivateHUD(List<GameObject> testAccidentSubjects)
    {
        _eventObjectsToMark = testAccidentSubjects;
        MarkObjects();
    }

    private void MarkObjects()
    {
        foreach (var eventObject in _eventObjectsToMark)
        {
            GameObject clone = Instantiate(highlightSymbol, eventObject.transform);
            clone.transform.localPosition = Vector3.RotateTowards(transform.forward, Camera.main.transform.position, 0, 0.0f);
            _highlightedObjects.Add(clone);
        }
    }
    public void DeactivateHUD()
    {
        IsEvent = false;
        _eventObjectsToMark.Clear();
        _highlightedObjects.Clear();
        if (!ManualDriving)
        {
            AIDrive();
        }
        else
        {
            ManualDrive();
        }
    }

    public void DriverAlert()
    {
        IsEvent = true;
        EventDrive();

    }
    public void AIDrive()
    {
        //Take over request back Image && Text && Sound -> maybe Blinking 
        //start NonEventDisplays
        //start AI DrivingSign

        Debug.Log("Aidrive start");

        if (TorBackBlinkingImage || TorBackBlinkingText)
        {
            BlinkFreq = TorBackBlinkingFrequency;
            BlinkLength = TorBackBlinkingLength;
            coroutine = Blink(BlinkFreq, BlinkLength);
            StartCoroutine(Blink(BlinkFreq, BlinkLength));

        }


        Date.enabled = true;
        Speed.enabled = true;
        SpeedGauge.enabled = true;
        MaxSpeed.enabled = true;
        Kreis.enabled = true;
        Weather.enabled = true;




    }
    public void ManualDrive()
    {        //You are driving
        //start NonEventDisplays
        //No TORBack no Sound 
    }
    public void EventDrive()
    {
        if (TimeShow) Date.enabled = false;
        if (SpeedShow) Speed.enabled = false;
        if (SpeedShow) SpeedGauge.enabled = false;
        if (SpeedLimitShow) MaxSpeed.enabled = false;
        if (SpeedLimitShow) Kreis.enabled = false;
        Weather.enabled = false;
        //Warning Sound && Triangle && Text && Blinking
        //Verbal Warning
        //
        if (BlinkingText || BlinkingTriangle)
        {
            BlinkFreq = BlinkingFrequence;
            BlinkLength = BlinkingForTime;
            coroutine = Blink(BlinkFreq, BlinkLength);
            StartCoroutine(Blink(BlinkFreq, BlinkLength));
        }

    }
    private IEnumerator Blink(float BlinkFreq, float BlinkLength)
    {
        //Debug.Log("Basd"+BlinkFreq + " "+ BlinkLength);
        //float EndTime = Time.time + BlinkLength;
        int i = 0;
        float EndI = BlinkFreq * BlinkLength;
        while (i < EndI)
        {
            Debug.Log("in der Zeitschleife " );
            if (IsEvent)
            {

                if (BlinkingText)
                {
                    WarningText.enabled = true;
                }
                if (BlinkingTriangle) WarningTriangle.enabled = true;

            }
            else
            {
                yield return new WaitForSeconds(3f);
                TorBackSign.enabled = true;
                TorBackText.enabled = true;

            }
            i++;
            yield return new WaitForSeconds(0.2f);

            Debug.Log(i);
            if (IsEvent)
            {
                WarningText.enabled = false;
                WarningTriangle.enabled = false;

            }
            else
            {
                Debug.Log(Time.time + " Time "  + " " + BlinkFreq);

                Debug.Log(" WarningText");
                TorBackSign.enabled = false;
                TorBackText.enabled = false;
            }
            yield return new WaitForSeconds(0.2f);
            //yield return null;

        }
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if (IsEvent)
        {
            //NonEventAnzeigen.SetActive(true);
        }
        else
        {

            //NonEventAnzeigen.SetActive(false);
        }
        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = (Mathf.FloorToInt(Time.time) + 1);
            // Call your function
            UpdateEverySecond();
        }

        // Update is called once per second
        void UpdateEverySecond()
        {
            speed = Mathf.Round(_carController.GetCurrentSpeedInKmH());
            Speed.text = speed + "";
            //speedLimit = Mathf.RoundToInt(_aimedSpeed.GetAimedSpeed());
            speedLimit = Mathf.RoundToInt(_aimedSpeed.GetAimedSpeed() * 3.6f);
            float quotientSpeed = speed / speedLimit;
            SpeedGauge.fillAmount = 0.75f * (Mathf.Round(quotientSpeed * 36) / 36);


            if (speed >= (speedLimit + 5))
            {
                SpeedGauge.color = Color.red;
                Speed.color = Color.red;
                //MaxSpeed.color = Color.red;
            }
            else
            {
                SpeedGauge.color = Color.green;
                Speed.color = Color.green;
                MaxSpeed.color = Color.white;
            }

            //Datum
            var today = System.DateTime.Now;
            Date.text = today.ToString("HH:mm");
            //Date.text = "13:22";
            //MaxSpeed
            MaxSpeed.text = speedLimit + "";
            Weather.text = "Westbrueck \n 22°C";
        }
    }
}
