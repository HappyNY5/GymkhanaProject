using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraInOutGarage : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camGarage;
    [SerializeField] private CinemachineVirtualCamera camGame;
    [SerializeField] private GameObject car;
    
    private Vector3 carPos = new Vector3(13.4f, 146.276f, 94.3f);
    private Vector3 carStartPos = new Vector3(-5f, 3f, 5f);
    private bool inGarage = false;

    public void InGarage()
    {
        //Сменить камеру и переместить машину
        camGarage.Priority = 10;
        camGarage.GetCinemachineComponent<>

        camGame.Priority = 0;


        car.transform.position = carPos;
    }

    public void OutGarage()
    {
        //Сменить камеру и переместить машину
        camGarage.Priority = 0;
        camGame.Priority = 10;

        car.transform.position = carStartPos;
    }
}
