using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerV2 : MonoBehaviour
{
    [Header("Sphere controller")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float forwardPower = 8f, revercePower = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 9.8f, maxWheelTurn = 30f;   
    [SerializeField] private Transform[] wheelsModelsTransform;
    [SerializeField] private WheelsPositions wheelsPositions;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform groundRayPoint;
    [Space]

    [Header("---VISUAL PARTS---")]
    //body
    [SerializeField] public Mesh[] bodyModelsMesh;
    private int curBodyIndex = 0; 
    [SerializeField] private Material bodyColorMaterial;
    private uint curBodyColorIndex = 0; 

    //wheels
    [SerializeField] private Mesh[] wheelModelsMesh;
    private uint curWheelsModelsIndex = 0; 
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



    void Start()
    {
        rigidBody.transform.parent = null;
        startPos = transform.position;

        transform.GetChild(0).GetComponent<MeshRenderer>().material = bodyColorMaterial;
    }

    void Update()
    {
        SphereCarControl();

        InputManager();
    }

    private void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.transform.position = startPos;
            this.transform.rotation = Quaternion.Euler(Vector3.zero);
            rigidBody.velocity = Vector3.zero;

            // SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SetSmokeSettings();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextBody();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            NextBodyColor();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            NextWheelsModel();
        }
    }

    void FixedUpdate()
    {
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

        horizInput = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Vertical") > 0)
        {
            vertInput = Input.GetAxis("Vertical") * forwardPower;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            vertInput = Input.GetAxis("Vertical") * revercePower;
        }


        if (grounded)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, horizInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));


        wheelsModelsTransform[0].localRotation = Quaternion.Euler(wheelsModelsTransform[0].localRotation.eulerAngles.x, (horizInput * maxWheelTurn), wheelsModelsTransform[0].localRotation.eulerAngles.z);
        wheelsModelsTransform[1].localRotation = Quaternion.Euler(wheelsModelsTransform[0].localRotation.eulerAngles.x, (horizInput * maxWheelTurn), wheelsModelsTransform[0].localRotation.eulerAngles.z);
    }

    private void SetSmokeSettings()
    {
        smokeMaterial.color = colorsPalette[curSmokeColorIndex];
        for (int i = 0; i < 4; i++)
        {
            wheelsModelsTransform[i].GetComponentInChildren<ParticleSystemRenderer>().mesh = smokeMeshs[curSmokeMeshIndex];
        }
        Debug.Log("Smoke changed");
    }

    private void NextBody(int chasableBodyIndex = -1)
    {
        // //сделать выбор по номеру

        if(curBodyIndex + 1 <= bodyModelsMesh.Length - 1)
            curBodyIndex += 1;
        else 
            curBodyIndex = 0;

        transform.GetChild(0).GetComponent<MeshFilter>().mesh = bodyModelsMesh[curBodyIndex];

        Debug.Log($"NEW cur body index = {curBodyIndex}");

        ChangeWheelsPos();
    }

    private void NextWheelsModel(int chasableWheelsIndex = -1)
    {
        //сделать выбор по номеру

        if(curWheelsModelsIndex + 1 <= wheelModelsMesh.Length - 1)
            curWheelsModelsIndex += 1;
        else 
            curWheelsModelsIndex = 0;

        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponentInChildren<MeshFilter>().mesh = wheelModelsMesh[curWheelsModelsIndex];
        }
    }

    private void ChangeWheelsPos()
    {    
        for (int i = 0; i < 4; i++)
        {           
            wheelsModelsTransform[i].localPosition = WheelsPositions.wheelsPositions[bodyModelsMesh[curBodyIndex]][i];
            Debug.Log(bodyModelsMesh[curBodyIndex].name);
        }
    }

    private void NextBodyColor()
    {
        if(curBodyColorIndex + 1 <= colorsPalette.Length - 1)
            curBodyColorIndex += 1;
        else 
            curBodyColorIndex = 0;

        bodyColorMaterial.color = colorsPalette[curBodyColorIndex];
    }


}
