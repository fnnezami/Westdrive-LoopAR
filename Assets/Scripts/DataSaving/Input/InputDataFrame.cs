using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this is a specialized LoopAR input data Frame. It stores the Input to the Vehicle, like steering wheel input,brake Input and so on.  Feel free to copy, but you might need to edit it.
[Serializable]public class InputDataFrame
{
    public double TimeStamp;
    public bool ReceivedInput;
    public float SteeringInput;
    public float AcellerationInput;
    public float BrakeInput;

    public Vector3 CarPosition;
    public Vector3 LocalCarPosition;
    public Vector3 CarRotation;
    public Quaternion CarRotationQuaternion;
}
