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
    [SerializeField] private GameObject _objectToHighlight;

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
        CreateLists();

    }
    public void CreateLists()
    {
        //First Take Over Request Objects(TorObjects)
        TorObjectsList.Add(TorBackSign);
        TorObjectsList.Add(TorBackText);
        //NonEvent Displayed Objects are not displayed in case of event
        if (!ManualDriving)
        {
            NonEventDisplay.Add(AIDriving);
        }
        if (!TimeShow)
        {
            NonEventDisplay.Add(Date);
        }
        if (!SpeedShow)
        {
            NonEventDisplay.Add(Speed);
        }
        if (SpeedLimitShow)
        {
            NonEventDisplay.Add(MaxSpeed);
        }
        //EvenDisplays are not Displayed in case of no Event
        EventDisplay.Add(WarningText);
        EventDisplay.Add(WarningTriangle);
    }
    public void DeactivateHUD()
    {
        IsEvent = false;
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
        BlinkFreq = TorBackBlinkingFrequency;
        BlinkLength = TorBackBlinkingLength;
        coroutine = Blink(BlinkFreq, BlinkLength, TorObjectsList);


    }
    public void ManualDrive()
    {
        //You are driving
        //start NonEventDisplays
        //No TORBack no Sound 


    }
    public void EventDrive()
    {
        //Warning Sound && Triangle && Text && Blinking
        //Verbal Warning
        //
        BlinkFreq = BlinkingFrequence;
        BlinkLength = BlinkingForTime;
        coroutine = Blink(BlinkFreq, BlinkLength, EventDisplay);

    }
    private IEnumerator Blink(float BlinkFreq, float BlinkLength,ArrayList Displays)
    {
        float EndTime = Time.time + BlinkLength;
        while (Time.time < EndTime)
        {
            foreach (GameObject obj in Displays)
            {
                obj.SetActive(true);
            }
            yield return new WaitForSeconds(1f / BlinkFreq);
            foreach (GameObject obj in Displays)
            {
                obj.SetActive(false);
            }
            yield return new WaitForSeconds(1f / BlinkFreq);

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
