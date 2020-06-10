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
    public AudioClip WarningVerbalSound,WarningSound;
    public RawImage YouDriving;
    public GameObject highlightSymbol;

    [Header("No Event")]
    public Text Speed;
    public Text MaxSpeed,Date, Weather;
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
    public bool TimeShow,SpeedShow,SpeedLimitShow;    
    public float TimeTillWarningSign;     
    public float BlinkingFrequence;
    public bool BlinkingText;
    public bool BlinkingTriangle;    
    public float TimeTillSound;   
    public float TimeTillWarningSound;       
    [SerializeField]private List<GameObject> _eventObjectsToMark;
    [SerializeField]private List<GameObject> _highlightedObjects;
    [SerializeField]private GameObject _objectToHighlight;

    [Header("No Event")]
    public bool NoEvent;
    [SerializeField] private int speedLimit;
    
    [SerializeField] private float nextUpdate = 1;
    
    [SerializeField] private float speed;

    public float TorBackBlinkingFrequency;
    public bool TorBackBlinkingText,TorBackBlinkingImage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
