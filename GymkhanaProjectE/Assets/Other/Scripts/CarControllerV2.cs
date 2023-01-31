using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CarControllerV2 : MonoBehaviour
{
    [Header("Sphere controller")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float forwardPower = 8f, revercePower = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 9.8f, maxWheelTurn = 30f;   
    [SerializeField] private Transform[] wheelsModelsTransform;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform groundRayPoint;
    private float acceleration = 0f;
    private float oldSpeed = 0f;
    

    [Space]

    [Header("---VISUAL PARTS---")]
    //body
    [SerializeField] public Mesh[] bodyModelsMesh;
    [SerializeField] public float wheelRadius;
    private int curBodyMeshIndex = 0; 
    [SerializeField] private Material bodyColorMaterial;
    private int curBodyColorIndex = 0; 

    //wheels
    [SerializeField] private Mesh[] wheelModelsMesh;
    [SerializeField] private Material wheelsColorMaterial;
    private int curWheelsMeshIndex = 0; 
    private int curWheelsColorIndex = 0;

    //colors
    [SerializeField] private Color[] colorsPalette;


    [Space]
    [Header("---SMOKE VISUAL SETTINGS---")]
    [SerializeField] private Material smokeMaterial;
    [SerializeField] private Mesh[] smokeMeshs;
    private int curSmokeMeshIndex = 0;
    private int curSmokeColorIndex = 0;

    private Vector3 startPos;
    private float horizInput, vertInput;
    private bool grounded = false;
    private float curSpeed = 0;



    void Start()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        rigidBody.transform.parent = null;

        transform.GetChild(0).GetComponent<MeshRenderer>().material = bodyColorMaterial;

        SelectBodySettings();
        SelectWheelSettings();
        SelectSmokeSettings();
    }

    void Update()
    {
        curSpeed = rigidBody.velocity.magnitude;
        WheelRotating();

        SphereCarControl();
        SmokeWorking();
        InputManager();
    }

    private void WheelRotating()
    {
        float newAngle = (360 * curSpeed * Time.deltaTime)/(3.14f * wheelRadius * 2);

        newAngle = ((vertInput < 0) ? -newAngle : newAngle);

        for (int i = 0; i < 4; i++)
        {
            wheelsModelsTransform[i].localRotation = Quaternion.Euler(0, wheelsModelsTransform[i].localRotation.eulerAngles.y, wheelsModelsTransform[i].localRotation.eulerAngles.z + newAngle);
        }
    }

    private void SmokeWorking()
    {
        if(acceleration > 10 || (horizInput != 0 && curSpeed > 1))
        { 
            for (int i = 0; i < 4; i++)
            {
                wheelsModelsTransform[i].GetChild(0).GetComponent<ParticleSystem>().emissionRate = 35;
            }
        }else{
            for (int i = 0; i < 4; i++)
            {
                wheelsModelsTransform[i].GetChild(0).GetComponent<ParticleSystem>().emissionRate = 0;
            }
        }
    }

    private void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }       
    }

    void FixedUpdate()
    {
        acceleration = (curSpeed - oldSpeed)/ Time.fixedDeltaTime;
        oldSpeed = curSpeed;

        RaycastHit hit;
        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, 0.5f, groundLayerMask))
        {
            grounded = true;
            rigidBody.drag = 3;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }else{
            grounded = false;
            rigidBody.drag = 0.1f;
        }

        if(grounded)
            rigidBody.AddForce(transform.forward * vertInput * 1000f);
        else
            rigidBody.AddForce(Vector3.up * -gravityForce * 100f);  
    }

    private void SphereCarControl()
    {
        transform.position = rigidBody.transform.position;
        if(!SDKusing.isAndro)
        {
            
            horizInput = Input.GetAxis("Horizontal");



            if (Input.GetAxis("Vertical") > 0)
            {
                vertInput = Input.GetAxis("Vertical") * forwardPower;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                vertInput = Input.GetAxis("Vertical") * revercePower;
            }
        }


Debug.Log($"Vert = {vertInput} Horiz = {horizInput}");
        if (grounded)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, horizInput * turnStrength * (vertInput/forwardPower) * Time.deltaTime, 0f));


        wheelsModelsTransform[0].localRotation = Quaternion.Euler(wheelsModelsTransform[0].localRotation.eulerAngles.x,90 + (horizInput * maxWheelTurn), wheelsModelsTransform[0].localRotation.eulerAngles.z);
        wheelsModelsTransform[1].localRotation = Quaternion.Euler(wheelsModelsTransform[1].localRotation.eulerAngles.x,-90 +(horizInput * maxWheelTurn), wheelsModelsTransform[1].localRotation.eulerAngles.z);
    }

    private void SelectSmokeSettings(int newSmokeMeshIndex = 0, int newSmokeColorIndex = 0)
    { 
        if((newSmokeMeshIndex > smokeMeshs.Length - 1) || (newSmokeColorIndex > colorsPalette.Length - 1))
        {
            Debug.Log($"new index smokes out of range\n current mesh,color {curSmokeMeshIndex}, {curSmokeColorIndex}\nnew mesh,color {newSmokeMeshIndex}, {newSmokeColorIndex}");
            return;
        }

        curSmokeMeshIndex = newSmokeMeshIndex;
        curSmokeColorIndex = newSmokeColorIndex;

        smokeMaterial.color = colorsPalette[newSmokeColorIndex];
        for (int i = 0; i < 4; i++)
        {
            wheelsModelsTransform[i].GetComponentInChildren<ParticleSystemRenderer>().mesh = smokeMeshs[newSmokeMeshIndex];
        }
        Debug.Log($"Smoke changed on NAME: {smokeMeshs[curSmokeMeshIndex].name}, COLOR: {colorsPalette[newSmokeColorIndex]}");
    }

    private void SelectBodySettings(int newBodyMeshIndex = 0, int newBodyColorIndex = 0)
    { 
        if((newBodyMeshIndex > bodyModelsMesh.Length - 1) || (newBodyColorIndex > colorsPalette.Length - 1))
        {
            Debug.Log($"new index out of range\n current mesh,color {curBodyMeshIndex}, {curBodyColorIndex}\nnew mesh,color {newBodyMeshIndex}, {newBodyColorIndex}");
            return;
        }

        curBodyMeshIndex = newBodyMeshIndex;
        curBodyColorIndex = newBodyColorIndex;

        transform.GetChild(0).GetComponent<MeshFilter>().mesh = bodyModelsMesh[curBodyMeshIndex];
        bodyColorMaterial.color = colorsPalette[curBodyColorIndex];
        
        ChangeWheelsPos();
        
        Debug.Log($"Body settings changed on NAME: {curBodyMeshIndex}, COLOR: {curBodyColorIndex}");
    }

    private void ChangeWheelsPos()
    {    
        for (int i = 0; i < 4; i++)
        {           
            wheelsModelsTransform[i].localPosition = WheelsPositions.wheelsPositions[bodyModelsMesh[curBodyMeshIndex]][i];
            wheelsModelsTransform[i].localRotation = Quaternion.Euler(0, 90 * Mathf.Pow(-1, i % 2), 0);
        }
        Debug.Log($"wheels position changed");
    }


    private void SelectWheelSettings(int newWheelsMeshIndex = 0, int newWheelsColorIndex = 0)
    { 
        if((newWheelsMeshIndex > wheelModelsMesh.Length - 1) || (newWheelsColorIndex > colorsPalette.Length - 1))
        {
            Debug.Log($"new index WHEELS out of range\n current mesh,color {curBodyMeshIndex}, {curBodyColorIndex}\nnew mesh,color {newWheelsMeshIndex}, {newWheelsColorIndex}");
            return;
        }

        curWheelsMeshIndex = newWheelsMeshIndex;
        curWheelsColorIndex = newWheelsColorIndex;

        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponentInChildren<MeshFilter>().mesh = wheelModelsMesh[curWheelsMeshIndex];
            wheelsColorMaterial.color = colorsPalette[curWheelsColorIndex];
        }
        
        Debug.Log($"Wheels settings changed on NAME: {curWheelsMeshIndex}, COLOR: {curWheelsColorIndex}");
    }

    public void GasInput(int forward)
    {
        vertInput = forward * forwardPower;
    }

    public void RotateInput(int rotate)
    {
        horizInput = rotate;   
        
    }
}
