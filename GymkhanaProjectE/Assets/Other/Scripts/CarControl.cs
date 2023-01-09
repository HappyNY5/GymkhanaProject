using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheelColliders;

    [SerializeField] private Transform[] wheels;

    [SerializeField] private int maxSpeed;
    [SerializeField] private float motorForce;
    [SerializeField] private float steerAngle;

    private float horizInput, vertInput, curSteerAngle;
    private Rigidbody rigidBody;


    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Inputs();
    }

    private void Inputs()
    {
        horizInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");

        curSteerAngle = steerAngle * horizInput;

        wheelColliders[0].steerAngle = curSteerAngle;
        wheelColliders[1].steerAngle = curSteerAngle;

        UpdateWheels();

    }

    private void UpdateWheels()
    {
        for (int i = 0; i < 4; i++)
        {
            if(rigidBody.velocity.magnitude < maxSpeed)
                wheelColliders[i].motorTorque = motorForce * vertInput;
            else 
                wheelColliders[i].motorTorque = 0;
            // wheelColliders[i].rpm = motorForce;

            // Debug.Log(rigidBody.velocity.magnitude < maxSpeed);
            

            Vector3 pos;
            Quaternion rot;

            wheelColliders[i].GetWorldPose(out pos, out rot);       

            
            wheels[i].position = pos;
            wheels[i].rotation = rot;
        }
    }
}
