using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using SharpDX;
using SharpDX.DirectInput;
using UnityEditor;
using ConstantForce = SharpDX.DirectInput.ConstantForce;
using DeviceType = SharpDX.DirectInput.DeviceType;





public class DirectInputTest : MonoBehaviour
{
    
    [DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();
     
    public static System.IntPtr GetWindowHandle() {
        return GetActiveWindow();
    }
    
    private DirectInput _directInput;
    // Start is called before the first frame update
    
    
    private Joystick joystick0;
    private Joystick joystick1;



    private EffectParameters ep = new EffectParameters();
    private ConstantForce CF = new ConstantForce();

    private Effect e;
    void Start()
    {
        _directInput= new DirectInput();
        
            IList<DeviceInstance> _deviceList = new List<DeviceInstance>();

            _deviceList = _directInput.GetDevices(DeviceType.Driving, DeviceEnumerationFlags.ForceFeedback); // we reduce our input just to driving devices which provide Force feedback
            
            
            // interating through the devices to see if everything is up and running
            
            foreach (var device in _deviceList)
            {
                Debug.Log(device.ProductName + " " + device.InstanceGuid + " " +  device.Usage);
            }
        
            // we have two steering wheels , actually only 0 seems be manipulatable. We stick with 0
            joystick0 = new Joystick(_directInput, _deviceList[0].InstanceGuid);    
            joystick1 = new Joystick(_directInput, _deviceList[1].InstanceGuid);
        
            //heres come the test effects parameters... we need to set those paramaters effect for Force Feedback
            
            
            ep = new EffectParameters();

            ep.Flags = EffectFlags.Cartesian | EffectFlags.ObjectOffsets;
            ep.Directions = new int[1] { 0 };
            ep.Gain = 5000;
            ep.Duration = int.MaxValue;
            ep.SamplePeriod = joystick0.Capabilities.ForceFeedbackSamplePeriod;
            CF.Magnitude = 10000;
            ep.Parameters = CF;
        
            EffectFile eff= new EffectFile();
    
            eff.Parameters = ep;
            
            
            
            
            // var pointer = joystick1.NativePointer;  //unfortunately this didnt worked for the window handle
            
            // the device cooperative level basically  just identifies, if the device is receiving input when the window is not active.
            // we needed to create this GetActiveWindow Handle for this
            joystick0.SetCooperativeLevel(GetActiveWindow(), CooperativeLevel.Exclusive| CooperativeLevel.Background);
            
            
            
             e = new Effect(joystick1, joystick0.GetEffects()[0].Guid, ep);        // Here we get some error
             

    }
    
    void Update()
    {
        //To see if something is going out.
        //Debug.Log(joystick0.GetCurrentState());
        
    }
}



