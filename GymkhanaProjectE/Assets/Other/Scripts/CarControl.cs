using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    [Header("---VISUAL PARTS---")]
    [SerializeField] private GameObject[] bodyModels;
    private uint curBodyIndex = 0; 
    [SerializeField] private Material bodyColorMaterial;
    [SerializeField] private Color[] bodyColors;
    private uint curBodyColorIndex = 0; 
    [SerializeField] private GameObject[] wheelModels;
    private uint curWheelsIndex = 0; 
    [SerializeField] private Color wheelColor;

    [Space]
    [Header("---WHEELS SETTINGS---")]
    [SerializeField] private WheelCollider[] wheelColliders;
    [SerializeField] private Transform[] wheels;
    [Space]
    [SerializeField] private Color smokeColor;
    [SerializeField] private Material smokeMaterial;
    [SerializeField] private Mesh smokeMesh;

    [Space]
    [Header("---OTHERS---")]
 
    [SerializeField] private int maxSpeed;
    [SerializeField] private float motorForce;
    [SerializeField] private float steerAngle;
    [SerializeField] private float slipLim;

    private float horizInput, vertInput, curSteerAngle;
    private Rigidbody rigidBody;
    private Vector3 startPos;

    private GameObject mainBodyModel;
    private GameObject mainWheelsModel;
    private Transform bodyParentTransform;

   void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        startPos = this.transform.position;

        bodyParentTransform = this.transform.GetChild(0);

        NextBody();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            this.transform.position = startPos;
            this.transform.rotation = Quaternion.Euler(Vector3.zero);
            rigidBody.velocity = Vector3.zero;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            SetSmokeSettings();
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            NextBody();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            NextBodyColor();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            NextWheels();
        }
    }

   

    void FixedUpdate()
    {
        Inputs();

        if(!Physics.Raycast(this.transform.position, Vector3.down, 1f))
            StableCar();        
    }


//=============================================================\\
    private void StableCar()
    {
        float stableForce = 6f;

        if(Mathf.Abs(this.transform.eulerAngles.x) >= 5)
        {
            Vector3 forceVector = Vector3.down * stableForce * (this.transform.eulerAngles.x - 180f);
            Vector3 posVector = new Vector3(this.transform.position.x + 1f * (float)Math.Sin(this.transform.eulerAngles.y * (Math.PI / 180)), this.transform.position.y, this.transform.position.z + 1.5f * (float)Math.Cos(this.transform.eulerAngles.y * (Math.PI / 180)));
            rigidBody.AddForceAtPosition(forceVector, posVector);
            Debug.DrawRay(posVector, forceVector, Color.red);
        }

        stableForce = 2.5f;
        
        if(Mathf.Abs(this.transform.eulerAngles.z) >= 5)
        {
            Vector3 forceVector = Vector3.down * stableForce * (this.transform.eulerAngles.z - 180f);
            Vector3 posVector = new Vector3(this.transform.position.x - 1f * (float)Math.Cos(this.transform.eulerAngles.y * (Math.PI / 180)), this.transform.position.y, this.transform.position.z + 1.5f * (float)Math.Sin(this.transform.eulerAngles.y * (Math.PI / 180)));
            rigidBody.AddForceAtPosition(forceVector, posVector);
            Debug.DrawRay(posVector, forceVector, Color.blue);
        }
    }
    private void NextBody(int chasableBodyIndex = -1)
    {
        //сделать выбор по номеру

        if(curBodyIndex + 1 <= bodyModels.Length - 1)
            curBodyIndex += 1;
        else 
            curBodyIndex = 0;

        mainBodyModel = bodyModels[curBodyIndex];

        if(bodyParentTransform.childCount != 0)
            Destroy(bodyParentTransform.GetChild(0).gameObject);
        Instantiate(mainBodyModel, bodyParentTransform);

        ChangeWheelsPos();
    }

    private void NextWheels(int chasableWheelsIndex = -1)
    {
        //сделать выбор по номеру

        if(curWheelsIndex + 1 <= wheelModels.Length - 1)
            curWheelsIndex += 1;
        else 
            curWheelsIndex = 0;

        mainWheelsModel = wheelModels[curWheelsIndex];


        
        for (int i = 0; i < 4; i++)
        {

            wheels[i].GetComponentInChildren<MeshFilter>().mesh = mainWheelsModel.GetComponentInChildren<MeshFilter>().sharedMesh;
            // Destroy(wheels[i].GetChild(0).gameObject);

            // GameObject wheel = Instantiate(mainWheelsModel, wheels[i].transform.position, Quaternion.Euler(this.transform.eulerAngles.x, 90 - (180 * -1 * (i%2)), this.transform.eulerAngles.z), wheels[i]);
        }
        

        
        
    }

    private void ChangeWheelsPos()
    {
        Vector3[] curWheelPos = mainBodyModel.GetComponent<WheelsPositions>().ReturnWheelsPosition();
    
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(curWheelPos[i]);
            wheelColliders[i].transform.localPosition = curWheelPos[i];
            wheelColliders[i].radius = bodyParentTransform.GetChild(0).GetComponent<WheelsPositions>().WheelsRadius;
            
            wheels[i].transform.localPosition = curWheelPos[i];
        }
    }

    private void NextBodyColor()
    {
        if(curBodyColorIndex + 1 <= bodyColors.Length - 1)
            curBodyColorIndex += 1;
        else 
            curBodyColorIndex = 0;

        bodyColorMaterial.color = bodyColors[curBodyColorIndex];

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

            if (rigidBody.velocity.magnitude < maxSpeed && vertInput > 0)
            {
                wheelColliders[i].motorTorque = motorForce * vertInput;
                wheelColliders[i].brakeTorque = 0;
            }
            else if (vertInput < 0)
                wheelColliders[i].brakeTorque = 100000;
            else
            {
                wheelColliders[i].motorTorque = 0;
                wheelColliders[i].brakeTorque = 0;
            }

            Vector3 pos;
            Quaternion rot;

            WheelHit hit;
            if (wheelColliders[i].GetGroundHit(out hit))
                if (Mathf.Abs(hit.forwardSlip) + Mathf.Abs(hit.sidewaysSlip) >= slipLim)
                {
                    wheelColliders[i].GetComponentInChildren<ParticleSystem>().Play();
                }
                else
                {
                    wheelColliders[i].GetComponentInChildren<ParticleSystem>().Stop();
                }

            wheelColliders[i].GetWorldPose(out pos, out rot);


            wheels[i].position = pos;
            wheels[i].rotation = rot;
        }
    }

    private void SetSmokeSettings()
    {
        smokeMaterial.color = smokeColor;
        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].GetComponentInChildren<ParticleSystemRenderer>().mesh = smokeMesh;
        }
        Debug.Log("AllChanges");
    }

    private void SetCarVisualSettings()
    {
        
        Debug.Log("All body Changes was applyed");
    }

}
