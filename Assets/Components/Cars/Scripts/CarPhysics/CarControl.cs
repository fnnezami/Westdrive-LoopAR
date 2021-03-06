using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private WheelCollider[] frontWheels;
    [SerializeField] private WheelCollider[] rearWheels;
    [SerializeField] private float _torque = 200f;
    [SerializeField] private float _maxSteerAngle = 30f;
    [SerializeField] private float maxBrakeTorque = 500f;
    [SerializeField] private bool allWheelDrive= false;
    [SerializeField] private bool rearBreakOnly = true;
    public float acceelelartionBoost = 1000f;
    [SerializeField] private float _maximumSpeed = 67f;  //meter per seconds
    private float _currentSpeed;

    public Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0);
    
    
    // Start is called before the first frame update
    void Start()
    {

        _rigidbody = this.gameObject.GetComponent<Rigidbody>();
        _rigidbody.centerOfMass += centerOfMassOffset;
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

      // Debug.Log("Speed Km/h: " + _currentSpeed * 3.6);
      _currentSpeed = _rigidbody.velocity.magnitude;



    }

    public void MoveVehicle(float accelerationInput, float brakeInput, float steeringInput)
    {
        foreach (var wheel in frontWheels)
        {
            TransferInputToWheels(wheel, accelerationInput,brakeInput, steeringInput);
        }

        foreach (var wheel in rearWheels)
        {
            TransferInputToWheels(wheel, accelerationInput, brakeInput);
        }

        //Debug.Log(_rigidbody.velocity.magnitude);
    }
    
    void TransferInputToWheels(WheelCollider wheelCol, float acceleration, float brake)
    {
        
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
        acceleration = Mathf.Clamp(acceleration, -1, 1);

        float thrustTorque = CalculateThrustTorque(acceleration);
        //wheelCol.motorTorque = acceelelartionBoost*   thrustTorque;
        wheelCol.motorTorque = Mathf.Clamp(thrustTorque * acceelelartionBoost,0, _maximumSpeed);
        /*Debug.Log( "Torque: " + wheelCol.motorTorque + ", rpm: " +    wheelCol.rpm);*/
        wheelCol.brakeTorque = brake;
        
    }

    void TransferInputToWheels(WheelCollider wheelCol, float acceleration, float brake, float steering)
    {
        acceleration = Mathf.Clamp(acceleration, -1, 1);
        //steering = Mathf.Clamp(steering, -1, 1) * maxSteerAngle;
        steering = steering * _maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;
//        Debug.Log("accel" + acceleration + " brake" + brake);
        if (allWheelDrive)
        {
            wheelCol.motorTorque = CalculateThrustTorque(acceleration);
        }
        
        if(!rearBreakOnly)
            wheelCol.brakeTorque = brake;
        
        wheelCol.steerAngle = steering;
        
    }


    private float CalculateThrustTorque(float acceleration)
    {
        float thrustTorque = 0;
        if (_currentSpeed < _maximumSpeed)
            thrustTorque = acceleration * _torque;
        return thrustTorque;
    }

    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    public float GetCurrentSpeedInKmH()
    {
        return _currentSpeed * 3.6f;
    }

    public float GetTorque()
    {
        return _torque;
    }

    public void SetTorque(float torque)
    {
        _torque = torque;
    }

    public void SetMaximumSpeed(float speedInKmh)
    {
        _maximumSpeed = speedInKmh/3.6f;
    }

    public float GetMaximumSpeed()
    {
        return _maximumSpeed;
    }

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    
}
