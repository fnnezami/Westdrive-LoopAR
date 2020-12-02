using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class SecondFPS : MonoBehaviour
{
 

    float elapsed = 0f;
    List<double> times; //List of times to average over
   

    private int fpsint;
    
    public bool running; //checks if the script should collect data or not 
    public List<int> _frameRates; // List of all recorded frame rates

    public string condition;
    
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
            times.Add (Time.deltaTime);
            elapsed += Time.deltaTime;
            if (elapsed >= 0.5f) {
                elapsed = elapsed % 0.5f;
                
                CalcFPS ();
            }
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
            
            writer.Write(condition);
            writer.Write(",");
            writer.Write(fpsall);
            writer.WriteLine();
            
            for (int i = 0; i < Mathf.Max(_frameRates.Count); ++i)
            {
                if (i < _frameRates.Count) writer.Write(_frameRates[i]);
                writer.WriteLine();
            } 
            
        }

         
    }
    
    //get the path where to save the collected fps
    private string getPath()
    {
        #if UNITY_EDITOR
                            return Application.dataPath + "/Data/" + condition + ".csv";
                
        #elif UNITY_ANDROID
                            return Application.persistentDataPath+"Saved_FPS.csv";
        #elif UNITY_IPHONE
                            return Application.persistentDataPath+"/"+"Saved_FPS.csv";
        #else
                            return Application.dataPath +"/"+"Saved_FPS.csv";
        #endif
    }
}
