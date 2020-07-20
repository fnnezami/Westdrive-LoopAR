using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpDX;
using SharpDX.DirectInput;
using DeviceType = SharpDX.DirectInput.DeviceType;

public class DirectInputTest : MonoBehaviour
{
    private DirectInput _directInput;
    // Start is called before the first frame update
    void Start()
    {
        _directInput= new DirectInput();

        _directInput.GetDevices();

        IList<DeviceInstance> devices = new List<DeviceInstance>();

        devices = _directInput.GetDevices();
        
        
        foreach (var device in devices)
        {
            if(device.Type == DeviceType.Driving) 
                Debug.Log(device.ProductName  +" usage "+  device.Usage + " type " + device.Type);
        }
        
        
        

    }

    // Update is called once per frame
    void Update()
    {

       
    }
}
