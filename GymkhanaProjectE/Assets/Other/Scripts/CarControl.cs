using System;
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

    [SerializeField] private float slipLim;

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

            if( rigidBody.velocity.magnitude < maxSpeed && vertInput > 0)
            {
                wheelColliders[i].motorTorque = motorForce * vertInput;
                wheelColliders[i].brakeTorque = 0;
            }
            else if(vertInput < 0)
                wheelColliders[i].brakeTorque = 100000;
            else 
            {
                wheelColliders[i].motorTorque = 0;
                wheelColliders[i].brakeTorque = 0;
            }

            WheelHit hit;
            if(wheelColliders[i].GetGroundHit(out hit))
                if(Mathf.Abs(hit.forwardSlip) + Mathf.Abs(hit.sidewaysSlip) >= slipLim)
                {
                    wheelColliders[i].GetComponentInChildren<ParticleSystem>().Play();
                }else{
                    wheelColliders[i].GetComponentInChildren<ParticleSystem>().Stop();
                }

            // Debug.Log(rigidBody.velocity.magnitude < maxSpeed);
            

            Vector3 pos;
            Quaternion rot;

            wheelColliders[i].GetWorldPose(out pos, out rot);       

            
            wheels[i].position = pos;
            wheels[i].rotation = rot;
        }
    }
}
