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

            _deviceList = _directInput.GetDevices(DeviceType.Driving, DeviceEnumerationFlags.ForceFeedback);
            
            
            // interating through the devices to see if everything is up and running
            
            foreach (var device in _deviceList)
            {
                Debug.Log(device.ProductName + " " + device.InstanceGuid + " " +  device.Usage);
            }
        
            // we have two steering wheels , actually only 0 seems be manipulatable. We stick with 0
            joystick0 = new Joystick(_directInput, _deviceList[0].InstanceGuid);    
            joystick1 = new Joystick(_directInput, _deviceList[1].InstanceGuid);
        
            //here come the test effects...
        
            ep.Flags = EffectFlags.Cartesian | EffectFlags.ObjectOffsets;
            ep.Directions = new int[1] { 0 };
            ep.Gain = 5000;
            ep.Duration = int.MaxValue;
            ep.SamplePeriod = joystick0.Capabilities.ForceFeedbackSamplePeriod;
            CF.Magnitude = 10000;
            ep.Parameters = CF;
        
            EffectFile eff= new EffectFile();
    
            eff.Parameters = ep;

            // Debug.Log("____Effect List_____");


            // var effs = joystick1.GetEffects();

            //foreach (var ef in effs)
            //{
            //EffectObject feo = new EffectObject(
            //fe.EffectGuid,
            //fe.EffectStruct,
            //   joystick);
            // Debug.Log(ef.Type + "  ef parameters " + ef.StaticParameters);
            //}
            var pointer = joystick1.NativePointer;
            
            joystick1.SetCooperativeLevel(GetActiveWindow(), CooperativeLevel.Exclusive| CooperativeLevel.Background);
            // e = new Effect(joystick1, _deviceList[0].ForceFeedbackDriverGuid, ep);

            //EffectObject effectObject;


            //joystick1.Get


            //Debug.Log(e.Status);

    }

    // Update is called once per frame
    void Update()
    {
        
        
       // Effect e = new Effect(joystick1,joystick1.Information.InstanceGuid, ep);
        //e.Start();
        //Debug.Log(joystick3.GetCurrentState());
        //Debug.Log(joystick1.GetForceFeedbackState());
       // var data = joystick1.GetBufferedData();

        //foreach (var item in data)
       // {
            //Debug.Log(item.ToString() + " " +  item.Value);
       // }

        //Debug.Log("joy2 " +joystick1.GetForceFeedbackState());
        //Debug.Log("joy3 " +joystick2.GetForceFeedbackState());
    }
}



