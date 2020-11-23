using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class SomethingNew : MonoBehaviour
{


    private int fpsint;
    
    float deltaTime = 0f;
    private int deltaTimeInt;

    public bool running = true; 
    public List<int> _frameRates;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        deltaTime += (Time.deltaTime - deltaTime) * .1f;
        float fps = 1.0f / deltaTime;

        deltaTimeInt = Convert.ToInt32(fps);
        _frameRates.Add(deltaTimeInt);
        
        
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
                return Application.dataPath + "/Data/" + "Saved_FPS2.csv";
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

