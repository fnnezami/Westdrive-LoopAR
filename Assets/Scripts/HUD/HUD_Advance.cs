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


    public AudioSource audioSource;
    public AudioSource audioSource2;

    public RawImage WarningTriangle;
    public Text WarningText;
    public AudioClip WarningVerbalSound, WarningSound;
    public RawImage YouDriving;
    public Text YouDrivingText;
    public GameObject highlightSymbol;

    [Header("No Event")]
    public Text Speed;
    public Text MaxSpeed, Date, Weather;
    public Image SpeedGauge;
    public RawImage Circle;
    private bool AIDrivingBool = false;
    public RawImage AIDriving;
    public Text AIDrivingText;

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
    public float BlinkingFrequence = 3;
    public float BlinkingForTime = 2;
    public bool BlinkingText;
    public bool BlinkingTriangle;
    public float TimeTillWarningVoice;
    public float TimeTillWarningSound, WarningSignDuration = 2;
    [SerializeField] private List<GameObject> _eventObjectsToMark;
    [SerializeField] private List<GameObject> _highlightedObjects;

    [Header("No Event AI Drive")]
    [SerializeField] private int speedLimit;

    [SerializeField] private float nextUpdate = 1;

    [SerializeField] private float speed;
    public float TorBackBlinkingFrequency = 3;
    public float TorBackBlinkingLength = 2, TorBackDuration = 2;
    public bool TorBackBlinkingText, TorBackBlinkingImage;
    [Header("No Event Manual Drive")]
    public bool ManualDriving;
    private float BlinkFreq;
    private float BlinkLength;

    // Start is called before the first frame update
    void Start()
    {
        _aimedSpeed = _carController.gameObject.GetComponent<AimedSpeed>();
        WarningText.enabled = false;
        WarningTriangle.enabled = false;
        TorBackSign.enabled = false;
        TorBackText.enabled = false;
        if (!IsEvent)
        {
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
            var outline = eventObject.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 10f;
            //GameObject clone = Instantiate(highlightSymbol, eventObject.transform);
            //clone.transform.localPosition = Vector3.RotateTowards(transform.forward, Camera.main.transform.position, 0, 0.0f);
            _highlightedObjects.Add(eventObject);
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
        AIDrivingBool = false;
        EventDrive();

    }
    public void AIDrive()
    {
        AIDrivingBool = true;
        YouDriving.enabled = false;
        YouDrivingText.enabled = false;
        //Take over request back Image && Text && Sound -> maybe Blinking 
        //start NonEventDisplays
        //start AI DrivingSign
        Debug.Log("Aidrive start");

        if (TorBackBlinkingImage || TorBackBlinkingText)
        {
            BlinkFreq = TorBackBlinkingFrequency;
            BlinkLength = TorBackBlinkingLength;
            StartCoroutine(Blink(BlinkFreq, BlinkLength));
        }
        else
        {
            StartCoroutine(ShowForSeconds(TorBackDuration));
        }
        StartCoroutine(SoundManagerTOR());
        StartCoroutine(ShowAfterSeconds());

        Date.enabled = true;
        Speed.enabled = true;
        SpeedGauge.enabled = true;
        MaxSpeed.enabled = true;
        Circle.enabled = true;
        Weather.enabled = true;
        //TorBackSign.enabled = false;
        //TorBackText.enabled = false;




    }
    public void ManualDrive()
    {        //You are driving
        ManualDriving = true;
        AIDrivingBool = false;

        StartCoroutine(ShowForSeconds(TorBackDuration));
        //start NonEventDisplays

        Date.enabled = true;
        Speed.enabled = true;
        SpeedGauge.enabled = true;
        MaxSpeed.enabled = true;
        Circle.enabled = true;
        Weather.enabled = true;
        AIDrivingText.enabled = false;
        AIDriving.enabled = false;
        //No TORBack no Sound 
        StartCoroutine(ShowAfterSeconds());
    }
    public void EventDrive()
    {
        if (!TimeShow) Date.enabled = false;
        if (!SpeedShow) Speed.enabled = false;
        if (!SpeedShow) SpeedGauge.enabled = false;
        if (!SpeedLimitShow) MaxSpeed.enabled = false;
        if (!SpeedLimitShow) Circle.enabled = false;
        Weather.enabled = false;

        AIDrivingText.enabled = false;
        AIDriving.enabled = false;
        //Warning Sound && Triangle && Text && Blinking
        //Verbal Warning
        //
        if (BlinkingText || BlinkingTriangle)
        {
            BlinkFreq = BlinkingFrequence;
            BlinkLength = BlinkingForTime;
            //coroutine = Blink(BlinkFreq, BlinkLength);
            StartCoroutine(Blink(BlinkFreq, BlinkLength));
        }
        else
        {
            StartCoroutine(ShowForSeconds(WarningSignDuration));
        }
        StartCoroutine(SoundManagerWarning());
        StartCoroutine(ShowAfterSeconds());

    }
    private IEnumerator SoundManagerTOR()
    {
        audioSource.Stop();
        audioSource2.Stop();
        yield return new WaitForSeconds(0.2f);
        audioSource.clip = TorBackSound;
        audioSource.Play();
        yield return null;
        StopCoroutine(SoundManagerTOR());
    }
    private IEnumerator SoundManagerWarning()
    {
        audioSource.clip = WarningSound;
        audioSource2.clip = WarningVerbalSound;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        audioSource2.Play();


        Debug.Log("musik");
        yield return new WaitForSeconds(1f);
        Debug.Log("Musik Ende");
        audioSource.Stop();
        audioSource2.Stop();
        StopCoroutine(SoundManagerWarning());
    }
    private IEnumerator ShowAfterSeconds()
    {
        yield return new WaitForSeconds(3f);
        if (IsEvent || ManualDriving)
        {

            YouDriving.enabled = true;
            YouDrivingText.enabled = true;
        }

        if (AIDrivingBool)
        {
            AIDrivingText.enabled = true;
            AIDriving.enabled = true;
        }
    }
    private IEnumerator ShowForSeconds(float TorBackDuration)
    {
        if (IsEvent)
        {
            yield return new WaitForSeconds(TimeTillWarningSign);
            WarningText.enabled = true;
            WarningTriangle.enabled = true;
            TorBackSign.enabled = false;
            TorBackText.enabled = false;
        }
        else
        {
            Debug.Log("TOR");
            WarningText.enabled = false;
            WarningTriangle.enabled = false;
            if (nextUpdate > 10)
            {
                TorBackSign.enabled = true;
                TorBackText.enabled = true;
            }

        }
        yield return new WaitForSeconds(TorBackDuration);
        WarningText.enabled = false;
        WarningTriangle.enabled = false;
        TorBackSign.enabled = false;
        TorBackText.enabled = false;


    }
    private IEnumerator Blink(float BlinkFreq, float BlinkLength)
    {
        WarningText.enabled = false;
        WarningTriangle.enabled = false;
        TorBackSign.enabled = false;
        TorBackText.enabled = false;
        int i = 0;
        float EndI = BlinkFreq * BlinkLength;
        while (i < EndI)
        {

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
                //yield return new WaitForSeconds(3f);
                TorBackSign.enabled = true;
                TorBackText.enabled = true;

            }
            i++;
            yield return new WaitForSeconds(1f / BlinkFreq);

            //Debug.Log(i);
            if (IsEvent)
            {
                WarningText.enabled = false;
                WarningTriangle.enabled = false;

            }
            else
            {
                TorBackSign.enabled = false;
                TorBackText.enabled = false;
            }
            yield return new WaitForSeconds(1f / BlinkFreq);
            //yield return null;

        }
        WarningText.enabled = false;
        WarningTriangle.enabled = false;
        TorBackSign.enabled = false;
        TorBackText.enabled = false;
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
