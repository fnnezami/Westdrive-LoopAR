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
 

    public int Granularity = 40; // how many frames to wait until you re-calculate the FPS
    List<double> times; //List of times to average over
    int counter = 40;

    private int fpsint;
    
    public bool running; //checks if the script should collect data or not 
    public List<int> _frameRates; // List of all recorded frame rates
    
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = -1;
        times = new List<double>();
    }

    // Update is called once per frame
    void Update()
    {
        if (running) 
        {
            if (counter <= 0)
            {
                CalcFPS (); 
                counter = Granularity; // resets the counter 
            } 

            times.Add (Time.deltaTime); // adds the completion time in seconds since the last frame to a list
            counter--; 
        }

    }
    
    //Calculates an average fps from the last frames 
    public void CalcFPS ()
    {
        double sum = 0;
        foreach (double F in times)
        {
            sum += F;
        }

        double average = sum / times.Count; //averages the needed time to complete one frame
        double fps = 1/average; //calculation from time needed to fps
        
        times.Clear(); //clears

        fpsint = Convert.ToInt32(fps);
        _frameRates.Add(fpsint);
        // update a GUIText or something
    }


    //saves the collected frames into a csv file
    public void Save()
    {
        string filePath = getPath();
        


        using (StreamWriter writer = new StreamWriter(filePath))
        {
            int sum2 = 0;
            foreach (int I in _frameRates)
            {
                sum2 += I;
            }

            int fpsall = 0;
            fpsall = sum2 / _frameRates.Count;
            
        
            for (int i = 0; i < Mathf.Max(_frameRates.Count); ++i)
            {
                if (i < _frameRates.Count) writer.Write(_frameRates[i]);
                writer.Write(",");
            } 
            
            writer.WriteLine();
            writer.Write(fpsall);
        }

         
    }
    
    //get the path where to save the collected fps
    private string getPath()
    {
        #if UNITY_EDITOR
                            return Application.dataPath + "/Data/" + "Saved_FPS.csv";
                
        #elif UNITY_ANDROID
                            return Application.persistentDataPath+"Saved_FPS.csv";
        #elif UNITY_IPHONE
                            return Application.persistentDataPath+"/"+"Saved_FPS.csv";
        #else
                            return Application.dataPath +"/"+"Saved_FPS.csv";
        #endif
    }
}

