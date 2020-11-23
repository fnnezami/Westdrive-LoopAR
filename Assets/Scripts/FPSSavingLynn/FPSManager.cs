using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class FPSManager : MonoBehaviour
{
    private int frameCount = 0;
    private double  dt = 0.0;
    private double  fps = 0.00;
    private double updateRate = 1;  // 4 updates per sec.
    private int fpsint = 0;

    private int currentfps;
    
    private double nextUpdate= 0.0;

    public bool running = true; 
    public List<int> _frameRates;
    // Start is called before the first frame update
    void Start()
    {
        nextUpdate = Time.time;
        _frameRates = new List<int>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (running == true)
        {
            frameCount++;
            dt += Time.deltaTime;
            
            
            // If the next update is reached
            if(Time.time>=nextUpdate){
                // Change the next update (current second+1)
                nextUpdate=Mathf.FloorToInt(Time.time)+1;
                // Call your fonction
                UpdateEverySecond();
            }  
        }
        
    }

    public void UpdateEverySecond()
    {

        currentfps = CalculateFPS();
        _frameRates.Add(currentfps);

    }

    public int CalculateFPS()
    {
       
        if (dt > 1.0/updateRate) 
        {
            fps = frameCount / dt ;
            frameCount = 0;
            dt -= 1.0/updateRate; 
        }

        fpsint = Convert.ToInt32(fps);
        return fpsint;
    }

    public void Save()
    {
        string filePath = getPath();
        


        using (StreamWriter writer = new StreamWriter(filePath))
        {
           
        
            for (int i = 0; i < Mathf.Max(_frameRates.Count); ++i)
            {
                if (i < _frameRates.Count) writer.Write(_frameRates[i]);
                writer.Write(",");
            } 
        }

         
    }
    private string getPath()
    {
        #if UNITY_EDITOR
                return Application.dataPath + "/Data/" + "Saved_FPS.csv";
                //"Participant " + "   " + DateTime.Now.ToString("dd-MM-yy   hh-mm-ss") + ".txt";
        #elif UNITY_ANDROID
                            return Application.persistentDataPath+"Saved_Inventory.csv";
        #elif UNITY_IPHONE
                            return Application.persistentDataPath+"/"+"Saved_Inventory.csv";
        #else
                            return Application.dataPath +"/"+"Saved_Inventory.csv";
        #endif
    }
}
