using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private static WheelCollider[] wheelColliders = new WheelCollider[4];
    [SerializeField] private Transform[] wheels;
    [Space]
    [SerializeField] private Color smokeColor;
    [SerializeField] private Material smokeMaterial;
    [SerializeField] private Mesh smokeMesh;
    private float precentForAxle = 0.6f;

    [Space]
    [Header("---OTHERS---")]
 
    [SerializeField] private float motorForce;
    [SerializeField] private float breakingForce;
    [SerializeField] private float steerAngle;
    [SerializeField] private float slipLim;
    [SerializeField] private Transform centerOfMass;
    
    public static int MaxSpeed = 60;

    

    private float horizInput, vertInput, curSteerAngle;
    private Rigidbody rigidBody;
    private Vector3 startPos;

    private GameObject mainBodyModel;
    private GameObject mainWheelsModel;
    private Transform bodyParentTransform;
    public static bool InBreakingZone = false;

    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass.localPosition;
        startPos = this.transform.position;

        bodyParentTransform = this.transform.GetChild(0);

        FindWheelColliders();

        NextBody();
    }

    private void FindWheelColliders()
    {
        for (int i = 0; i < 4; i++)
        {
            Transform wheel = this.transform.GetChild(2).transform.GetChild(i);
            wheelColliders[i] = wheel.GetComponent<WheelCollider>();
            Debug.Log(wheel.name);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            this.transform.position = startPos;
            this.transform.rotation = Quaternion.Euler(Vector3.zero);
            rigidBody.velocity = Vector3.zero;
        
            // SceneManager.LoadScene(0);
        
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
        }

        stableForce = 2.5f;
        
        if(Mathf.Abs(this.transform.eulerAngles.z) >= 5)
        {
            Vector3 forceVector = Vector3.down * stableForce * (this.transform.eulerAngles.z - 180f);
            Vector3 posVector = new Vector3(this.transform.position.x - 1f * (float)Math.Cos(this.transform.eulerAngles.y * (Math.PI / 180)), this.transform.position.y, this.transform.position.z + 1.5f * (float)Math.Sin(this.transform.eulerAngles.y * (Math.PI / 180)));
            rigidBody.AddForceAtPosition(forceVector, posVector);
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


    public void Inputs()
    {
        
        horizInput = Input.GetAxis("Horizontal");

        // horizInput = Input.acceleration.x;


        bool handbreak = Input.GetKey(KeyCode.Space);
        vertInput = ((float)((1 - Math.Pow(rigidBody.velocity.magnitude / MaxSpeed, 4)))) * Input.GetAxis("Vertical");
        // Debug.Log(vertInput);
        // vertInput = Input.GetAxis("Vertical");
        
        

        curSteerAngle = steerAngle * horizInput;

        wheelColliders[0].steerAngle = curSteerAngle;
        wheelColliders[1].steerAngle = curSteerAngle;

        UpdateWheels(handbreak);
    }

    private void UpdateWheels(bool _handbr)
    {
        Vector2 vec1 = new Vector2(rigidBody.velocity.z, rigidBody.velocity.x);

        Vector3 vec21 = new Vector3((float)Math.Sin(this.transform.eulerAngles.y * (Math.PI / 180)), 0,(float)Math.Cos(this.transform.eulerAngles.y * (Math.PI / 180)));
        Vector2 vec22 = new Vector2(vec21.z, vec21.x);

        float angle = Vector2.Angle(vec1.normalized, vec22.normalized);
        float angleCoefficient = angle/360;

        Debug.Log(vertInput + angleCoefficient);


        for (int i = 0; i < 3; i+=2)
        {
// Debug.Log($"Wheel {i + 1} RPM = {wheelColliders[i].rpm}");

            WheelHit hit;
            if (wheelColliders[i].GetGroundHit(out hit))
            {
                if (Mathf.Abs(hit.forwardSlip) + Mathf.Abs(hit.sidewaysSlip) >= slipLim)
                {
                    wheelColliders[i].GetComponentInChildren<ParticleSystem>().emissionRate = 30;
                    wheelColliders[i+1].GetComponentInChildren<ParticleSystem>().emissionRate = 30;
                }
                else
                {
                    wheelColliders[i].GetComponentInChildren<ParticleSystem>().emissionRate = 0;
                    wheelColliders[i+1].GetComponentInChildren<ParticleSystem>().emissionRate = 0;
                }
            }

            if (!_handbr && !InBreakingZone)
            {
                wheelColliders[i].brakeTorque = 0;
                wheelColliders[i+1].brakeTorque = 0;
                wheelColliders[i].motorTorque = (MaxSpeed * motorForce * (vertInput + angleCoefficient )) / (((float)Math.PI) * wheelColliders[i].radius * 2);
                wheelColliders[i+1].motorTorque = (MaxSpeed * motorForce * (vertInput + angleCoefficient )) / (((float)Math.PI) * wheelColliders[i].radius * 2);
            }
            
            if (_handbr && i>=2)
            {
                wheelColliders[i].motorTorque = 0;
                wheelColliders[i+1].motorTorque = 0;
                wheelColliders[i].brakeTorque = 3000000;
                wheelColliders[i+1].brakeTorque = 3000000;
            }

            if(InBreakingZone)
            {
                wheelColliders[i].motorTorque = 0;
                wheelColliders[i].brakeTorque = breakingForce;

                wheelColliders[i+1].motorTorque = 0;
                wheelColliders[i+1].brakeTorque = breakingForce;
Debug.Log("Break");
            }

            if(vertInput < 0)
            {
                wheelColliders[i].brakeTorque = breakingForce;
                wheelColliders[i+1].brakeTorque = breakingForce;
            }

            Vector3 pos;
            Quaternion rot;

            Vector3 pos1;
            Quaternion rot1;

            wheelColliders[i].GetWorldPose(out pos, out rot);
            wheelColliders[i+1].GetWorldPose(out pos1, out rot1);

            wheels[i].position = pos;
            wheels[i].rotation = rot;

            wheels[i+1].position = pos1;
            wheels[i+1].rotation = rot1;
        }
    }

    private void ChangeParticlePlay(bool enable)
    {
        
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



    public static void DriftMode(bool enabled)
    {
        for (int i = 0; i < 4; i++)
        {
            WheelFrictionCurve sf = wheelColliders[i].sidewaysFriction;

            sf.stiffness = (enabled ? 0.5f : 3f);

            wheelColliders[i].sidewaysFriction = sf; 
        }
    }
}
