using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class Manager : MonoBehaviour
{
 

    public int Granularity = 50; // how many frames to wait until you re-calculate the FPS
    List<double> times;
    int counter = 5;

    private int fpsint;
    
    public bool running = true; 
    public List<int> _frameRates;
    // Start is called before the first frame update
    void Start()
    {

        times = new List<double>();
    }

    // Update is called once per frame
    void Update()
    {
        if (counter <= 0)
        {
            CalcFPS ();
            counter = Granularity;
        } 

        times.Add (Time.deltaTime);
        counter--; 
    }
    
    public void CalcFPS ()
    {
        double sum = 0;
        foreach (double F in times)
        {
            sum += F;
        }

        double average = sum / times.Count;
        double fps = 1/average;
        
        times.Clear();

        fpsint = Convert.ToInt32(fps);
        _frameRates.Add(fpsint);
        // update a GUIText or something
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

